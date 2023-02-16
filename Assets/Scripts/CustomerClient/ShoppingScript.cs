using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using TMPro;
using YamlDotNet.RepresentationModel;
public class ShoppingScript : MonoBehaviour
{

    public GameObject shoppingList, shoppingListObjectPrefab, payButton, nameInputField, mainMenu, settingsMenu;
    public double totalCost;
    string order, customerName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddItemToCart(FoodItem foodItem)
    {
        GameObject cartItem = Instantiate(shoppingListObjectPrefab, shoppingList.transform);
        cartItem.GetComponent<ItemScript>().food = foodItem;
        cartItem.transform.Find("Name").GetComponent<TMP_Text>().SetText(foodItem.foodName);
        cartItem.transform.Find("Price").GetComponent<TMP_Text>().SetText(foodItem.foodPrice.ToString() + "€");
        RefreshGroceryList();
    }
    public void RefreshGroceryList()
    {
        totalCost = 0;
        foreach(Transform i in shoppingList.transform)
        {
            if (i.GetComponent<ItemScript>() && !i.GetComponent<ItemScript>().noLongerValid) totalCost += i.GetComponent<ItemScript>().food.foodPrice;
            totalCost = System.Math.Ceiling(totalCost * 100) / 100;
        }
        payButton.GetComponentInChildren<TMP_Text>().SetText(totalCost.ToString() + "€ - Maksa");
    }
    public void GetUsername()
    {
        customerName = nameInputField.GetComponent<TMP_InputField>().text;
    }
    public void WriteOrderToDisk()
    {
        
        //get current time
        DateTime now = DateTime.Now;
        foreach (Transform i in shoppingList.transform)
        {
            if (i.GetComponent<ItemScript>()) order += i.GetComponent<ItemScript>().food.foodName + "\n";
        }
        var stream = new YamlStream(
            new YamlDocument(
                new YamlMappingNode(
                    new YamlScalarNode("customer"), new YamlScalarNode(customerName),
                    new YamlScalarNode("timedate"), new YamlScalarNode(now.ToString()),
                    new YamlScalarNode("totalcost"), new YamlScalarNode(totalCost.ToString() + "€"),
                    new YamlScalarNode("order"), new YamlScalarNode(order) { Style = YamlDotNet.Core.ScalarStyle.Literal }
                )
            )
        );

        UnitySystemConsoleRedirector.Redirect();
        try
        {
            using (TextWriter writer = File.CreateText(GetComponent<SFTPScript>().localFile))
                stream.Save(writer, false);
        }
        catch(DirectoryNotFoundException e)
        {
            int lastBackslashIndex = GetComponent<SFTPScript>().localFile.LastIndexOf('\\');
            string output = GetComponent<SFTPScript>().localFile.Substring(0, lastBackslashIndex);
            DirectoryInfo di = Directory.CreateDirectory(output);
            using (TextWriter writer = File.CreateText(GetComponent<SFTPScript>().localFile))
                stream.Save(writer, false);
                
            Debug.Log("error: " + e.ToString());
        }
        GetComponent<SFTPScript>().WriteToSFTP();
        payButton.GetComponentInChildren<TMP_Text>().SetText(0 + "€ - Maksa");
        foreach (Transform i in shoppingList.transform)
        {
            Destroy(i.gameObject);
        }
    }
    public void OpenSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
        
    public void CloseSettingsMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

}
