using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Move;
using Bindings.Installers.ViewInstallers;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PieceMoves.Utils
{
    public static class PossibleMovesBinder
    {
        public static void InstallBindings(DiContainer Container)
        {
            BoardGeneratorInstaller.Install(Container);
            PieceFactoryInstaller.Install(Container);
            MovesGeneratorRepositoryInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            MovesGeneratorInstaller.Install(Container);
            PossibleMovesFactoryInstaller.Install(Container);
            BoardInfoInstaller.Install(Container);
            KingMoveFilterInstaller.Install(Container);
            PinnedPieceFilterInstaller.Install(Container);
        }
    }
}