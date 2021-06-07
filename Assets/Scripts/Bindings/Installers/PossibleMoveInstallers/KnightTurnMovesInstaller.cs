using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.PossibleMoveInstallers
{
    public class KnightTurnMovesInstaller : Installer<KnightTurnMovesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PossibleKnightMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, KnightTurnMoves, KnightTurnMoves.Factory>().FromNew();
        }
    }
}