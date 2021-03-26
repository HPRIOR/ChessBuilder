using Zenject;

public class PieceMoveGeneratorFactoryInstaller : Installer<PieceMoveGeneratorFactoryInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IPieceMoveGeneratorFactory>().To<PieceMoveGeneratorFactory>().AsSingle();
    }
}