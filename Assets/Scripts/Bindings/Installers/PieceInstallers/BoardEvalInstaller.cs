﻿using Models.Services.Interfaces;
using Models.Services.Moves.Factories;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Piece;
using Zenject;

public class BoardEvalInstaller : Installer<BoardEvalInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IBoardEvalFactory>().To<BoardEvalFactory>().AsSingle();
        Container.BindFactory<PieceColour, BoardEval, BoardEval.Factory>().FromNew();
    }
}