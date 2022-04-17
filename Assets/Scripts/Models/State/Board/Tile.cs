using Models.State.BuildState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.State.Board
{
    public readonly struct Tile
    {
        public readonly BuildTileState BuildTileState;
        public readonly PieceType CurrentPiece;
        public readonly Position Position;

        public Tile(Position position, PieceType currentPiece, BuildTileState buildTileState = default)
        {
            Position = position;
            CurrentPiece = currentPiece;
            BuildTileState = buildTileState;
        }

        public Tile(Position position, BuildTileState buildTileState = default)
        {
            Position = position;
            BuildTileState = buildTileState;
            CurrentPiece = PieceType.NullPiece;
        }

        public Tile Clone() => new Tile(Position, CurrentPiece, BuildTileState);

        private bool CanBuild(BuildTileState buildState, PieceType piece, PieceColour turn) =>
            buildState.Turns == 0 &&
            buildState.BuildingPiece != PieceType.NullPiece &&
            piece == PieceType.NullPiece &&
            turn.NextTurn() == buildState.BuildingPiece.Colour(); // ensure builds on end of turn


        public Tile WithPiece(PieceType newPiece, PieceColour turn)
        {
            var noPieceBeingBuilt = BuildTileState.BuildingPiece == PieceType.NullPiece;
            if (noPieceBeingBuilt)
                return new Tile(Position, newPiece);

            var newBuildState = BuildTileState.Decrement();
            var canBuild = CanBuild(newBuildState, newPiece, turn);
            if (canBuild)
                return new Tile(Position, newBuildState.BuildingPiece);
            return new Tile(Position, newPiece, newBuildState); // decrement build
        }

        public Tile WithDecrementedBuildState(PieceColour turn)
        {
            var noPieceBeingBuilt = BuildTileState.BuildingPiece == PieceType.NullPiece;
            if (noPieceBeingBuilt)
                return new Tile(Position, CurrentPiece);

            var newBuildState = BuildTileState.Decrement();
            var canBuild = CanBuild(newBuildState, CurrentPiece, turn);
            if (canBuild)
                return new Tile(Position, newBuildState.BuildingPiece);
            return new Tile(Position, CurrentPiece, newBuildState); // decrement build           
        }

        public Tile WithBuild(PieceType newPiece) => new Tile(Position, CurrentPiece, new BuildTileState(newPiece));

        public override string ToString() =>
            $"Tile at ({Position.X}, {Position.Y}) containing" +
            $" {CurrentPiece.ToString()} and building {BuildTileState.BuildingPiece}";
    }
}