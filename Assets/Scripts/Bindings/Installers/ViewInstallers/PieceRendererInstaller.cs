using View.Interfaces;
using View.Renderers;
using Zenject;

namespace Bindings.Installers.ViewInstallers
{
    public class PieceRendererInstaller : Installer<PieceRendererInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IPieceRenderer>().To<PieceRenderer>().AsSingle();
        }
    }
}