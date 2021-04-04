using UnityEngine;
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
            board[3, 3].CurrentPiece = PieceType.WhiteQueen;
            board[4, 4].CurrentPiece = PieceType.BlackQueen;
            return new BoardState(board);
        }
    }
}