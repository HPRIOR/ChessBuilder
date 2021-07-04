using Models.Services.Interfaces;
using Models.Services.Moves.Utils;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Board
{
    public class BoardInfoInstaller : Installer<BoardInfoInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBoardInfo>().To<BoardInfo>().AsTransient();
        }
    }
}