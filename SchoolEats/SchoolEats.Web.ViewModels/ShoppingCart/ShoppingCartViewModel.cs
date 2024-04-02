namespace SchoolEats.Web.ViewModels.ShoppingCart
{
    using Dish;

    public class ShoppingCartViewModel
    {
        public ICollection<AllDishesViewModel> Dishes { get; set; }
    }
}
