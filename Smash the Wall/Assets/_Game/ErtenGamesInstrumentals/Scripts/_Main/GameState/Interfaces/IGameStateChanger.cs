namespace GameState
{
    public interface IGameStateChanger
    {
        public void ChangeState(GameStateBase gameState);
    }
}