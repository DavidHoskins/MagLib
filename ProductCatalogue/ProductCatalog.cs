using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

class ProductCatalogue
{
    private List<PurchasableItem> _items = new List<PurchasableItem>();
    public enum SortByType  {Price, Name, Description};
    public ProductCatalogue(string path)
    {
        StreamReader reader = new StreamReader(path);
        if (reader == null)
        {
            Debug.LogError("Failed to open JSON file.");
            return;
        }
        string jsonString = reader.ReadToEnd();
        reader.Close();
        FromJson(jsonString);
    }
    private void FromJson(string jsonString)
    {
        JsonProto jsonItemProto = JsonConvert.DeserializeObject<JsonProto>(jsonString);
        if(jsonItemProto != null)
        {
            if(jsonItemProto.Bundles != null)
            {
                _items.AddRange(jsonItemProto.Bundles);
            }
            if (jsonItemProto.Products != null)
            {
                _items.AddRange(jsonItemProto.Products);
            }
        }
    }

    public IEnumerable<PurchasableItem> SortBy(SortByType sortByType, bool ascending = true)
    {
        switch (sortByType)
        {
            case SortByType.Name:
                return ascending ? _items.OrderBy(item => item.Name) : _items.OrderBy(item => item.Name).Reverse();
            case SortByType.Price:
                return ascending ? _items.OrderBy(item => item.Price) : _items.OrderBy(item => item.Price).Reverse();
            case SortByType.Description:
                return ascending ? _items.OrderBy(item => item.Description) : _items.OrderBy(item => item.Description).Reverse();
            default:
                Debug.LogWarning("SortByType is undefined. Returning unsorted items.");
                return _items;
        }
    }

    public IEnumerable<PurchasableItem> SortBy(bool ascending = true, params string[] itemOrder)
    {
        if (itemOrder.Length == 0)
        {
            Debug.LogWarning("No item order provided. Returning unsorted items.");
            return _items;
        }

        IOrderedEnumerable<PurchasableItem> sortedItems = _items.OrderBy(item => {
            // Sort for bundles
            if(item is Bundle bundle)
            {
                foreach (Product product in bundle.Products)
                {
                    int index = itemOrder.ToList().IndexOf(product.Name);
                    if(index != -1)
                        return index;
                }
            }

            // Sort for products
            return itemOrder.ToList().IndexOf(item.Name);
        });

        return ascending ? sortedItems : sortedItems.Reverse();
    }

    public IEnumerable<PurchasableItem> FilterBy(params string[] itemOrder)
    {
        if (itemOrder.Length == 0)
        {
            Debug.LogWarning("No item order provided. Returning unsorted items.");
            return _items;
        }

        IEnumerable<PurchasableItem> filteredItems = _items.Where(item => {
            if(item is Bundle bundle)
            {
                foreach (Product product in bundle.Products)
                {
                    if (itemOrder.Contains(product.Name))
                        return true;
                }
            }
            return itemOrder.Contains(item.Name);
        });

        return filteredItems;
    }

    public IEnumerable<PurchasableItem> FilterBy(Func<PurchasableItem, bool> filterFunc)
    {
        return _items.Where(filterFunc);
    }

    public IEnumerable<PurchasableItem> CustomSortBy(Func<PurchasableItem, object> sortFunc, bool ascending = true)
    {
        IOrderedEnumerable<PurchasableItem> sortedItems = _items.OrderBy(sortFunc);
        return ascending ? sortedItems : sortedItems.Reverse();
    }
}