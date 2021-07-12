using Models.Services.Game.Interfaces;
using Models.Services.Interfaces;
using Models.State.Board;
using Models.State.PieceState;
using UnityEngine;
using Zenject;

namespace Models.Services.Game.Implementations
{
    public class Game : MonoBehaviour
    {
        private IBoardGenerator _boardGenerator;
        private IGameStateController GameStateController { get; set; }

        public void Start()
        {
            GameStateController.UpdateGameState(InitBoard());
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
            // board[0, 0].CurrentPiece = new Piece(PieceType.WhiteRook);
            // board[1, 0].CurrentPiece = new Piece(PieceType.WhiteKnight);
            // board[2, 0].CurrentPiece = new Piece(PieceType.WhiteBishop);
            // board[3, 0].CurrentPiece = new Piece(PieceType.WhiteQueen);
            board[4, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            // board[5, 0].CurrentPiece = new Piece(PieceType.WhiteBishop);
            // board[6, 0].CurrentPiece = new Piece(PieceType.WhiteKnight);
            // board[7, 0].CurrentPiece = new Piece(PieceType.WhiteRook);

            // board[0, 1].CurrentPiece = new Piece(PieceType.WhitePawn);
            // board[1, 1].CurrentPiece = new Piece(PieceType.WhitePawn);
            // board[2, 1].CurrentPiece = new Piece(PieceType.WhitePawn);
            // board[3, 1].CurrentPiece = new Piece(PieceType.WhitePawn);
            // board[4, 1].CurrentPiece = new Piece(PieceType.WhitePawn);
            // board[5, 1].CurrentPiece = new Piece(PieceType.WhitePawn);
            // board[6, 1].CurrentPiece = new Piece(PieceType.WhitePawn);
            // board[7, 1].CurrentPiece = new Piece(PieceType.WhitePawn);

            // board[0, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            // board[1, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            // board[2, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            // board[3, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            // board[4, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            // board[5, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            // board[6, 6].CurrentPiece = new Piece(PieceType.BlackPawn);
            // board[7, 6].CurrentPiece = new Piece(PieceType.BlackPawn);

            // board[0, 7].CurrentPiece = new Piece(PieceType.BlackRook);
            // board[1, 7].CurrentPiece = new Piece(PieceType.BlackKnight);
            // board[2, 7].CurrentPiece = new Piece(PieceType.BlackBishop);
            board[3, 7].CurrentPiece = new Piece(PieceType.BlackKing);
            // board[4, 7].CurrentPiece = new Piece(PieceType.BlackQueen);
            // board[5, 7].CurrentPiece = new Piece(PieceType.BlackBishop);
            // board[6, 7].CurrentPiece = new Piece(PieceType.BlackKnight);
            // board[7, 7].CurrentPiece = new Piece(PieceType.BlackRook);
            return new BoardState(board);
        }
    }
}