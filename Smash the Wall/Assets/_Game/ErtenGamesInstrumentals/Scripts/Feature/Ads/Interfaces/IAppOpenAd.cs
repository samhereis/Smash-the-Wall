using System;

namespace ErtenGamesInstrumentals.Ads
{
    public interface IAppOpenAd
    {
        public void TryShowAppOpenAd(string adID, Action onShowed, Action onFail);
        public void Request();
    }
}