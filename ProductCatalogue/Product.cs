class Product : PurchasableItem
{
    public int Quantity { get; set; } 

    public Product(string name, int price, string description, int quantity) : base(name, price, description)
    {
        Quantity = quantity;
    }
}