using Controllers.Commands;
using Controllers.Factories;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.ControllerInstallers
{
    public class BuildCommandInstaller : Installer<BuildCommandInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BuildCommandFactory>().AsSingle();
            Container.BindFactory<Position, PieceType, BuildCommand, BuildCommand.Factory>();
        }
    }
}