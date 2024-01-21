namespace ErtenGamesInstrumentals.Ads
{
    public interface IBannerAd
    {
        public void TryShowBanner(string adID);
        public void Destroy();
        public void Request();
    }
}