using View.Interfaces;
using View.Renderers;
using Zenject;

namespace Bindings.Installers.ViewInstallers
{
    public class BoardStateChangeRendererInstaller : Installer<BoardStateChangeRendererInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBoardStateChangeRenderer>().WithId("Piece").To<PieceRenderer>().AsSingle();
            Container.Bind<IBoardStateChangeRenderer>().WithId("Build").To<BuildRenderer>().AsSingle();
        }
    }
}