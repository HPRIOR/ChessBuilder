using Bindings.Installers.BoardInstallers;
using Bindings.Installers.MoveInstallers;
using Bindings.Installers.PieceInstallers;
using Zenject;

namespace Tests.UnitTests.PossibleMoves.PieceMoves.Utils
{
    public static class PossibleMovesBinder
    {
        public static void InstallBindings(DiContainer Container)
        {
            BoardGeneratorInstaller.Install(Container);
            PieceSpawnerInstaller.Install(Container);
            MoveGeneratorRepositoryInstaller.Install(Container);
            TileEvaluatorInstaller.Install(Container);
            BoardScannerInstaller.Install(Container);
            PositionTranslatorInstaller.Install(Container);
            AllPossibleMovesGeneratorInstaller.Install(Container);
            PossibleMovesFactoryInstaller.Install(Container);
            BoardInfoInstaller.Install(Container);
            KingMoveFilterInstaller.Install(Container);
        }
    }
}