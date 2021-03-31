namespace Clinic.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public Service Service { get; set; }
        public int Amount { get; set; }
        public string ShoppingCartId { get; set; }
    }
}