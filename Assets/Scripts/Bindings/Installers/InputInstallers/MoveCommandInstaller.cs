using Zenject;

public class MoveCommandInstaller : Installer<MoveCommandInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<MoveCommandFactory>().AsSingle();
        Container.BindFactory<IBoardPosition, IBoardPosition, MoveCommand, MoveCommand.Factory>().FromNew();
    }
}