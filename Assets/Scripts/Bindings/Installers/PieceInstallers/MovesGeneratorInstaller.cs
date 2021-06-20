using Models.Services.Interfaces;
using Models.Services.Moves.MoveGenerators;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class MovesGeneratorInstaller : Installer<MovesGeneratorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IMovesGenerator>().To<MovesGenerator>().AsSingle();
        }
    }
}