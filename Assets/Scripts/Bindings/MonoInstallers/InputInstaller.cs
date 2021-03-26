using Zenject;

public class InputInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        DragAndDropCommandInstaller.Install(Container);
        CommandInvokerInstaller.Install(Container);
        MoveDataInstaller.Install(Container);
    }
}