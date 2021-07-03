using Bindings.Installers.ModelInstallers.Board;
using Bindings.Installers.ModelInstallers.Build;
using Bindings.Installers.ModelInstallers.Move;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class ModelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Board
            BoardGeneratorInstaller.Install(Container);
            BoardInfoInstaller.Install(Container);

            // Build
            BuilderInstaller.Install(Container);
            BuildPointsCalculatorInstaller.Install(Container);
            BuildResolverInstaller.Install(Container);
            BuildValidatorInstaller.Install(Container);
            HomeBaseBuildMoveGeneratorInstaller.Install(Container);

            //Move
            BoardScannerInstaller.Install(Container);
            KingMoveFilterInstaller.Install(Container);
            MovesGeneratorInstaller.Install(Container);
            MovesGeneratorRepositoryInstaller.Install(Container);
            MoveValidatorInstaller.Install(Container);
            PieceMoverInstaller.Install(Container);
            PinnedPieceFilterInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            PossibleMovesFactoryInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            TileFactoryInstaller.Install(Container);
        }
    }
}