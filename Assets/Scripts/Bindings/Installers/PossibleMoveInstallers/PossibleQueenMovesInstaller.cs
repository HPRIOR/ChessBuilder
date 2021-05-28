using Models.Services.Moves.Factories.PossibleMoveGeneratorFactories;
using Models.Services.Moves.PossibleMoveGenerators;
using Models.State.PieceState;
using Zenject;

namespace Bindings.Installers.PossibleMoveInstallers
{
    public class PossibleQueenMovesInstaller : Installer<PossibleQueenMovesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PossibleQueenMovesFactory>().AsSingle();
            Container.BindFactory<PieceColour, PossibleQueenMoves, PossibleQueenMoves.Factory>().FromNew();
        }
    }
}