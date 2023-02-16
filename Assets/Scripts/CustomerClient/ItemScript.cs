using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{

    public FoodItem food;
    public bool noLongerValid;
    public ShoppingScript paymentManager;
    // Start is called before the first frame update
    void Start()
    {
        paymentManager = GameObject.FindObjectOfType<ShoppingScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RemoveItemFromList()
    {
        noLongerValid = true;
        paymentManager.RefreshGroceryList();
        Destroy(gameObject);
    }
}
