class PurchasableItem
{
    public string Name { get; set; }
    public int Price { get; set; } // Price in USD in cents
    public string Description { get; set; }

    public PurchasableItem(string name, int price, string description)
    {
        Name = name;
        Price = price;
        Description = description;
    }
}