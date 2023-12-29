namespace GameState
{
    public abstract class GameStateViewBase<TSceneManager> where TSceneManager : GameState_SceneManagerBase
    {
        protected TSceneManager _sceneManager;

        public GameStateViewBase(TSceneManager sceneManager)
        {
            _sceneManager = sceneManager;
        }
    }
}