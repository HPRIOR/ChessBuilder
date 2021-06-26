using Models.Services.Interfaces;
using Models.Services.Moves.MoveGenerators;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class MovesGeneratorRepositoryInstaller : Installer<MovesGeneratorRepositoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IMovesGeneratorRepository>().To<MovesGeneratorRepository>().AsSingle();
        }
    }
}