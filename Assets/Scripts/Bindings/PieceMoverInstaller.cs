using Zenject;

public class PieceMoverInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPieceMover>().To<PieceMover>().AsSingle();
    }
}
