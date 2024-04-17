namespace SchoolEats.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    //клас на количка 
    public class Cart
    {
        public Cart()
        {
            this.Id = Guid.NewGuid();
        }
        [Key]
        //Guid- за защита от кражба на данни
        public Guid Id { get; set; }

        [ForeignKey(nameof(Dish))]
        public Guid DishId { get; set; }

        public Dish Dish { get; set; }

        public Guid BuyerId { get; set; }

        public int Quantity { get; set; }
    }
}
