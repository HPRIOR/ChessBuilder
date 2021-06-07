using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators.TurnMoves;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.PossibleMoveInstallers
{
    public class BishopTurnMovesInstaller : Installer<BishopTurnMovesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PossibleBishopMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, BishopTurnMoves, BishopTurnMoves.Factory>().FromNew();
        }
    }
}