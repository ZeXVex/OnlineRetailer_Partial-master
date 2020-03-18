using RestSharp;
using SharedModels;
using System;

namespace OrderApi.Infrastructure
{
    public class CustomerServieGateway : IServiceGateway<Customer>
    {
        Uri customerServiceBaseUrl;

        public CustomerServieGateway(Uri baseUrl)
        {
            customerServiceBaseUrl = baseUrl;
        }
        public Customer Get(int ID)
        {
            RestClient c = new RestClient();
            c.BaseUrl = customerServiceBaseUrl;

            var request = new RestRequest(ID.ToString(), Method.GET);
            var response = c.Execute<Customer>(request);
            var customer = response.Data;
            return customer;
        }
    }
}
