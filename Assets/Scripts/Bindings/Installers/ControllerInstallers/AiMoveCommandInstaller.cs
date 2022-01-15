using Controllers.Commands;
using Controllers.Factories;
using Models.State.Board;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.ControllerInstallers
{
    public class AIMoveCommandInstaller : Installer<AIMoveCommandInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<AiMoveCommandFactory>().AsSingle();
            Container.BindFactory<AiMoveCommand, AiMoveCommand.Factory>();
        }
    }
}