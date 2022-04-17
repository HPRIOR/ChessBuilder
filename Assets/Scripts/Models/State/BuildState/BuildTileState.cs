using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.State.BuildState
{
    public readonly struct BuildTileState
    {
        public PieceType BuildingPiece { get; }
        public int Turns { get; }

        // used to instantiate default/null build state, and to decrement build states
        public BuildTileState(int turns = 0, PieceType buildingPiece = default)
        {
            BuildingPiece = buildingPiece;
            Turns = turns;
        }

        // used to instantiate initial value of BuildState
        public BuildTileState(PieceType pieceType)
        {
            BuildingPiece = pieceType;
            Turns = pieceType.Value();
        }

        public BuildTileState Decrement() => Turns == 0 ? this : new BuildTileState(Turns - 1, BuildingPiece);
    }
}