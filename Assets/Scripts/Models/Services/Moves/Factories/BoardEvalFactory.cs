using Models.Services.Interfaces;
using Models.Services.Moves.PossibleMoveHelpers;
using Models.State.PieceState;

namespace Models.Services.Moves.Factories
{
    public class BoardEvalFactory : IBoardEvalFactory
    {
        private readonly BoardMoveEval.Factory _boardEvalFactory;

        public BoardEvalFactory(BoardMoveEval.Factory boardEvalFactory)
        {
            _boardEvalFactory = boardEvalFactory;
        }

        public IBoardMoveEval Create(PieceColour pieceColour) =>
            _boardEvalFactory.Create(pieceColour);
    }
}