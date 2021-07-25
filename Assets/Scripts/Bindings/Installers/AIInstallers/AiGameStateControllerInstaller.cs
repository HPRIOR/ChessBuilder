using Models.Services.Game.Implementations;
using Models.Services.Game.Interfaces;
using Zenject;

namespace Bindings.Installers.AIInstallers
{
    public class AiGameStateControllerInstaller : Installer<AiGameStateControllerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(ITurnEventInvoker), typeof(IGameStateController)).To<AiGameStateController>()
                .AsSingle();
        }
    }
}