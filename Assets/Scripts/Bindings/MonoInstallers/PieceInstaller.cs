using Bindings.Installers.InputInstallers;
using Bindings.Installers.MoveInstallers;
using Bindings.Installers.PieceInstallers;
using Bindings.Installers.ViewInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class PieceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            PieceBuildSelectorFactoryInstaller.Install(Container);
            SpriteFactoryInstaller.Install(Container);
            PieceFactoryInstaller.Install(Container);
            BuildPieceFactoryInstaller.Install(Container);
            PieceMoverInstaller.Install(Container);
            MovesGeneratorRepositoryInstaller.Install(Container);
            MovesGeneratorInstaller.Install(Container);
            BuildCommandInstaller.Install(Container);
            TileFactoryInstaller.Install(Container);
            BoardRendererInstaller.Install(Container);
            BoardStateChangeRendererInstaller.Install(Container);
            BuildPointsCalculatorInstaller.Install(Container);
            BuilderInstaller.Install(Container);
            HomeBaseBuildMoveGeneratorInstaller.Install(Container);
            BuildResolverInstaller.Install(Container);
            BuildValidatorInstaller.Install(Container);
            PossibleMovesFactoryInstaller.Install(Container);
            MoveValidatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            BoardInfoInstaller.Install(Container);
            KingMoveFilterInstaller.Install(Container);
            PinnedPieceFilterInstaller.Install(Container);
        }
    }
}