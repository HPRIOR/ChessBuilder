using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveGenerators;
using Zenject;

namespace Bindings.Installers.PieceInstallers
{
    public class AllPossibleMovesGeneratorInstaller : Installer<AllPossibleMovesGeneratorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAllPossibleMovesGenerator>().To<PossibleTurnMovesGenerator>().AsSingle();
        }
    }
}