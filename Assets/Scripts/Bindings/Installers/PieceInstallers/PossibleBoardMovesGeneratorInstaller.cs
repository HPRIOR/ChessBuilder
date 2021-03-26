using Zenject;

public class PossibleBoardMovesGeneratorInstaller : Installer<PossibleBoardMovesGeneratorInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IPossibleBoardMovesGenerator>().To<PossibleBoardMovesGenerator>().AsSingle();
    }
}