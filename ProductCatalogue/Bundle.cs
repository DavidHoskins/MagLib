using System.Collections.Generic;
class Bundle : PurchasableItem
{
    public List<Product> Products { get; set; } // List of products in the bundle

    public Bundle(string name, int price, string description, List<Product> products) : base(name, price, description)
    {
        Products = products;
    }
}