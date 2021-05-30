using Bindings.Installers.BoardInstallers;
using Bindings.Installers.GameInstallers;
using Bindings.Installers.InputInstallers;
using Bindings.Installers.PieceInstallers;
using Bindings.Installers.PossibleMoveInstallers;
using Zenject;

namespace Bindings.Utils
{
    public static class BindAllDefault
    {
        public static void InstallAllTo(DiContainer container)
        {
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
            PossibleBishopMovesInstaller.Install(container);
            PossibleKingMovesInstaller.Install(container);
            PossibleKnightMovesInstaller.Install(container);
            PossiblePawnMovesInstaller.Install(container);
            PossibleQueenMovesInstaller.Install(container);
            PossibleRookMovesInstaller.Install(container);
            PossibleMoveFactoryInstaller.Install(container);
            BoardEvalInstaller.Install(container);
        }
    }
}