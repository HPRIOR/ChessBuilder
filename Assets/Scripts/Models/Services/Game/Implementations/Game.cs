using System.Collections.Generic;
using Models.Services.Board;
using Models.Services.Game.Interfaces;
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
            board[4, 0].CurrentPiece = new Piece(PieceType.WhiteKing);
            board[3, 7].CurrentPiece = new Piece(PieceType.BlackKing);

            var activePieces = new HashSet<Position> {new Position(4, 0), new Position(3, 7)};
            var activeBuilds = new HashSet<Position>();
            return new BoardState(board, activePieces, activeBuilds);
        }
    }
}