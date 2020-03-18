using RestSharp;
using SharedModels;
using System;

namespace OrderApi.Infrastructure
{
    public class ProductsServiceGateway : IServiceGateway<Product>
    {
        Uri productServiceBaseUrl;

        public ProductsServiceGateway(Uri baseUrl)
        {
            productServiceBaseUrl = baseUrl;
        }
        public Product Get(int ID)
        {
            RestClient c = new RestClient();
            c.BaseUrl = productServiceBaseUrl;

            var request = new RestRequest(ID.ToString(), Method.GET);
            var response = c.Execute<Product>(request);
            var orderedProduct = response.Data;
            return orderedProduct;
        }
    }
}
