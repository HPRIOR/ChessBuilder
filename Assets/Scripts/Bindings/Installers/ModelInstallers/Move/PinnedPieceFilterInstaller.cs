using Models.Services.Moves.Utils;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Move
{
    public class PinnedPieceFilterInstaller : Installer<PinnedPieceFilterInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PinnedPieceFilter>().AsSingle();
        }
    }
}