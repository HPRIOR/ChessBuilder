using Models.State.PieceState;

namespace Models.State.BuildState
{
    public readonly struct BuildState
    {
        public Piece BuildingPiece { get; }
        public int Turns { get; }

        public BuildState(int turns = 0, Piece buildingPiece = default)
        {
            BuildingPiece = buildingPiece;
            Turns = turns;
        }

        public BuildState Decrement() => Turns == 0 ? this : new BuildState(Turns - 1, BuildingPiece);
    }
}