using CustomerApi.Data;
using CustomerApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CustomerApi.Controllers
{
    [ApiController]
    [Route("Customers")]
    public class CustomersController : ControllerBase
    {
        private readonly IRepository<Customer> repository;

        public CustomersController(IRepository<Customer> repos)
        {
            repository = repos;
        }

        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return repository.GetAll();
        }

        // GET Customer/1
        [Route("GetCustomer/{id}")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            var newCustomer = repository.Add(customer);
            return CreatedAtRoute("GetCustomer", new { id = newCustomer.ID }, newCustomer);
        }

        [HttpPut]
        public IActionResult Edit([FromBody]Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            repository.Edit(customer);
            return StatusCode(204);
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            repository.Remove(id);
            return StatusCode(204);
        }
    }
}
