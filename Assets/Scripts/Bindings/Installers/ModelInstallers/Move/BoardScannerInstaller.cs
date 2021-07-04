using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
using Models.Services.Moves.Utils;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Move
{
    public class BoardScannerInstaller : Installer<BoardScannerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ITileEvaluator>().To<TileEvaluator>().AsSingle();
            Container.Bind<IBoardScannerFactory>().To<BoardScannerFactory>().AsSingle();
            Container.BindFactory<PieceColour, BoardScanner, BoardScanner.Factory>().FromNew();
            Container.BindFactory<PieceColour, NonTurnBoardScanner, NonTurnBoardScanner.Factory>().FromNew();
        }
    }
}