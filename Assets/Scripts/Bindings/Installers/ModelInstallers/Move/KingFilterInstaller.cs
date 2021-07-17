using Models.Services.Moves.Utils;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Move
{
    public class KingMoveFilterInstaller : Installer<KingMoveFilterInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<KingMoveFilter>().AsSingle();
        }
    }
}