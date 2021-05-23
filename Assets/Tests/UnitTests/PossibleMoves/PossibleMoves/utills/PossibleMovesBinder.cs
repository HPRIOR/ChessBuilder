using Bindings.Installers.BoardInstallers;
using Bindings.Installers.InputInstallers;
using Bindings.Installers.PieceInstallers;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PossibleMoves.utills
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
            BoardEvalInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            BoardPositionTranslatorInstaller.Install(Container);
            AllPossibleMovesGeneratorInstaller.Install(Container);
        }
    }
}