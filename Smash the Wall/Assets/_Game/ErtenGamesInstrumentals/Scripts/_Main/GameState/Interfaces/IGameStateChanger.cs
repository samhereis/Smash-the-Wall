namespace GameState
{
    public interface IGameStateChanger
    {
        public void ChangeState(GameState_ControllerBase gameState);
    }
}