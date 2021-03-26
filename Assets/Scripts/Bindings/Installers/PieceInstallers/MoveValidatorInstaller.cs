using Zenject;

public class MoveValidatorInstaller : Installer<MoveValidatorInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IMoveValidator>().To<MoveValidator>().AsSingle();
    }
}