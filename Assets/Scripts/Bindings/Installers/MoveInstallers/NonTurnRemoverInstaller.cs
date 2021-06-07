using Models.Services.Moves.PossibleMoveHelpers;
using Zenject;

namespace Bindings.Installers.MoveInstallers
{
    public class NonTurnRemoverInstaller : Installer<NonTurnRemoverInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<KingMoveFilter>().AsSingle();
        }
    }
}