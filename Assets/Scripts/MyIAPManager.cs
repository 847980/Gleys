using System;
using System.Collections;
using System.Collections.Generic;
using Gley.EasyIAP;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MyIAPManager : MonoBehaviour
{
    public static MyIAPManager I;
    public Button buySubscription;
    public Button buyCoin;
    public Button buyLevel;
    public TextMeshProUGUI isCanceledText;
    public TextMeshProUGUI coinText;
    private int _coin = 0;
    public int coin
    {
        get { return _coin; }
        set { _coin = value; coinText.SetText($"coin: {_coin}"); }
    }

    void Start()
    {
        I = this;
        API.Initialize(OnInitialized);
    }

    private void OnInitialized(IAPOperationStatus status, string message, List<StoreProduct> products)
    {
        if (status == IAPOperationStatus.Success)
        {
            RefreshUI();
        }
        else Debug.Log("failed init iap");
    }

    public void RefreshUI()
    {
        isCanceledText.text = $"subs is cancelled: {API.IsCancelled(ShopProductNames.subscription)}";
        buyCoin.GetComponentInChildren<TextMeshProUGUI>().SetText($"koin {API.GetLocalizedPriceString(ShopProductNames.gleykoin)}");
        buyLevel.GetComponentInChildren<TextMeshProUGUI>().SetText($"open level {API.GetLocalizedPriceString(ShopProductNames.openlevel2)}");
        buySubscription.GetComponentInChildren<TextMeshProUGUI>().SetText($"koin {API.GetLocalizedPriceString(ShopProductNames.subscription)}");
        if (API.IsActive(ShopProductNames.openlevel2)) buyLevel.interactable = false;
        if (API.IsActive(ShopProductNames.subscription)) buySubscription.interactable = false;
    }
    public UnityAction<StoreProduct> OnPurchasedCompleted;
    public void BuyItem(ShopProductNames item, UnityAction<StoreProduct> OnPurchasedCompleted)
    {
        this.OnPurchasedCompleted += OnPurchasedCompleted;
        API.BuyProduct(item, OnPurchased);
    }

    private void OnPurchased(IAPOperationStatus status, string message, StoreProduct product)
    {
        if (status == IAPOperationStatus.Success)
        {
            OnPurchasedCompleted?.Invoke(product);
            OnPurchasedCompleted = null;
            RefreshUI();
        }
        else print("fail purchased item");
    }
}
