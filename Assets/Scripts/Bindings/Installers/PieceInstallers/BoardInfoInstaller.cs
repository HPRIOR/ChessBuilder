using Models.Services.Interfaces;
using Models.Services.Moves.MoveHelpers;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class BoardInfoInstaller : Installer<BoardInfoInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBoardInfo>().To<BoardInfo>().AsTransient();
        }
    }
}