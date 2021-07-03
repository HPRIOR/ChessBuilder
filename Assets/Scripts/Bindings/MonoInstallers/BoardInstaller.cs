using Bindings.Installers.ModelInstallers.Board;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class BoardInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BoardGeneratorInstaller.Install(Container);
        }
    }
}