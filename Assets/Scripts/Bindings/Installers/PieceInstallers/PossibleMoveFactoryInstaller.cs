using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class PossibleMoveFactoryInstaller : Installer<PossibleMoveFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IPossibleMoveFactory>().To<PossibleMoveFactory>().AsSingle();
        }
    }
}