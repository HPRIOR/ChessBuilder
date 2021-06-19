using Models.State.PieceState;

namespace Models.State.Board
{
    public class Tile
    {
        private Tile(Position position, Piece currentPiece, BuildState.BuildState buildState = default)
        {
            Position = position;
            CurrentPiece = currentPiece;
            BuildState = buildState;
        }

        public Tile(Position position, BuildState.BuildState buildState = default)
        {
            Position = position;
            BuildState = buildState;
            CurrentPiece = new Piece(PieceType.NullPiece);
        }

        public Piece CurrentPiece { get; set; }
        public Position Position { get; }
        public BuildState.BuildState BuildState { get; set; }

        public Tile Clone() => new Tile(Position, CurrentPiece);

        public Tile CloneWithDecrementBuildState()
        {
            var noPieceBeingBuilt = BuildState.BuildingPiece == PieceType.NullPiece;
            if (noPieceBeingBuilt)
                return new Tile(Position, CurrentPiece);
            return new Tile(Position, CurrentPiece, BuildState.Decrement()); // decrement build
        }

        public override string ToString() =>
            $"Tile at ({Position.X}, {Position.Y}) containing" +
            $" {CurrentPiece}";
    }
}