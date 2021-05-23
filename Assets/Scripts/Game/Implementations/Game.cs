using Game.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using UnityEngine;
using Zenject;

namespace Game.Implementations
{
    public class Game : MonoBehaviour
    {
        private IBoardGenerator _boardGenerator;
        private IGameState GameState { get; set; }

        public void Start()
        {
            GameState.UpdateGameState(InitBoard());
        }

        [Inject]
        public void Construct(
            IGameState initState,
            IBoardGenerator boardGenerator
        )
        {
            _boardGenerator = boardGenerator;
            GameState = initState;
        }

        private BoardState InitBoard()
        {
            var board = _boardGenerator.GenerateBoard();
            board[3, 3].CurrentPiece = new Piece(PieceType.WhitePawn);
            board[4, 4].CurrentPiece = new Piece(PieceType.BlackPawn);
            return new BoardState(board);
        }
    }
}