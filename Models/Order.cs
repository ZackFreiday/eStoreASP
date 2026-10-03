using System.ComponentModel.DataAnnotations;

namespace eStoreASP.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
