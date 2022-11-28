using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OnlineShoppingApp.DomainModels;
using OnlineShoppingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Services
{
    public class ShoppingService : IShoppingService
    {
        private readonly IConfiguration _configuration;

        private readonly MongoClient dbClient;

       
        public ShoppingService(IConfiguration configuration)
        {

            this._configuration = configuration;
            this.dbClient = new MongoClient(_configuration.GetConnectionString("ShoppingAppConn"));
        }
        
        public async Task<bool> AddNewProduct(Product product)
        {
            Products prod = new Products();
            prod.ProductName = product.ProductName;
            prod.ProductDescription = product.ProductDescription;
            prod.ProductStatus = product.ProductStatus;
            prod.Price = product.Price;
            prod.Features = product.Features;
            Quantity quan = new Quantity();

            await this.dbClient.GetDatabase("Shopping").GetCollection<Products>("Products").InsertOneAsync(prod);
            quan.ProductId = prod.Id;
            quan.quantity = product.Quantity;
            await this.dbClient.GetDatabase("Shopping").GetCollection<Quantity>("Quantity").InsertOneAsync(quan);
            return true;
        }

        public async Task<bool> ChangePassword(ForgotPassword forgotPassword)
        {
            //MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ShoppingAppConn"));
            var filter = Builders<Customers>.Filter.Eq("email", forgotPassword.Email);
            var update = Builders<Customers>.Update.Set("password", forgotPassword.Password);
            var dbList = await dbClient.GetDatabase("Shopping").GetCollection<Customers>("Customers")
               .Find(x => x.Email == forgotPassword.Email ).FirstOrDefaultAsync();
            if (dbList != null)
            {
                await this.dbClient.GetDatabase("Shopping").GetCollection<Customers>("Customers").UpdateOneAsync(filter, update);
                return true;
            }
            return false;

        }

        public async  Task<bool> CheckAdmin(string id)
        {
            var dbList = await dbClient.GetDatabase("Shopping").GetCollection<Admins>("Admins")
               .Find(x=>x.CustomerId==id).FirstOrDefaultAsync();
            if (dbList != null)
                return true;
            return false;
        }

        public async Task<bool> CheckLogin(LoginCustomer cust)
        {
           // MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ShoppingAppConn"));
            var dbList = await dbClient.GetDatabase("Shopping").GetCollection<Customers>("Customers")
                .Find(x => x.Email == cust.Email && x.Password == cust.Password).FirstOrDefaultAsync();
            if (dbList != null)
            {
                return true;
            }
            return false;
            
            
        }

        public async Task<bool> DeleteProduct(string productId)
        {
            var product = dbClient.GetDatabase("Shopping").GetCollection<Products>("Products").AsQueryable();

            var quan = dbClient.GetDatabase("Shopping").GetCollection<Quantity>("Quantity").AsQueryable();
            
            if (product.Where(x => x.Id == productId).FirstOrDefault() != null)
            {
                await dbClient.GetDatabase("Shopping").GetCollection<Products>("Products").DeleteOneAsync(x => x.Id == productId);
                await dbClient.GetDatabase("Shopping").GetCollection<Quantity>("Quantity").DeleteOneAsync(x => x.Id == productId);
            }
            else
                return false;
            return true;


        }

        public async  Task<bool> RegisterCustomer(RegisterCustomer newCust)
        {
            var customer = new Customers();
            
            customer.FirstName = newCust.FirstName;
            customer.LastName = newCust.LastName;
            customer.Password = newCust.Password;
            customer.Email = newCust.Email;
            customer.ContactNumber = newCust.ContactNumber;
           // MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ShoppingAppConn"));
             await dbClient.GetDatabase("Shopping").GetCollection<Customers>("Customers").InsertOneAsync(customer);
            return true;

        }

        public async Task<Products> SearchProduct(string productName)
        {
            //var dbList=await dbClient.GetDatabase("Shopping").GetCollection<Products>("Products").FindAsync(x=>x.ProductName==productName);
            //return dbList;
            var product = dbClient.GetDatabase("Shopping").GetCollection<Products>("Products").AsQueryable();
            return product.Where(x => x.ProductName == productName).FirstOrDefault();
        }

        public async Task<bool> UpdateProductStatus(string productId)
        {
            var status="";
            var product = dbClient.GetDatabase("Shopping").GetCollection<Products>("Products").AsQueryable();
            if (product.Where(x => x.Id == productId).FirstOrDefault() != null)
            {
                var filter = Builders<Products>.Filter.Eq("Id", productId);
                
                var productQuantity=dbClient.GetDatabase("Shopping").GetCollection<Quantity>("Quantity").AsQueryable();
                var quan = productQuantity.Where(x => x.ProductId == productId).FirstOrDefault();
                if (quan.quantity == 0)
                    status = "OUT OF STOCK";
                else
                    status = "HURRY UP TO PURCHASE";
                var update = Builders<Products>.Update.Set("ProductStatus", status);
                

                await this.dbClient.GetDatabase("Shopping").GetCollection<Products>("Products").UpdateOneAsync(filter, update);
                
            }
                
            else
                return false;
            return true;
        }

        public  async Task<List<Products>> ViewAllProducts()
        {
            var products = dbClient.GetDatabase("Shopping").GetCollection<Products>("Products").AsQueryable();
            return await products.ToListAsync();
        }

        public async Task<List<Customers>> ViewCustomers()
        {
            //MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("ShoppingAppConn"));
            var dbList = dbClient.GetDatabase("Shopping").GetCollection<Customers>("Customers").AsQueryable();
            return await dbList.ToListAsync();
        }

        public async   Task<bool> UpdateQuantity(string productId)
        {
            var prodQuan= dbClient.GetDatabase("Shopping").GetCollection<Quantity>("Quantity").AsQueryable();
           var quan= prodQuan.Where(x => x.ProductId == productId).FirstOrDefault();
            if (quan.quantity>0)
            {
                var filter = Builders<Quantity>.Filter.Eq("ProductId", productId);
                var update = Builders<Quantity>.Update.Set("quantity", quan.quantity - 1);
                await this.dbClient.GetDatabase("Shopping").GetCollection<Quantity>("Quantity").UpdateOneAsync(filter, update);
            }
            else
                return false;
            return true;


        }

        public async Task<bool> OrderProduct(Orders order)
        {
            var date = DateTime.UtcNow;
            order.Date = date;
            if (await UpdateQuantity(order.ProductId))
            {
                await this.dbClient.GetDatabase("Shopping").GetCollection<Orders>("Orders").InsertOneAsync(order);
                return true;
            }
            return false;
        }

        public async Task<List<Orders>> ViewAllOrders()
        {
            var orders= dbClient.GetDatabase("Shopping").GetCollection<Orders>("Orders").AsQueryable();
            return await orders.ToListAsync();

        }

        public async Task<Products> ViewProductById(string id)
        {
            var product = dbClient.GetDatabase("Shopping").GetCollection<Products>("Products").AsQueryable();
            return product.Where(x => x.Id==id).FirstOrDefault();
        }
        public async Task<Customers> ViewCustomerByEmail(string email)
        {
            var cust = dbClient.GetDatabase("Shopping").GetCollection<Customers>("Customers").AsQueryable();
           
            return cust.Where(x => x.Email== email).FirstOrDefault();
        }
    }
}
