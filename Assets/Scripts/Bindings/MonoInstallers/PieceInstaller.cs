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
            MoveGeneratorRepositoryInstaller.Install(Container);
            AllPossibleMovesGeneratorInstaller.Install(Container);


            PossibleMovesFactoryInstaller.Install(Container);
            MoveValidatorInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            BoardEvalInstaller.Install(Container);
            KingMoveFilterInstaller.Install(Container);
        }
    }
}