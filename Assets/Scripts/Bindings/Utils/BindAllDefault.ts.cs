using Bindings.Installers.ControllerInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Build;
using Bindings.Installers.ModelInstallers.Move;
using Bindings.Installers.ViewInstallers;
using Zenject;

namespace Bindings.Utils
{
    public static class BindAllDefault
    {
        public static void InstallAllTo(DiContainer container)
        {
            HomeBaseBuildMoveGeneratorInstaller.Install(container);
            BoardGeneratorInstaller.Install(container);
            GameStateControllerInstaller.Install(container);
            CommandInvokerInstaller.Install(container);
            MoveCommandInstaller.Install(container);
            TileEvaluatorInstaller.Install(container);
            PositionTranslatorInstaller.Install(container);
            BoardScannerInstaller.Install(container);
            PieceFactoryInstaller.Install(container);
            MovesGeneratorInstaller.Install(container);
            MovesGeneratorRepositoryInstaller.Install(container);
            BoardInfoInstaller.Install(container);
            KingMoveFilterInstaller.Install(container);
            PossibleMovesFactoryInstaller.Install(container);
            PinnedPieceFilterInstaller.Install(container);
            GameOverEvalInstaller.Install(container);
            GameStateUpdaterInstaller.Install(container);
            GameInitializerInstaller.Install(container);
            CheckedStateManagerInstaller.Install(container);
        }
    }
}