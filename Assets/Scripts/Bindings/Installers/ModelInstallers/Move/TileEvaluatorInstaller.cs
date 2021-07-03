using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
using Models.Services.Moves.MoveHelpers;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.ModelInstallers.Move
{
    public class TileEvaluatorInstaller : Installer<TileEvaluatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ITileEvaluatorFactory>().To<TileEvaluatorFactory>().AsSingle();
            Container.BindFactory<PieceColour, TileEvaluator, TileEvaluator.Factory>().FromNew();
        }
    }
}