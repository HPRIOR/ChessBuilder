using Models.Services.Moves.Factories;
using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Move
{
    public class PositionTranslatorInstaller : Installer<PositionTranslatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IPositionTranslatorFactory>().To<BoardPositionTranslatorFactory>().AsSingle();
            Container.BindFactory<PieceColour, PositionTranslator, PositionTranslator.Factory>().FromNew();
        }
    }
}