using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.PossibleMoveInstallers
{
    public class PossibleRookMovesInstaller : Installer<PossibleRookMovesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PossibleRookMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, PossibleRookMoves, PossibleRookMoves.Factory>().FromNew();
        }
    }
}