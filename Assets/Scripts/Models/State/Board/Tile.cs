using Models.State.BuildState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.State.Board
{
    /*
     * The desired logic is for builds to take place where possible at the end of a players
     * turn. This means that even if a build state is at 0 at the start of a players turn
     * (e.g. it was at 1 and then was decremented after the enemies move).
     * This means that the logic with a WithDecrementedBuildState has to account for the turn when
     * calculating a possible build.
     * WithPiece will always be called at the end of a players turn: if either player has a build move
     * of 0, it can be built at this stage
     */
    public struct Tile
    {
        public BuildTileState BuildTileState;
        public PieceType CurrentPiece;
        public Position Position;

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

        public Tile WithPiece(PieceType newPiece)
        {
            var noPieceBeingBuilt = BuildTileState.BuildingPiece == PieceType.NullPiece;
            if (noPieceBeingBuilt)
                return new Tile(Position, newPiece);

            var newBuildState = BuildTileState.Decrement();
            var canBuild =
                newBuildState.Turns == 0 &&
                newBuildState.BuildingPiece != PieceType.NullPiece &&
                newPiece == PieceType.NullPiece; 
                // no need to check for player turn here

            if (canBuild)
                return new Tile(Position, newBuildState.BuildingPiece);

            return new Tile(Position, newPiece, BuildTileState.Decrement());
        }

        public Tile WithDecrementedBuildState(PieceColour turn)
        {
            var noPieceBeingBuilt = BuildTileState.BuildingPiece == PieceType.NullPiece;
            if (noPieceBeingBuilt)
                return new Tile(Position, CurrentPiece);

            var newBuildState = BuildTileState.Decrement();
            var canBuild =
                newBuildState.Turns == 0 &&
                newBuildState.BuildingPiece != PieceType.NullPiece &&
                CurrentPiece == PieceType.NullPiece &&
                turn.NextTurn() == newBuildState.BuildingPiece.Colour(); // ensure builds on end of turn

            if (canBuild)
                return new Tile(Position, newBuildState.BuildingPiece);
            return new Tile(Position, CurrentPiece, newBuildState); // decrement build           
        }

        public Tile WithBuild(PieceType newPiece)
        {
            return new Tile(Position, CurrentPiece, new BuildTileState(newPiece));
        }

        public override string ToString() =>
            $"Tile at ({Position.X}, {Position.Y}) containing" +
            $" {CurrentPiece.ToString()} and building {BuildTileState.BuildingPiece}";
    }
}