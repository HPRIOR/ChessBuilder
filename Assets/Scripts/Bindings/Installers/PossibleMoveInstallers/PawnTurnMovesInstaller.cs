using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.PossibleMoveInstallers
{
    public class PawnTurnMovesInstaller : Installer<PawnTurnMovesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PossiblePawnMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, PawnTurnMoves, PawnTurnMoves.Factory>().FromNew();
        }
    }
}