using System;
using System.Linq;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Managers
{
    public class InAppPurchacesManager : MonoBehaviour, IDetailedStoreListener
    {
        public static InAppPurchacesManager instance { get; private set; }
        [field: SerializeField] public bool isInitialized { get; private set; }

        public Action onInitialize;
        public Action<Item> onPurchase; 
        public Action<Item> onPurchaseFailed;
        public Action onRestore;

        [SerializeField] private bool _autoInitialize = false;
        [SerializeField] private bool _subscriptionsInEditor;
        [field: SerializeField] public Item[] items { get; private set; }

        private static IStoreController _storeController;
        private static IExtensionProvider _storeExtensionProvider;
        private static SubscriptionManager _subscriptionManager;

        private void Awake()
        {
            instance = this;
            if (_autoInitialize) Initialize();
        }

        public async void Initialize()
        {
            try
            {
                var options = new InitializationOptions().SetEnvironmentName("production");

                await UnityServices.InitializeAsync(options);

                if (_storeController == null)
                {
                    if (isInitialized)
                    {
                        return;
                    }

                    var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

                    foreach (Item item in items) builder.AddProduct(item.id, item.type);

                    UnityPurchasing.Initialize(this, builder);
                }
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Failed to initialize Unity services!\n\n{exception}");
            }
        }

        public Item GetItem(string id)
        {
            return items.FirstOrDefault(x => x.id.Equals(id));
        }

        private Item GetItemWithProduct(Product product)
        {
            return items.FirstOrDefault(x => x.product.Equals(product));
        }

        public bool IsPurchased(string storeId)
        {
            return IsPurchased(GetItem(storeId));
        }

        public bool IsPurchased(Item item)
        {
            if (isInitialized)
            {
                if (item.product != null && item.product.hasReceipt) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }

        public bool IsSubscribed(string storeId)
        {
            return IsSubscribed(GetItem(storeId));
        }

        public bool IsSubscribed(Item item)
        {
            if (item.type != ProductType.Subscription)
            {
                Debug.LogWarning($"{item.id} type is not subscription!");
                return false;
            }

            if (item.product.receipt == null)
            {
                return false;
            }

            var subscriptionManager = new SubscriptionManager(item.product, null);

#if UNITY_EDITOR
            return _subscriptionsInEditor;
#else
        var info = subscriptionManager.getSubscriptionInfo();
        return info.isSubscribed() == Result.True;
#endif
        }

        public void Purchase(Item item)
        {
            if (isInitialized)
            {
                if (item.product != null && item.product.availableToPurchase)
                {
                    Debug.Log($"IAP Purchasing {item.product.definition.id} ...");
                    _storeController.InitiatePurchase(item.product);

                    int amount = Mathf.FloorToInt((float)item.product.metadata.localizedPrice * 100);

                    Debug.Log($"IAP Price: {item.product.metadata.localizedPrice} amount: {amount}");
                }
                else
                {
                    Debug.LogWarning($"IAP {item.type} {item.id} not found!");
                }
            }
            else
            {
                Debug.LogWarning($"IAP not initialized!");
            }
        }

        public void Restore()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
            {
                _storeExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions((result, message) =>
                {
                    Debug.Log($"IAP Purchases restored {result} - {message}");
                    onRestore?.Invoke();
                });
            }
            else
            {
                Debug.Log($"IAP Not supported on {Application.platform}");
                onRestore?.Invoke();
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _storeExtensionProvider = extensions;

            foreach (var item in items) item.product = _storeController.products.WithID(item.id);

            isInitialized = true;
            onInitialize?.Invoke();
        }

        public void OnPurchaseComplete(Product product)
        {
            Debug.Log(product.metadata);
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log($"IAP Purchases Failed {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.Log($"IAP Purchases Failed {error} - {message}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            var item = GetItemWithProduct(product);
            Debug.Log($"IAP Failed to purchase {item.type} {item.id}! storeSpecificIdt: {product.definition.storeSpecificId} reason: {reason}");

            onPurchaseFailed?.Invoke(item);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            var item = GetItemWithProduct(product);
            Debug.Log($"IAP Failed to purchase {item.type} {item.id}! storeSpecificIdt: {product.definition.storeSpecificId} reason: {failureDescription.message}");

            onPurchaseFailed?.Invoke(item);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            onPurchase?.Invoke(GetItemWithProduct(args.purchasedProduct));

            return PurchaseProcessingResult.Complete;
        }

        [Serializable]
        public class Item
        {
            public string id;
            public ProductType type;
            public Product product;

            public string GetPrice()
            {
                if (product != null && product.metadata != null && !string.IsNullOrEmpty(product.metadata.isoCurrencyCode))
                {
                    return $"{product.metadata.localizedPrice} {product.metadata.isoCurrencyCode}";
                }
                else
                {
                    Debug.Log($"IAP Cannot get price for {id}!");
                    return "";
                }
            }

            public float GetPriceAmount()
            {
                if (product != null && product.metadata != null && !string.IsNullOrEmpty(product.metadata.isoCurrencyCode))
                {
                    return (float)product.metadata.localizedPrice;
                }
                else
                {
                    Debug.Log($"IAP Cannot get price for {id}!");
                    return 0;
                }
            }

            public string GetPriceCurrency()
            {
                if (product != null && product.metadata != null && !string.IsNullOrEmpty(product.metadata.isoCurrencyCode))
                {
                    return product.metadata.isoCurrencyCode;
                }
                else
                {
                    Debug.Log($"IAP Cannot get price for {id}!");
                    return "";
                }
            }
        }

        [Serializable]
        public struct Receipt
        {
            public string store;
            public string transactionID;
            public string payload;
        }

        [Serializable]
        public struct PayloadAndroid
        {
            public string json;
            public string signature;
        }
    }

}