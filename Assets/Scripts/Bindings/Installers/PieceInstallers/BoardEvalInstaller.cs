using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class BoardEvalInstaller : Installer<BoardEvalInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBoardInfo>().To<BoardInfo>().AsSingle();
        }
    }
}