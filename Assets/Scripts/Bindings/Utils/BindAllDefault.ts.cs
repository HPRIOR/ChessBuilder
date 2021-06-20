using Bindings.Installers.BoardInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.InputInstallers;
using Bindings.Installers.MoveInstallers;
using Bindings.Installers.PieceInstallers;
using Zenject;

namespace Bindings.Utils
{
    public static class BindAllDefault
    {
        public static void InstallAllTo(DiContainer container)
        {
            HomeBaseBuildMoveGeneratorInstaller.Install(container);
            BoardGeneratorInstaller.Install(container);
            GameStateInstaller.Install(container);
            CommandInvokerInstaller.Install(container);
            MoveCommandInstaller.Install(container);
            TileEvaluatorInstaller.Install(container);
            PositionTranslatorInstaller.Install(container);
            BoardScannerInstaller.Install(container);
            MoveValidatorInstaller.Install(container);
            PieceMoverInstaller.Install(container);
            PieceSpawnerInstaller.Install(container);
            AllPossibleMovesGeneratorInstaller.Install(container);
            MoveGeneratorRepositoryInstaller.Install(container);
            BoardInfoInstaller.Install(container);
            KingMoveFilterInstaller.Install(container);
            PossibleMovesFactoryInstaller.Install(container);
            PinnedPieceFilterInstaller.Install(container);
        }
    }
}