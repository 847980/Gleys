#if GLEY_IAP_IOS || GLEY_IAP_GOOGLEPLAY || GLEY_IAP_AMAZON || GLEY_IAP_MACOS || GLEY_IAP_WINDOWS
#define GleyIAPEnabled
#endif

#if GleyIAPEnabled
using System;
using UnityEngine;
using UnityEngine.Purchasing;
#endif


namespace Gley.EasyIAP
{
    public enum RewardType
    {
        No,
        Coin,
        Level
    }

    
    [System.Serializable]
    public class StoreProduct
    {
        public string productName;
        public ProductType productType;
        public RewardType rewardType;
        public ProductSo productSo;
        public string idGooglePlay;
        public string idAmazon;
        public string idIOS;
        public string idMac;
        public string idWindows;
        public int value;
        public string localizedPriceString = "-";
        public double price;
        public string isoCurrencyCode;
        public string receipt;
        internal string localizedDescription;
        internal string localizedTitle;
        internal bool active;
        internal SubscriptionInfo subscriptionInfo;



        public StoreProduct(string productName, ProductType productType, int value, string idGooglePlay, string idIOS, string idAmazon, string idMac, string idWindows, RewardType rewardType, ProductSo productSo)
        {
            this.productName = productName;
            this.productType = productType;
            this.value = value;
            this.idGooglePlay = idGooglePlay;
            this.idIOS = idIOS;
            this.idAmazon = idAmazon;
            this.idMac = idMac;
            this.idWindows = idWindows;
            this.rewardType = rewardType;
            this.productSo = productSo;
        }


        public StoreProduct()
        {
            productName = "";
            idGooglePlay = "";
            idIOS = "";
            idAmazon = "";
            idMac = "";
            idWindows = "";
            productType = ProductType.Consumable;
            rewardType = RewardType.Coin;
        }

#if GleyIAPEnabled
        internal UnityEngine.Purchasing.ProductType GetProductType()
        {
            return (UnityEngine.Purchasing.ProductType)(int)productType;
        }
        internal RewardType GetRewardType()
        {
            return (RewardType)(int)rewardType;
        }
        internal ShopProductNames GetProductName()
        {
            return (ShopProductNames) Enum.Parse(typeof(ShopProductNames), productName);
        }
#endif

        internal string GetStoreID()
        {
#if GLEY_IAP_MACOS
            return idMac;
#elif GLEY_IAP_IOS
            return idIOS;
#elif GLEY_IAP_GOOGLEPLAY
            return idGooglePlay;
#elif GLEY_IAP_AMAZON
            return idAmazon;
#elif GLEY_IAP_WINDOWS
            return idWindows;
#else
            return "";
#endif
        }
    }

#if !GleyIAPEnabled
    public class SubscriptionInfo
    {
    }

    public enum Result
    {

    }
#endif
}
