﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.Providers
{
    public abstract class AdProvider : MonoBehaviour
    {
        [field: SerializeField] public bool IsTest { get; set; }
        [field: SerializeField] public bool Consent { get; set; }
        [field: SerializeField] public string ApiKey { get; private set; }
        [field: SerializeField] public bool IsInitialized { get; set; }
        [field: SerializeField] public string AppOpenAdUnitId { get; private set; }
        [field: SerializeField] public string BannerAdUnitId { get; private set; }
        [field: SerializeField] public string InterstitialAdUnitId { get; private set; }
        [field: SerializeField] public string RewardedAdUnitId { get; private set; }
        [field: SerializeField] public string RewardedInterstitialAdUnitId { get; private set; }
        [field: SerializeField] public string MRecAdUnitId { get; private set; }

        public Action OnInitialize;
        public Action<AdType, string, string> OnError;
        public Action<AdType, string> OnLoad;
        public Action<AdType, string> OnShow;
        public Action<AdType, string> OnEarnReward;
        public Action<AdType, string> OnClose;
        public Action<AdType, string> OnClick;
        public Action<AdType, string> OnImpression;
        public Action<AdType, string, Revenue> OnPaid;

        public List<AdType> InitializedAdTypes { get; private set; }
        public List<AdType> RequestingAdTypes { get; private set; }

        public virtual void Initialize(bool isTest, bool consent, string apiKey, string appOpen, string banner, string interstitial, string rewarded, string rewardedInterstitial, string mrec)
        {
            IsTest = isTest;
            Consent = consent;
            ApiKey = apiKey;
            AppOpenAdUnitId = appOpen;
            BannerAdUnitId = banner;
            InterstitialAdUnitId = interstitial;
            RewardedAdUnitId = rewarded;
            RewardedInterstitialAdUnitId = rewardedInterstitial;
            MRecAdUnitId = mrec;

            InitializedAdTypes = new List<AdType>();
            RequestingAdTypes = new List<AdType>();

            OnError += (type, id, error) => SetAdLoading(type, false);
            OnLoad += (type, id) => SetAdLoading(type, false);
            OnShow += (type, id) => SetAdLoading(type, false);
            OnClose += (type, id) => SetAdLoading(type, false);

            if (!string.IsNullOrEmpty(AppOpenAdUnitId))
                InitializedAdTypes.Add(AdType.AppOpen);

            if (!string.IsNullOrEmpty(BannerAdUnitId))
                InitializedAdTypes.Add(AdType.Banner);

            if (!string.IsNullOrEmpty(InterstitialAdUnitId))
                InitializedAdTypes.Add(AdType.Interstitial);

            if (!string.IsNullOrEmpty(RewardedAdUnitId))
                InitializedAdTypes.Add(AdType.Rewarded);

            if (!string.IsNullOrEmpty(RewardedInterstitialAdUnitId))
                InitializedAdTypes.Add(AdType.RewardedInterstitial);

            if (!string.IsNullOrEmpty(MRecAdUnitId))
                InitializedAdTypes.Add(AdType.MRec);
        }

        private void SetAdLoading(AdType type, bool isLoading)
        {
            if (isLoading)
            {
                if (!RequestingAdTypes.Contains(type))
                    RequestingAdTypes.Add(type);
            }
            else
            {
                RequestingAdTypes.Remove(type);
            }
        }

        public virtual bool IsLoading(AdType type)
        {
            return RequestingAdTypes.Contains(type);
        }

        public virtual bool IsReady(AdType type)
        {
            return false;
        }

        public virtual void Request(AdType type)
        {
            SetAdLoading(type, true);
        }

        public virtual void Show(AdType type)
        {

        }

        public virtual void Hide(AdType type)
        {

        }

        public virtual void Destroy(AdType type)
        {

        }

        public virtual void OnPause(bool pause)
        {

        }
    }
}
