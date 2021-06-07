using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.MoveInstallers
{
    public class QueenTurnMovesInstaller : Installer<QueenTurnMovesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PossibleQueenMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, QueenTurnMoves, QueenTurnMoves.Factory>().FromNew();
        }
    }
}