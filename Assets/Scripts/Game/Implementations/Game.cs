using Game.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.Interfaces;
using Models.State.Piece;
using UnityEngine;
using Zenject;

namespace Game.Implementations
{
    public class Game : MonoBehaviour
    {
        private IBoardGenerator _boardGenerator;
        private IGameState GameState { get; set; }

        [Inject]
        public void Construct(
            IGameState initState,
            IBoardGenerator boardGenerator
            )
        {
            _boardGenerator = boardGenerator;
            GameState = initState;
        }

        public void Start()
        {
            GameState.UpdateGameState(InitBoard());
        }

        private IBoardState InitBoard()
        {
            var board = _boardGenerator.GenerateBoard();
            board[3, 3].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[4, 4].CurrentPiece = new Piece(PieceType.BlackQueen);
            return new BoardState(board);
        }
    }
}