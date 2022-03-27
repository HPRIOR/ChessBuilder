using Models.Services.Moves.Utils;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Move
{
    public class CheckedStateManagerInstaller : Installer<CheckedStateManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<CheckedStateManager>().AsSingle();
        }
    }
}