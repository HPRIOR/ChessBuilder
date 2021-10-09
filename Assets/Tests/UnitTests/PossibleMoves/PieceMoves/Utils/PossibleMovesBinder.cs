using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Move;
using Bindings.Installers.ViewInstallers;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PieceMoves.Utils
{
    public static class PossibleMovesBinder
    {
        public static void InstallBindings(DiContainer container)
        {
            BoardGeneratorInstaller.Install(container);
            PieceFactoryInstaller.Install(container);
            MovesGeneratorRepositoryInstaller.Install(container);
            TileEvaluatorInstaller.Install(container);
            BoardScannerInstaller.Install(container);
            PositionTranslatorInstaller.Install(container);
            MovesGeneratorInstaller.Install(container);
            PossibleMovesFactoryInstaller.Install(container);
            BoardInfoInstaller.Install(container);
            KingMoveFilterInstaller.Install(container);
            PinnedPieceFilterInstaller.Install(container);
            CheckedStateManagerInstaller.Install(container);
        }
    }
}