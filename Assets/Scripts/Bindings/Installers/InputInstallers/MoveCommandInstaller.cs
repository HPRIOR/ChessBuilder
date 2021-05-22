using Controllers.Commands;
using Controllers.Factories;
using Models.State.Interfaces;
using Zenject;

namespace Bindings.Installers.InputInstallers
{
    public class MoveCommandInstaller : Installer<MoveCommandInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MoveCommandFactory>().AsSingle();
            Container.BindFactory<IBoardPosition, IBoardPosition, MoveCommand, MoveCommand.Factory>().FromNew();
        }
    }
}