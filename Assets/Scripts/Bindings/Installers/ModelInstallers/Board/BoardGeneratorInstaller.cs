using Models.Services.Board;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Board
{
    public class BoardGeneratorInstaller : Installer<BoardGeneratorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBoardGenerator>().To<BoardGenerator>().AsSingle();
        }
    }
}