﻿using Models.Services.Moves.Interfaces;
using Models.Services.Moves.Utils;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories
{
    public class BoardPositionTranslatorFactory : IPositionTranslatorFactory
    {
        private readonly PositionTranslator.Factory _boardPositionTranslatorFactory;

        public BoardPositionTranslatorFactory(PositionTranslator.Factory boardPositionTranslatorFactory)
        {
            _boardPositionTranslatorFactory = boardPositionTranslatorFactory;
        }

        public IPositionTranslator Create(PieceColour pieceColour) =>
            _boardPositionTranslatorFactory.Create(pieceColour);
    }
}