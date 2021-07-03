using Bindings.Installers.ViewInstallers;
using Zenject;

namespace Bindings.MonoInstallers
{
    public class ViewInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BoardRendererInstaller.Install(Container);
            BoardStateChangeRendererInstaller.Install(Container);
            BuildPieceFactoryInstaller.Install(Container);
            PieceBuildSelectorFactoryInstaller.Install(Container);
            PieceFactoryInstaller.Install(Container);
            SpriteFactoryInstaller.Install(Container);
        }
    }
}