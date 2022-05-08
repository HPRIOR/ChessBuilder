using Models.Services.Game.Implementations;
using Zenject;

namespace Bindings.Installers.GameInstallers
{
    public class GameContextInstaller : Installer<GameContextInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<GameContext>().AsSingle();
        }
    }
}