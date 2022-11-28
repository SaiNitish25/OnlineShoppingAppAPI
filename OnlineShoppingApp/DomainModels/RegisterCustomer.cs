using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingApp.DomainModels
{
    public class RegisterCustomer
    {
       
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string LoginId { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public long ContactNumber { get; set; }
    }
}
