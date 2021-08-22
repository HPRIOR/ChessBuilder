using System.Collections.Generic;
using System.Linq;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceType;

namespace Models.Services.Build.Utils
{
    public class BuildResolver : IBuildResolver
    {
        public IEnumerable<(Position, PieceType)> ResolveBuilds(BoardState boardState, PieceColour turn)
        {
            var activeBuildPositions =
                new List<Tile>(
                    boardState.ActiveBuilds.Select(position =>
                        boardState.Board[position.X, position.Y])); //TODO remove linq

            var resolvedBuilds = new List<(Position, PieceType)>();
            foreach (var tile in activeBuildPositions)
            {
                var canBuild = tile.BuildTileState.Turns == 0 &&
                               tile.BuildTileState.BuildingPiece != PieceType.NullPiece &&
                               tile.CurrentPiece.Type == PieceType.NullPiece &&
                               tile.BuildTileState.BuildingPiece.Colour() == turn;
                if (canBuild)
                {
                    // Account for new active piece in active builds and pieces
                    boardState.ActiveBuilds.Remove(tile.Position);
                    boardState.ActivePieces.Add(tile.Position);

                    tile.CurrentPiece = new Piece(tile.BuildTileState.BuildingPiece);
                    tile.BuildTileState = new BuildTileState(); // reset build state

                    resolvedBuilds.Add((tile.Position, tile.CurrentPiece.Type));
                }
            }

            return resolvedBuilds;
        }
    }
}