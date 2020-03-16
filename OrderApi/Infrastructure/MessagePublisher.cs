using CustomerApi.Models;
using EasyNetQ;
using SharedProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure
{
    public class MessagePublisher : IMessagePublisher, IDisposable
    {
        IBus bus;

        public MessagePublisher(string connectionString)
        {
            bus = RabbitHutch.CreateBus(connectionString);
        }
        public void Dispose()
        {
            bus.Dispose();
        }

        public void PublishOrderStatusChangedMessage(int? customerId, IEnumerable<Order.OrderLine> orderLines, string topic)
        {
            var message = new OrderStatusChangedMessage
            {
                CustomerID = customerId,
                //OrderLines = orderLines
            };

            bus.Publish(message, topic);
        }

        public Customer RequestCustomer(int id)
        {
            var cr = CustomerRequest { id = id };
            var response = bus.Request<CustomerRequest, Customer>(cq);
            return response;
        }
    }
}
