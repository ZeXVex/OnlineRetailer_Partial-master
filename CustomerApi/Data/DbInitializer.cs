using CustomerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data
{
    public class DbInitializer : IDbInitializer
    {
        void IDbInitializer.Initialize(CustomerApiContext dbContext)
        {
            
        }

        void IDbInitializer.Initializer(CustomerApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Customers.Any())
            {
                // Check if DB has been seeded.
                return;
            }

            List<Customer> customers = new List<Customer>
            {
                new Customer { ID = 1, Name = "jokes", Email = "jokes@gmail.com", Phone = "5849603035", BillingAddress = "Address1", ShippingAddress = "Address1", creditStanding = true},
                new Customer { ID = 2, Name = "TheSpade", Email = "TheSpade@hotmail.dk", Phone = "59409864", BillingAddress = "Address2", ShippingAddress = "Address2", creditStanding = false}
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
