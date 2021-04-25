using Models.Services.Interfaces;
using Models.Services.Moves.PieceMovers;
using Zenject;

public class MoveValidatorInstaller : Installer<MoveValidatorInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IMoveValidator>().To<MoveValidator>().AsSingle();
    }
}