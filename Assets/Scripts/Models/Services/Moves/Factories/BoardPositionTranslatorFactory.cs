using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.Piece;

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