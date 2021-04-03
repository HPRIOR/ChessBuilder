using Zenject;

public class GameStateInstaller : Installer<GameStateInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IGameState>().To<GameState>().AsSingle();
    }
}