using Bindings.Installers.BoardInstallers;
using Bindings.Installers.MoveInstallers;
using Bindings.Installers.PieceInstallers;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PieceMoves.Utils
{
    // TODO: refactor so each test installs only what it needs, and mocks where necessary 
    public static class PossibleMovesBinder
    {
        public static void InstallBindings(DiContainer Container)
        {
            BoardGeneratorInstaller.Install(Container);
            PieceSpawnerInstaller.Install(Container);
            PossibleMoveFactoryInstaller.Install(Container);
            BishopTurnMovesInstaller.Install(Container);
            KingTurnMovesInstaller.Install(Container);
            KnightTurnMovesInstaller.Install(Container);
            PawnTurnMovesInstaller.Install(Container);
            QueenTurnMovesInstaller.Install(Container);
            RookTurnMovesInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            AllPossibleMovesGeneratorInstaller.Install(Container);
            BoardEvalInstaller.Install(Container);
            NonTurnRemoverInstaller.Install(Container);
        }
    }
}