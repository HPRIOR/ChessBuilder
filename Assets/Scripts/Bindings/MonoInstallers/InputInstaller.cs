using Zenject;

public class InputInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        MoveCommandInstaller.Install(Container);
        CommandInvokerInstaller.Install(Container);
    }
}