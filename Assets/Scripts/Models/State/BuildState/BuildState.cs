using System.Collections.Generic;
using Models.State.PieceState;

namespace Models.State.BuildState
{
    public readonly struct BuildState
    {
        private static readonly IDictionary<PieceType, int> pieceCost = new Dictionary<PieceType, int>
        {
            {PieceType.BlackPawn, 3},
            {PieceType.WhitePawn, 3}
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
            Turns = pieceCost[pieceType];
        }


        public BuildState Decrement() => Turns == 0 ? this : new BuildState(Turns - 1, BuildingPiece);
    }
}