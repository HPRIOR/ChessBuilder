using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.PossibleMoveInstallers
{
    public class PossibleKnightMovesInstaller : Installer<PossibleKnightMovesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PossibleKnightMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, PossibleKnightMoves, PossibleKnightMoves.Factory>().FromNew();
        }
    }
}