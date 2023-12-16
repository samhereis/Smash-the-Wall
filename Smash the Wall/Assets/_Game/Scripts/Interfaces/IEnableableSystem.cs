public interface IEnableableSystem
{
    public bool isActive { get; }

    public void Enable();
    public void Disable();
}