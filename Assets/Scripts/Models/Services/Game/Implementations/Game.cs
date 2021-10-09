using Models.Services.Board;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using UnityEngine;
using Zenject;

namespace Models.Services.Game.Implementations
{
    public sealed class Game : MonoBehaviour
    {
        private IBoardGenerator _boardGenerator;
        private IGameStateController GameStateController { get; set; }

        public void Start()
        {
            GameStateController.InitializeGame(InitBoard());
        }

        [Inject]
        public void Construct(
            IGameStateController initStateController,
            IBoardGenerator boardGenerator
        )
        {
            _boardGenerator = boardGenerator;
            GameStateController = initStateController;
        }

        private BoardState InitBoard()
        {
            var board = _boardGenerator.GenerateBoard();
            board[6][7].CurrentPiece = PieceType.BlackKing;
            board[5][7].CurrentPiece = PieceType.BlackRook;
            board[3][7].CurrentPiece = PieceType.BlackQueen;
            board[0][7].CurrentPiece = PieceType.BlackRook;
            board[1][6].CurrentPiece = PieceType.BlackPawn;
            board[3][6].CurrentPiece = PieceType.BlackBishop;
            board[5][6].CurrentPiece = PieceType.BlackPawn;
            board[6][6].CurrentPiece = PieceType.BlackPawn;
            board[7][6].CurrentPiece = PieceType.BlackPawn;
            board[0][5].CurrentPiece = PieceType.BlackPawn;
            board[2][5].CurrentPiece = PieceType.BlackKnight;
            board[4][5].CurrentPiece = PieceType.BlackPawn;
            board[5][5].CurrentPiece = PieceType.BlackKnight;
            board[3][4].CurrentPiece = PieceType.BlackPawn;

            board[6][0].CurrentPiece = PieceType.WhiteKing;
            board[4][0].CurrentPiece = PieceType.WhiteRook;
            board[3][0].CurrentPiece = PieceType.WhiteRook;
            board[1][1].CurrentPiece = PieceType.WhitePawn;
            board[5][1].CurrentPiece = PieceType.WhitePawn;
            board[6][1].CurrentPiece = PieceType.WhitePawn;
            board[0][2].CurrentPiece = PieceType.WhitePawn;
            board[2][2].CurrentPiece = PieceType.WhiteKnight;
            board[3][2].CurrentPiece = PieceType.WhiteQueen;
            board[5][2].CurrentPiece = PieceType.WhiteKnight;
            board[7][2].CurrentPiece = PieceType.WhitePawn;
            board[1][3].CurrentPiece = PieceType.WhitePawn;
            board[5][3].CurrentPiece = PieceType.WhiteBishop;

            return new BoardState(board);
        }
    }
}