namespace ErtenGamesInstrumentals.Ads
{
    public interface IInterstitialAd
    {
        public void TryShowInterstitial(string adID);
        public void Request();
    }
}