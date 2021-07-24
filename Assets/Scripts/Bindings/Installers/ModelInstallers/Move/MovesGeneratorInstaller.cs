using Models.Services.Moves.Interfaces;
using Models.Services.Moves.MoveGenerators;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Move
{
    public class MovesGeneratorInstaller : Installer<MovesGeneratorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IMovesGenerator>().To<MovesGenerator>().AsSingle();
        }
    }
}