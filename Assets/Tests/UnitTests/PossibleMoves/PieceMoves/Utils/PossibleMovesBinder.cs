using Bindings.Installers.BoardInstallers;
using Bindings.Installers.InputInstallers;
using Bindings.Installers.PieceInstallers;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PossibleMoves.Utils
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
            TileEvaluatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            AllPossibleMovesGeneratorInstaller.Install(Container);
        }
    }
}