using Bindings.Installers.PieceInstallers;
using Bindings.Installers.PossibleMoveInstallers;
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

            PossibleBishopMovesInstaller.Install(Container);
            PossibleKingMovesInstaller.Install(Container);
            PossibleKnightMovesInstaller.Install(Container);
            PossiblePawnMovesInstaller.Install(Container);
            PossibleQueenMovesInstaller.Install(Container);
            PossibleRookMovesInstaller.Install(Container);

            MoveValidatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            BoardEvalInstaller.Install(Container);
            NonTurnRemoverInstaller.Install(Container);
        }
    }
}