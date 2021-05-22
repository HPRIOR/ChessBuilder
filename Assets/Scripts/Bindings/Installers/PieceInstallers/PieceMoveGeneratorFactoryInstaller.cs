using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class PieceMoveGeneratorFactoryInstaller : Installer<PieceMoveGeneratorFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IPieceMoveGeneratorFactory>().To<PieceMoveGeneratorFactory>().AsSingle();
        }
    }
}