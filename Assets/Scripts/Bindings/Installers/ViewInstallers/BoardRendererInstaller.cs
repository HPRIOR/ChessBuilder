using View.Interfaces;
using View.Renderers;
using Zenject;

namespace Bindings.Installers.ViewInstallers
{
    public class BoardRendererInstaller : Installer<BoardRendererInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBoardRenderer>().To<BoardRenderer>().AsSingle();
        }
    }
}