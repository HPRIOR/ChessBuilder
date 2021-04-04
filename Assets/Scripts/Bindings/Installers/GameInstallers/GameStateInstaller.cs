using Zenject;

public class GameStateInstaller : Installer<GameStateInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind(typeof(ITurnEventInvoker), typeof(IGameState)).To<GameState>().AsSingle();
    }
}