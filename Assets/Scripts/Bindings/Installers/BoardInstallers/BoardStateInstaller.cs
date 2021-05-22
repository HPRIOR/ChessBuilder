using Models.Services.Board;
using Models.Services.Interfaces;
using Zenject;

namespace Bindings.Installers.BoardInstallers
{
    public class BoardStateInstaller : Installer<BoardStateInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBoardGenerator>().To<BoardGenerator>().AsSingle();
        }
    }
}