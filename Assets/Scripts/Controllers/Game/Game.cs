using UnityEngine;
using Assets.Scripts.Models.Piece;
using Zenject;

namespace Assets.Scripts.Controllers.Game
{
    public class Game : MonoBehaviour
    {
        private IBoardGenerator _boardGenerator;
        public IGameState GameState { get; private set; }

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