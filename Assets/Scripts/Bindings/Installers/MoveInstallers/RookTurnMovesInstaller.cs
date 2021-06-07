using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.MoveInstallers
{
    public class RookTurnMovesInstaller : Installer<RookTurnMovesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PossibleRookMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, RookTurnMoves, RookTurnMoves.Factory>().FromNew();
        }
    }
}