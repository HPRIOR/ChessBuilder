using Bindings.Installers.MoveInstallers;
using Bindings.Installers.PieceInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class PieceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            PieceSpawnerInstaller.Install(Container);
            PieceMoverInstaller.Install(Container);
            PossibleMoveFactoryInstaller.Install(Container);
            AllPossibleMovesGeneratorInstaller.Install(Container);

            BishopTurnMovesInstaller.Install(Container);
            KingTurnMovesInstaller.Install(Container);
            KnightTurnMovesInstaller.Install(Container);
            PawnTurnMovesInstaller.Install(Container);
            QueenTurnMovesInstaller.Install(Container);
            RookTurnMovesInstaller.Install(Container);

            MoveValidatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            BoardEvalInstaller.Install(Container);
            NonTurnRemoverInstaller.Install(Container);
        }
    }
}