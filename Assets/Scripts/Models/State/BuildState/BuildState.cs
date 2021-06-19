using System.Collections.Generic;
using Models.State.PieceState;

namespace Models.State.BuildState
{
    public readonly struct BuildState
    {
        private static readonly IDictionary<PieceType, int> PieceCost = new Dictionary<PieceType, int>
        {
            {PieceType.BlackPawn, 1},
            {PieceType.WhitePawn, 1},
            {PieceType.BlackBishop, 3},
            {PieceType.WhiteBishop, 3},
            {PieceType.BlackKnight, 3},
            {PieceType.WhiteKnight, 3},
            {PieceType.BlackRook, 5},
            {PieceType.WhiteRook, 5},
            {PieceType.BlackQueen, 5},
            {PieceType.WhiteQueen, 5}
        };

        public PieceType BuildingPiece { get; }
        public int Turns { get; }

        public BuildState(int turns = 0, PieceType buildingPiece = default)
        {
            BuildingPiece = buildingPiece;
            Turns = turns;
        }

        public BuildState(PieceType pieceType)
        {
            BuildingPiece = pieceType;
            Turns = PieceCost[pieceType];
        }


        public BuildState Decrement() => Turns == 0 ? this : new BuildState(Turns - 1, BuildingPiece);
    }
}