using View.Interfaces;
using View.Renderers;
using Zenject;

namespace Bindings.Installers.ViewInstallers
{
    public class BoardStateChangeRendererInstaller : Installer<BoardStateChangeRendererInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IStateChangeRenderer>().WithId("Piece").To<PieceRenderer>().AsSingle();
            Container.Bind<IStateChangeRenderer>().WithId("Build").To<BuildRenderer>().AsSingle();
        }
    }
}