using Bindings.Installers.BoardInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class BoardInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BoardStateInstaller.Install(Container);
        }
    }
}