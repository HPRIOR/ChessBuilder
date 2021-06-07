using Models.Services.Moves.PossibleMoveHelpers;
using Zenject;

namespace Bindings.Installers.MoveInstallers
{
    public class KingMoveFilterInstaller : Installer<KingMoveFilterInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<KingMoveFilter>().AsSingle();
        }
    }
}