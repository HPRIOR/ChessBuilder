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
            MoveValidatorInstaller.Install(Container);
            BoardEvalInstaller.Install(Container);
            BoardPositionTranslatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
        }
    }
}