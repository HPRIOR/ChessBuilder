using Bindings.Installers.BoardInstallers;
using Bindings.Installers.InputInstallers;
using Bindings.Installers.PieceInstallers;
using Bindings.Installers.PossibleMoveInstallers;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PieceMoves.Utils
{
    public static class PossibleMovesBinder
    {
        public static void InstallBindings(DiContainer Container)
        {
            BoardGeneratorInstaller.Install(Container);
            PieceSpawnerInstaller.Install(Container);
            PossibleMoveFactoryInstaller.Install(Container);
            CommandInvokerInstaller.Install(Container);
            MoveCommandInstaller.Install(Container);
            PossibleBishopMovesInstaller.Install(Container);
            PossibleKingMovesInstaller.Install(Container);
            PossibleKnightMovesInstaller.Install(Container);
            PossiblePawnMovesInstaller.Install(Container);
            PossibleQueenMovesInstaller.Install(Container);
            PossibleRookMovesInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            AllPossibleMovesGeneratorInstaller.Install(Container);
            BoardEvalInstaller.Install(Container);
            NonTurnRemoverInstaller.Install(Container);
        }
    }
}