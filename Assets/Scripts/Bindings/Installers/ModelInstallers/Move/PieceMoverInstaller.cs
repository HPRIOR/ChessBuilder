using Models.Services.Moves.Interfaces;
using Models.Services.Moves.MoveGenerators;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Move
{
    public class PieceMoverInstaller : Installer<PieceMoverInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IPieceMover>().To<PieceMover>().AsSingle();
        }
    }
}