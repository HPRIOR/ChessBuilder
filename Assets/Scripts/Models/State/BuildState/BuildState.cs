using Models.State.PieceState;

namespace Models.State.BuildState
{
    public readonly struct BuildState
    {
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
            Turns = BuildPoints.PieceCost[pieceType];
        }


        public BuildState Decrement() => Turns == 0 ? this : new BuildState(Turns - 1, BuildingPiece);
    }
}