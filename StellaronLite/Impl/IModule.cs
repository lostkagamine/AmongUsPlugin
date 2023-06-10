namespace StellaronLite.Impl;

public interface IModule
{
    public void OnEnable();
    public void OnDisable();

    public void DrawUi();
}
