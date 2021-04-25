using Models.Services.Board;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.Interfaces;
using Zenject;

public class BoardStateInstaller : Installer<BoardStateInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IBoardGenerator>().To<BoardGenerator>().AsSingle();
        Container.Bind<IBoardState>().To<BoardState>().AsSingle();
    }
}