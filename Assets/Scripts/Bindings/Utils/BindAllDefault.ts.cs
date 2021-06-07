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
            BishopTurnMovesInstaller.Install(container);
            KingTurnMovesInstaller.Install(container);
            KnightTurnMovesInstaller.Install(container);
            PawnTurnMovesInstaller.Install(container);
            QueenTurnMovesInstaller.Install(container);
            RookTurnMovesInstaller.Install(container);
            PossibleMoveFactoryInstaller.Install(container);
            BoardEvalInstaller.Install(container);
            NonTurnRemoverInstaller.Install(container);
        }
    }
}