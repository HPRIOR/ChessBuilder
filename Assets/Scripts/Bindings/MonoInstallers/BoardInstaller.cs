using Zenject;

public class BoardInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BoardStateInstaller.Install(Container);
    }
}