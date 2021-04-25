using Models.Services.Interfaces;
using Models.Services.Moves.PieceMovers;
using Zenject;

public class PieceMoverInstaller : Installer<PieceMoverInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IPieceMover>().To<PieceMover>().AsSingle();
    }
}