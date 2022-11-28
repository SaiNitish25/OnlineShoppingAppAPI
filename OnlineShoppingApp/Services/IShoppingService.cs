using OnlineShoppingApp.DomainModels;
using OnlineShoppingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Services
{
    public interface IShoppingService
    {
        Task<bool> CheckLogin(LoginCustomer cust);

        Task<List<Customers>> ViewCustomers();

        Task<bool> RegisterCustomer(RegisterCustomer customer);

        Task<bool> ChangePassword(ForgotPassword forgotPassword);
        Task<List<Products>> ViewAllProducts();

        Task<Products> SearchProduct(string productName);

        Task<bool> CheckAdmin(string id);

        //Admin Access Method
        Task<bool> AddNewProduct(Product product);

        Task<bool> UpdateProductStatus(string productId);

        Task<bool> DeleteProduct(string productId);

        //IN UI When Order clicked the qunatity should be reduced
        Task<bool> UpdateQuantity(string productId);

        Task<bool> OrderProduct(Orders order);

        Task<List<Orders>> ViewAllOrders();

        Task<Products> ViewProductById(string id);
        Task<Customers> ViewCustomerByEmail(string email);





    }
}
