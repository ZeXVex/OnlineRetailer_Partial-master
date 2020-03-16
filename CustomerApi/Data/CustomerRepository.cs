using CustomerApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Data
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly CustomerApiContext Db;

        public CustomerRepository(CustomerApiContext context)
        {
            Db = context;
        }
        Customer IRepository<Customer>.Add(Customer entity)
        {
            var newCustomer = Db.Customers.Add(entity).Entity;
            Db.SaveChanges();
            return newCustomer;
        }

        void IRepository<Customer>.Edit(Customer entity)
        {
            Db.Entry(entity).State = EntityState.Modified;
            Db.SaveChanges();
        }

        Customer IRepository<Customer>.Get(int id)
        {
            return Db.Customers.FirstOrDefault(o => o.ID == id);
        }

        IEnumerable<Customer> IRepository<Customer>.GetAll()
        {
            return Db.Customers.ToList();
        }

        void IRepository<Customer>.Remove(int id)
        {
            var order = Db.Customers.FirstOrDefault(p => p.Id == id);
            Db.Customers.Remove(order);
            Db.SaveChanges();
        }
    }
}
