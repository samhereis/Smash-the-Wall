using System;
using System.Drawing;
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
        public static InAppPurchacesManager Instance { get; private set; }
        [field: SerializeField] public bool IsInitialized { get; private set; }

        public Action OnInitialize;
        public Action<Item> onPurchaseCompleted;
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
            Instance = this;
            if (_autoInitialize) Initialize();
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
            if (IsInitialized)
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

        public async void Initialize()
        {
            try
            {
                var options = new InitializationOptions().SetEnvironmentName("production");
                await UnityServices.InitializeAsync(options);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Unity Services InitializeAsync exception: {exception}");
            }


            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (Item item in items) builder.AddProduct(item.id, item.type);

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _storeExtensionProvider = extensions;

            foreach (var item in items) item.product = _storeController.products.WithID(item.id);

            IsInitialized = true;
            OnInitialize?.Invoke();
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

        public void Purchase(string itemID)
        {
            Purchase(GetItem(itemID));
        }

        public void Purchase(Item item)
        {
            if (IsInitialized)
            {
                if (item.product != null && item.product.availableToPurchase)
                {
                    Debug.Log($"IAP Purchasing {item.product.definition.id} ...");
                    _storeController.InitiatePurchase(item.product);
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

        public void OnPurchaseComplete(Product product)
        {
            var amount = product.metadata.localizedPrice;
            Debug.Log($"Bought {product} - amount: {amount}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            var item = GetItemWithProduct(args.purchasedProduct);
            var amount = item.product.metadata.localizedPrice;
            Debug.Log($"Bought {item.id} - amount: {amount}");

            onPurchaseCompleted?.Invoke(item);

            return PurchaseProcessingResult.Complete;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log($"IAP Purchases restored {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.Log($"IAP Purchases restored {error} - {message}");
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

        [Serializable]
        public class Item
        {
            public string id;
            public UnityEngine.Purchasing.ProductType type;
            public UnityEngine.Purchasing.Product product;

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
            public string Store;
            public string TransactionID;
            public string Payload;
        }

        [Serializable]
        public struct PayloadAndroid
        {
            public string Json;
            public string Signature;
        }
    }

}