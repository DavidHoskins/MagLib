using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    public GameObject productPrefab; // Prefab for the product UI element
    public GameObject bundlePrefab; // Prefab for the bundle UI element
    public Transform productListParent; // Parent transform for the product list UI elements

    // Start is called before the first frame update
    void Start()
    {
        ProductCatalogue productCatalogue = new ProductCatalogue("Assets/Resources/test_json.json");

        Vector2 uiPosition = new Vector2(-GetCanvasWidth() / 2, GetCanvasHeight() / 2);

        // Adjust the initial position to start from the top left corner of the canvas
        uiPosition.y -= GetUIElementHeight(productPrefab) + 10;
        uiPosition.x += GetUIElementWidth(productPrefab) + 10; 

        foreach (var item in productCatalogue.SortBy(ProductCatalogue.SortByType.Name, ascending: false))
        {
            GenerateProductUI(item, uiPosition);
            uiPosition.x += GetUIElementWidth(productPrefab) + 10; // Adjust the x position for the next UI element
            if (uiPosition.x + GetUIElementWidth(productPrefab) > GetCanvasWidth() / 2)
            {
                uiPosition.x = -GetCanvasWidth() / 2; // Reset x position
                uiPosition.y -= GetUIElementHeight(productPrefab) + 10; // Move down to the next row
            }

        }
    }

    float GetCanvasWidth()
    {
        RectTransform rectTransform = productListParent.GetComponent<RectTransform>();
        return rectTransform.rect.width;
    }

    float GetCanvasHeight()
    {
        RectTransform rectTransform = productListParent.GetComponent<RectTransform>();
        return rectTransform.rect.height;
    }

    float GetUIElementWidth(GameObject uiElement)
    {
        RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
        return rectTransform.rect.width;
    }

    float GetUIElementHeight(GameObject uiElement)
    {
        RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
        return rectTransform.rect.height;
    }

    void GenerateProductUI(PurchasableItem item, Vector2 uiPosition)
    {
        GameObject uiElement = null;
        if (item is Product product)
        {
            uiElement = Instantiate(productPrefab, productListParent);
            uiElement.transform.localPosition = uiPosition;
            uiElement.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = product.Name;
            uiElement.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = product.Price.ToString();
            uiElement.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = product.Description;
            uiElement.transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = product.Quantity.ToString();
        }
        else if (item is Bundle bundle)
        {
            uiElement = Instantiate(bundlePrefab, productListParent);
            uiElement.transform.localPosition = uiPosition;
            uiElement.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = bundle.Name;
            uiElement.transform.Find("Price").GetComponent<TextMeshProUGUI>().text = bundle.Price.ToString();
            uiElement.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = bundle.Description;

            TextMeshProUGUI gemText = uiElement.transform.Find("Gems").GetComponent<TextMeshProUGUI>();
            if (bundle.Products.Find(p => p.Name == "Gems") != null)
                gemText.text = $"{bundle.Products[0].Name} : {bundle.Products[0].Quantity}";
            else
                gemText.enabled = false;
            
            TextMeshProUGUI coinText = uiElement.transform.Find("Coins").GetComponent<TextMeshProUGUI>();
            if (bundle.Products.Find(p => p.Name == "Coins") != null)
                coinText.text = $"{bundle.Products[1].Name} : {bundle.Products[1].Quantity}";
            else
                coinText.enabled = false;

            TextMeshProUGUI ticketText = uiElement.transform.Find("Tickets").GetComponent<TextMeshProUGUI>();
            if (bundle.Products.Find(p => p.Name == "Tickets") != null)
                ticketText.text = $"{bundle.Products[2].Name} : {bundle.Products[2].Quantity}";
            else
                ticketText.enabled = false;
        }
    }
}
