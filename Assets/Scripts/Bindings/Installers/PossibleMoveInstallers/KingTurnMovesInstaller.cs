using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.PossibleMoveInstallers
{
    public class KingTurnMovesInstaller : Installer<KingTurnMovesInstaller>

    {
        public override void InstallBindings()
        {
            Container.Bind<PossibleKingMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, KingTurnMoves, KingTurnMoves.Factory>().FromNew();
        }
    }
}