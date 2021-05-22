using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class BoardPositionTranslatorInstaller : Installer<BoardPositionTranslatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IPositionTranslatorFactory>().To<BoardPositionTranslatorFactory>().AsSingle();
            Container.BindFactory<PieceColour, PositionTranslator, PositionTranslator.Factory>().FromNew();
        }
    }
}