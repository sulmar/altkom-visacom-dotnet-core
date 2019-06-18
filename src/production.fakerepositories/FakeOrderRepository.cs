using System;
using production.models;
using System.Collections.Generic;
using System.Linq;

namespace production.fakerepositories
{
    public class FakeOrderRepository : IOrderRepository
    {
        private readonly ICollection<Order> orders;
        private readonly OrderFaker orderFaker;

        public FakeOrderRepository(OrderFaker orderFaker)
        {
            orders = orderFaker.Generate(1000);
        }

        public void Add(Order order)
        {
            orders.Add(order);
        }

        public IEnumerable<Order> Get()
        {
            return orders;
        }

        public Order Get(int id)
        {
           return orders.SingleOrDefault(p=>p.Id == id);
        }

        public void Remove(int id)
        {
            orders.Remove(Get(id));
        }

        public void Update(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
