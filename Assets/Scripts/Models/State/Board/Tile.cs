using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.State.Board
{
    public class Tile
    {
        private Tile(Position position, Piece currentPiece, BuildTileState buildTileState = default)
        {
            Position = position;
            CurrentPiece = currentPiece;
            BuildTileState = buildTileState;
        }

        public Tile(Position position, BuildTileState buildTileState = default)
        {
            Position = position;
            BuildTileState = buildTileState;
            CurrentPiece = new Piece(PieceType.NullPiece);
        }

        public Piece CurrentPiece { get; set; }
        public Position Position { get; }
        public BuildTileState BuildTileState { get; set; }

        public Tile Clone() => new Tile(Position, CurrentPiece);

        public Tile CloneWithDecrementBuildState()
        {
            var noPieceBeingBuilt = BuildTileState.BuildingPiece == PieceType.NullPiece;
            if (noPieceBeingBuilt)
                return new Tile(Position, CurrentPiece);
            return new Tile(Position, CurrentPiece, BuildTileState.Decrement()); // decrement build
        }

        public override string ToString() =>
            $"Tile at ({Position.X}, {Position.Y}) containing" +
            $" {CurrentPiece}";
    }
}