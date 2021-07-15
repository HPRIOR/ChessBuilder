using Models.Services.AI;
using Models.Services.AI.Interfaces;
using Zenject;

namespace Bindings.Installers.AIInstallers
{
    public class StaticEvaluatorInstaller: Installer<StaticEvaluatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IStaticEvaluator>().To<StaticEvaluator>().AsSingle();
        }
    }
}