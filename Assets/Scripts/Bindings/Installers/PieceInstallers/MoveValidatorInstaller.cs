using Models.Services.Interfaces;
using Models.Services.Moves.PieceMovers;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class MoveValidatorInstaller : Installer<MoveValidatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IMoveValidator>().To<MoveValidator>().AsSingle();
        }
    }
}