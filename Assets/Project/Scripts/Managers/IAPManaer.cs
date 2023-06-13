using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManaer : MonoBehaviour
{
    public string buttonName= "com.e***";
    [SerializeField] DataScripts data;
    public void OnPurchaseComplate(Product product)
    {
        if(product.definition.id == buttonName)
        {
            data.adsOn = true;
        }
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason purchaseFailureReason)
    {
        Debug.Log("Urun Adi" + product + purchaseFailureReason + "Sebebinden satýn alým gerceklesmedi");
    }

}
