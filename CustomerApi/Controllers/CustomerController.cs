using CustomerApi.Data;
using CustomerApi.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<Customer> repository;
        public CustomerController(IRepository<Customer> repo)
        {
            repository = repo;
        }

        // GET: orders
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return repository.GetAll();
        }

        // GET orders/5
        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST orders
        [HttpPost]
        public IActionResult Post([FromBody]Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            // Call ProductApi to get the product ordered
            RestClient c = new RestClient();
            // You may need to change the port number in the BaseUrl below
            // before you can run the request.
            c.BaseUrl = new Uri("https://localhost:5001/products/");
            var request = new RestRequest(customer.ProductId.ToString(), Method.GET);
            var response = c.Execute<Product>(request);
            var orderedProduct = response.Data;

            if (customer.Quantity <= orderedProduct.ItemsInStock - orderedProduct.ItemsReserved)
            {
                // reduce the number of items in stock for the ordered product,
                // and create a new order.
                orderedProduct.ItemsReserved += customer.Quantity;
                var updateRequest = new RestRequest(orderedProduct.Id.ToString(), Method.PUT);
                updateRequest.AddJsonBody(orderedProduct);
                var updateResponse = c.Execute(updateRequest);

                if (updateResponse.IsSuccessful)
                {
                    var newOrder = repository.Add(customer);
                    return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);
                }
            }

            // If the order could not be created, "return no content".
            return NoContent();
        }
    }
}
