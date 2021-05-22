using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.PieceState;
using Zenject;

public class BoardEvalInstaller : Installer<BoardEvalInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IBoardEvalFactory>().To<BoardEvalFactory>().AsSingle();
        Container.BindFactory<PieceColour, BoardMoveEval, BoardMoveEval.Factory>().FromNew();
    }
}