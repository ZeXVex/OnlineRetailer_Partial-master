using EasyNetQ;
using SharedModels;
using System;
using System.Collections.Generic;

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
        void IMessagePublisher.PublishOrderStatusChangedMessage(int? customerId, IEnumerable<Order.OrderLine> orderLines, string topic)
        {
            var message = new OrderStatusChangedMessage
            {
                CustomerID = customerId,
                //OrderLines = orderLines
            };

            bus.Publish(message, topic);
        }
        Customer IMessagePublisher.RequestCustomer(int id)
        {
            var cr = new CustomerRequest { ID = id };
            var response = bus.Request<CustomerRequest, SharedModels.Customer>(cr);
            return response;
        }
    }
}
