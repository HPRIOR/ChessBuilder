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
            Container.BindFactory<PieceColour, QueenMoves, QueenMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, RookTurnMoves, RookTurnMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, PawnMoves, PawnMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, BishopMoves, BishopMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, KingMoves, KingMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, KnightMoves, KnightMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, PawnNonTurnMoves, PawnNonTurnMoves.Factory>().FromNew();
        }
    }
}