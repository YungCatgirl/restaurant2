using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMenuItem : MonoBehaviour
{
    public FoodItem food;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddItem()
    {
        GameObject.FindObjectOfType<ShoppingScript>().AddItemToCart(food);
    }
}
