using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForOrders : MonoBehaviour
{
    public GameObject orderList;
    public GameObject orderPrefab;
    public Order order;
    // Start is called before the first frame update
    public void CheckOrders()
    {
        order = (Order)ScriptableObject.CreateInstance(typeof(Order));
        order = GetComponent<SFTPScript>().RefreshRemoteOrders();
        GameObject goOrder = (GameObject)Instantiate(orderPrefab, orderList.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
