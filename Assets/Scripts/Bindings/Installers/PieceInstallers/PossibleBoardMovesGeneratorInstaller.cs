using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveGenerators;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class PossibleBoardMovesGeneratorInstaller : Installer<PossibleBoardMovesGeneratorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IPossibleMovesGenerator>().To<PossibleBoardMovesGenerator>().AsSingle();
        }
    }
}