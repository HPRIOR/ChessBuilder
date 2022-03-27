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
            BuildValidatorInstaller.Install(Container);
            HomeBaseBuildMoveGeneratorInstaller.Install(Container);

            //Move
            BoardScannerInstaller.Install(Container);
            KingMoveFilterInstaller.Install(Container);
            MovesGeneratorInstaller.Install(Container);
            MovesGeneratorRepositoryInstaller.Install(Container);
            MoveValidatorInstaller.Install(Container);
            PinnedPieceFilterInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            PossibleMovesFactoryInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            TileFactoryInstaller.Install(Container);
            CheckedStateManagerInstaller.Install(Container);
        }
    }
}