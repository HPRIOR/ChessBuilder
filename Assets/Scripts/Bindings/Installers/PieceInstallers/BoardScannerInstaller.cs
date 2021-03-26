using Zenject;

public class BoardScannerInstaller : Installer<BoardScannerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IBoardEval>().To<BoardEval>().AsSingle();
        Container.Bind<IBoardScannerFactory>().To<BoardScannerFactory>().AsSingle();
        Container.BindFactory<PieceColour, BoardScanner, BoardScanner.Factory>().FromNew();
    }
}