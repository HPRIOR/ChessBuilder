using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.PossibleMoveInstallers
{
    public class PossibleBishopMovesInstaller : Installer<PossibleBishopMovesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PossibleBishopMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, PossibleBishopMoves, PossibleBishopMoves.Factory>().FromNew();
        }
    }
}