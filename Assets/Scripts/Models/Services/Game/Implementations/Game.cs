using System.Collections.Generic;
using Models.Services.Board;
using Models.Services.Game.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using UnityEngine;
using Zenject;

namespace Models.Services.Game.Implementations
{
    public sealed class Game : MonoBehaviour
    {
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
            GameStateController = initStateController;
        }

        private BoardState InitBoard()
        {
            var piecesDict = new Dictionary<Position, PieceType>()
            {
                { new Position(6, 7), PieceType.BlackKing },
                { new Position(5, 7), PieceType.BlackRook },
                { new Position(3, 7), PieceType.BlackQueen },
                { new Position(0, 7), PieceType.BlackRook },
                { new Position(1, 6), PieceType.BlackPawn },
                { new Position(3, 6), PieceType.BlackBishop },
                { new Position(5, 6), PieceType.BlackPawn },
                { new Position(6, 6), PieceType.BlackPawn },
                { new Position(7, 6), PieceType.BlackPawn },
                { new Position(0, 5), PieceType.BlackPawn },
                { new Position(2, 5), PieceType.BlackKnight },
                { new Position(4, 5), PieceType.BlackPawn },
                { new Position(5, 5), PieceType.BlackKnight },
                { new Position(3, 4), PieceType.BlackPawn },

                { new Position(6, 0), PieceType.WhiteKing },
                { new Position(4, 0), PieceType.WhiteRook },
                { new Position(3, 0), PieceType.WhiteRook },
                { new Position(1, 1), PieceType.WhitePawn },
                { new Position(5, 1), PieceType.WhitePawn },
                { new Position(6, 1), PieceType.WhitePawn },
                { new Position(0, 2), PieceType.WhitePawn },
                { new Position(2, 2), PieceType.WhiteKnight },
                { new Position(3, 2), PieceType.WhiteQueen },
                { new Position(5, 2), PieceType.WhiteKnight },
                { new Position(7, 2), PieceType.WhitePawn },
                { new Position(1, 3), PieceType.WhitePawn },
                { new Position(5, 3), PieceType.WhiteBishop },
            };

            return new BoardState(piecesDict);
        }
    }
}