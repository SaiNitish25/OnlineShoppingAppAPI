using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.DomainModels;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Controllers
{
    
    [ApiController]
    public class ShoppingController : Controller
    {
        private readonly IShoppingService shoppingService;
        public ShoppingController(IShoppingService shopping)
        {
            this.shoppingService = shopping;
        }

        [HttpGet]
        [Route("[Controller]/customers")]
        public async Task<ActionResult> GetCustomers()
        {
            var customers = await shoppingService.ViewCustomers();
            return Ok(customers);
        }

        [HttpPost]
        [Route("[Controller]/register")]
        public async Task<ActionResult> RegisterCustomer([FromBody] RegisterCustomer newCust) 
        {
            if (newCust.Password == newCust.ConfirmPassword)
                await shoppingService.RegisterCustomer(newCust);
            else
                return Ok(false);
            return Ok(true);
        }

        [HttpPost]
        [Route("[Controller]/login")]
        public async Task<ActionResult> CheckLogin([FromBody] LoginCustomer cust)
        {
            if(await shoppingService.CheckLogin(cust))
            {
                return Ok("true");
            }
            return Ok("false");
        }

        [HttpPut]
        [Route("[Controller]/forgotPassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ForgotPassword forgotPassword)
        {
            if (forgotPassword.Password == forgotPassword.ConfirmPassword)
                if (await shoppingService.ChangePassword(forgotPassword))
                    return Ok(true);
            return Ok(false);
          
        }


        [HttpPost]
        [Route("[Controller]/add")]
        public async Task<ActionResult> AddProduct([FromBody] Product product)
        {
            if (await shoppingService.CheckAdmin(product.CustomerId))
                await shoppingService.AddNewProduct(product);
            else
                return Ok(false);
            return Ok(true);
        }

        [HttpGet]
        [Route("[Controller]/search/{productName}")]
        public async Task<ActionResult> SearchProduct([FromRoute] string productName)
        {
           var product= await shoppingService.SearchProduct(productName);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("[Controller]/all")]
        public async Task<ActionResult> ViewAllProducts()
        {
            var products = await shoppingService.ViewAllProducts();
            if (products != null)
            {
                return Ok(products);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("[Controller]/delete/{id:length(24)}/{cid:length(24)}")]

        public async Task<ActionResult> DeleteProduct([FromRoute] string id,[FromRoute] string cid)
        {
            if (await shoppingService.CheckAdmin(cid))
            {
                if (await shoppingService.DeleteProduct(id))
                    return Ok(true);
                else
                    return Ok(false);
            }
            else
            {
                return Ok(false);
            }
        }

        [HttpPut]
        [Route("[Controller]/update/{id:length(24)}/{cid:length(24)}")]
        public async Task<ActionResult> UpdateProductStatus([FromRoute] string id,[FromRoute] string cid)
        {
            if (await shoppingService.CheckAdmin(cid))
            {
                if (await shoppingService.UpdateProductStatus(id))
                    return Ok(true);
                else
                    return Ok(false);
            }
            else
            {
                return Ok(false);
            }
        }

        [HttpPost]
        [Route("[Controller]/orderProduct")]
        public async Task<ActionResult> OrderProduct([FromBody] Orders order)
        {
           
                if (await shoppingService.OrderProduct(order))
                    return Ok(true);
                else
                    return Ok(false);
            
            
        }

        [HttpGet]
        [Route("[Controller]/orders/{id:length(24)}")]
        public async Task<ActionResult> ViewAllOrders([FromRoute] string id)
        {
            if(await shoppingService.CheckAdmin(id))
            {
                var orders = await shoppingService.ViewAllOrders();
                return Ok(orders);
            }
            return Content("User has No admin access");
        }

        [HttpGet]
        [Route("[Controller]/product/{id:length(24)}")]
        public async Task<ActionResult> viewProductById([FromRoute] string id)
        {
            var product = await shoppingService.ViewProductById(id);
            return Ok(product);
        }

        [HttpGet]
        [Route("[Controller]/customer/{email}")]
        public async Task<ActionResult> viewCustomer([FromRoute] string email)
        {
            var cust = await shoppingService.ViewCustomerByEmail(email);
            return Ok(cust);
        }

        [HttpGet]
        [Route("[Controller]/checkAdmin/{id:length(24)}")]
        public async Task<ActionResult> CheckAdmin([FromRoute] string id)
        {
            if(await shoppingService.CheckAdmin(id))
            {
                return Ok(true);
            }
            return Ok(false);
        }



    }
}
