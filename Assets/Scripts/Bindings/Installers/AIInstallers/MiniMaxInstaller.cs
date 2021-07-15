using Models.Services.AI;
using Zenject;

namespace Bindings.Installers.AIInstallers
{
    public class MiniMaxInstaller: Installer<MiniMaxInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MiniMax>().AsSingle();
        }
    }
}