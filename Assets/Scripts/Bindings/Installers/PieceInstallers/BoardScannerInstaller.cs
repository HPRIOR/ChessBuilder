using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class BoardScannerInstaller : Installer<BoardScannerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBoardMoveEval>().To<BoardMoveEval>().AsSingle();
            Container.Bind<IBoardScannerFactory>().To<BoardScannerFactory>().AsSingle();
            Container.BindFactory<PieceColour, BoardScanner, BoardScanner.Factory>().FromNew();
        }
    }
}