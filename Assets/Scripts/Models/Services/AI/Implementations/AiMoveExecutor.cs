using Models.Services.Game.Interfaces;
using Models.State.PieceState;

namespace Models.Services.AI.Implementations
{
    public class AiMoveExecutor 
    {
        private readonly AiMoveGenerator _aiMoveGenerator;

        public AiMoveExecutor(AiMoveGenerator aiMoveGenerator)
        {
            _aiMoveGenerator = aiMoveGenerator;
        }
        
        public void MakeMove(IGameStateController gameStateController, PieceColour turn)
        {
            var move = _aiMoveGenerator.GetMove(gameStateController.CurrentGameState, 4, turn);
            var isPieceMove = move.MoveType == MoveType.Move;
            if (isPieceMove)
                gameStateController.UpdateGameState(move.From, move.To);
            else
                gameStateController.UpdateGameState(move.From, move.Type);
        }
    }
}