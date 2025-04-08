using System;
using System.Collections;
using System.Collections.Generic;
using Gley.EasyIAP;
using UnityEngine;

public class IAPButton : MonoBehaviour
{
    public ShopProductNames ShopProductName;
    public void Buy() => MyIAPManager.I.BuyItem(ShopProductName, OnSuccess);


    private void OnSuccess(StoreProduct product)
    {
        print($"sukses beli {product.productName} {product.rewardType}");
        if (product.rewardType == RewardType.Coin)
        {
            MyIAPManager.I.coin += product.value;
        }
    }
}
