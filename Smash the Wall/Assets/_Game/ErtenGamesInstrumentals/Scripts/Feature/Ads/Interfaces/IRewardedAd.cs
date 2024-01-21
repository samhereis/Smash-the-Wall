using System;

namespace ErtenGamesInstrumentals.Ads
{
    public interface IRewardedAd
    {
        public void TryShowRewardedAd(string adID, Action onRewarded, Action onFail);
        public void Request();
    }
}