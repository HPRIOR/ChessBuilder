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
            Container.BindFactory<PieceColour, bool, QueenMoves, QueenMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, bool, RookTurnMoves, RookTurnMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, bool, PawnMoves, PawnMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, bool, BishopMoves, BishopMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, bool, KingMoves, KingMoves.Factory>().FromNew();
            Container.BindFactory<PieceColour, bool, KnightMoves, KnightMoves.Factory>().FromNew();
        }
    }
}