namespace GameState
{
    public abstract class GameStateBase
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
    }
}