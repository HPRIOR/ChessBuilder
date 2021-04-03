using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        GameStateInstaller.Install(Container);
    }
}