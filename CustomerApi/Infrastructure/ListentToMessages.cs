using System;
using System.Threading;
using CustomerApi.Data;
using SharedModels;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using CustomerApi.Models;

namespace CustomerApi.Infrastructure
{
    public class ListentToMessages
    {
        IServiceProvider provider;
        string connectionString;

        // The service provider is passed as a parameter, because the class needs
        // access to the product repository. With the service provider, we can create
        // a service scope that can provide an instance of the product repository.
        public ListentToMessages(IServiceProvider provider, string connectionString)
        {
            this.provider = provider;
            this.connectionString = connectionString;
        }

        public void Start()
        {
            using (var bus = RabbitHutch.CreateBus(connectionString))
            {
                //bus.Respond<CustomerRequest, ReturnedCustomer>(request => new ReturnedCustomer { customer = HandleCustomerRequest(request.Id) });
                bus.Respond<Models.CustomerRequest, SharedModels.Customer>(request => HandleCustomerRequest(request.ID));
                //bus.Respond<CustomerRequest, Customer>(request => new Customer { Id = 1, Name = "customer1", Email = "e1@mail.com", PhoneNumber = "1234", BillingAddress = "billingAddress1", ShippingAddress = "shippingAddress1", CreditStanding = true });

                // block the thread so that it will not exit and stop subscribing.
                lock (this)
                {
                    Monitor.Wait(this);
                }
            }
        }
        private SharedModels.Customer HandleCustomerRequest(int id)
        {
            // A service scope is created to get an instance of the product repository.
            // When the service scope is disposed, the product repository instance will
            // also be disposed.
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var customerRepos = services.GetService<IRepository<SharedModels.Customer>>();
                return customerRepos.Get(id);
            }
        }
    }
}
