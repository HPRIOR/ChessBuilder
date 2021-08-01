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
            BuildResolverInstaller.Install(container);
            BuildPointsCalculatorInstaller.Install(container);
            HomeBaseBuildMoveGeneratorInstaller.Install(container);
            BoardGeneratorInstaller.Install(container);
            GameStateControllerInstaller.Install(container);
            CommandInvokerInstaller.Install(container);
            MoveCommandInstaller.Install(container);
            TileEvaluatorInstaller.Install(container);
            PositionTranslatorInstaller.Install(container);
            BoardScannerInstaller.Install(container);
            MoveValidatorInstaller.Install(container);
            PieceMoverInstaller.Install(container);
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
            BuilderInstaller.Install(container);
        }
    }
}