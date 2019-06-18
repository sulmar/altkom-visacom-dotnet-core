using System;
using System.Collections.Generic;

namespace production.models
{

    public interface IOrderRepository 
    {
        IEnumerable<Order> Get();
        Order Get(int id);
        void Add(Order order);
        void Update(Order order);
        void Remove(int id);
    }

    public abstract class Base
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class Order : Base
    {
        public string Number { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
      
        public DateTime DueDate { get; set; }
        public bool IsDeleted { get; set; }
        public OrderStatus Status { get; set; }

        public enum OrderStatus
        {
            Added,
            Planning,
            Confirmed,
            Done,
            Canceled
        }

    }

  
}
