using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.PossibleMoveInstallers
{
    public class PossibleKingMovesInstaller : Installer<PossibleKingMovesInstaller>

    {
        public override void InstallBindings()
        {
            Container.Bind<PossibleKingMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, PossibleKingMoves, PossibleKingMoves.Factory>().FromNew();
        }
    }
}