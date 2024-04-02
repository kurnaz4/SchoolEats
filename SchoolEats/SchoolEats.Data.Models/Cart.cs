namespace SchoolEats.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class Cart
    {
        public Cart()
        {
            this.Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Dish))]
        public Guid DishId { get; set; }

        public Dish Dish { get; set; }

        public Guid BuyerId { get; set; }

        public int Quantity { get; set; }
    }
}
