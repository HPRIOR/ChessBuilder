using System.Collections.Generic;
using Models.State.PieceState;

namespace Models.State.BuildState
{
    public readonly struct BuildState
    {
        private static readonly IDictionary<PieceType, int> PieceCost = new Dictionary<PieceType, int>
        {
            {PieceType.NullPiece, 0},
            {PieceType.BlackKing, 0},
            {PieceType.WhiteKing, 0},
            {PieceType.BlackPawn, 1},
            {PieceType.WhitePawn, 1},
            {PieceType.BlackBishop, 3},
            {PieceType.WhiteBishop, 3},
            {PieceType.BlackKnight, 3},
            {PieceType.WhiteKnight, 3},
            {PieceType.BlackRook, 5},
            {PieceType.WhiteRook, 5},
            {PieceType.BlackQueen, 9},
            {PieceType.WhiteQueen, 9}
        };

        public PieceType BuildingPiece { get; }
        public int Turns { get; }

        // used to instantiate default/null build state, and to decrement build states
        public BuildState(int turns = 0, PieceType buildingPiece = default)
        {
            BuildingPiece = buildingPiece;
            Turns = turns;
        }

        // used to instantiate initial value of BuildState
        public BuildState(PieceType pieceType)
        {
            BuildingPiece = pieceType;
            Turns = PieceCost[pieceType];
        }


        public BuildState Decrement() => Turns == 0 ? this : new BuildState(Turns - 1, BuildingPiece);
    }
}