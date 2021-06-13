using Controllers.Commands;
using Controllers.Factories;
using Models.State.Board;
using Zenject;

namespace Bindings.Installers.InputInstallers
{
    public class MoveCommandInstaller : Installer<MoveCommandInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MoveCommandFactory>().AsSingle();
            Container.BindFactory<Position, Position, MoveCommand, MoveCommand.Factory>().FromNew();
        }
    }
}