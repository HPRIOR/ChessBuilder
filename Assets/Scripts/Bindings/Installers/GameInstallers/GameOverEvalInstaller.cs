using Models.Services.Game.Implementations;
using Models.Services.Game.Interfaces;
using Zenject;

namespace Bindings.Installers.GameInstallers
{
    public class GameOverEvalInstaller : Installer<GameOverEvalInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameOverEval>().To<GameOverEval>().AsSingle();
        }
    }
}