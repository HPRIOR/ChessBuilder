using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.MoveInstallers
{
    public class PossibleMovesFactoryInstaller : Installer<PossibleMovesFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PossibleMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, QueenTurnMoves, QueenTurnMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, RookTurnMoves, RookTurnMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, PawnTurnMoves, PawnTurnMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, BishopTurnMoves, BishopTurnMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, KingTurnMoves, KingTurnMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, KnightTurnMoves, KnightTurnMoves.Factory>().FromNew();
        }
    }
}