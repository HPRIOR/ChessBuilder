using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.PossibleMoveInstallers
{
    public class PossiblePawnMovesInstaller : Installer<PossiblePawnMovesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PossiblePawnMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, PossiblePawnMoves, PossiblePawnMoves.Factory>().FromNew();
        }
    }
}