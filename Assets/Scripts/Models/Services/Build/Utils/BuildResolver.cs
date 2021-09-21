using System;
using System.Collections.Generic;
using System.Linq;
using Models.Services.Build.Interfaces;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using Models.Utils.ExtensionMethods.PieceTypeExt;

namespace Models.Services.Build.Utils
{
    public class BuildResolver : IBuildResolver
    {
        public IEnumerable<(Position, PieceType)> ResolveBuilds(BoardState boardState, PieceColour turn)
        {
            var activeBuildPositions = boardState.ActiveBuilds.ToArray();
            var resolvedBuilds = new List<(Position, PieceType)>();
            
            for (int i = 0; i < activeBuildPositions.Length; i++)
            {
                var pos = activeBuildPositions[i];
                ref var tile = ref boardState.GetTileAt(pos);
                var canBuild = tile.BuildTileState.Turns == 0 &&
                               tile.BuildTileState.BuildingPiece != PieceType.NullPiece &&
                               tile.CurrentPiece.Type == PieceType.NullPiece &&
                               tile.BuildTileState.BuildingPiece.Colour() == turn;
                if (canBuild)
                {
                    // Account for new active piece in active builds and pieces
                    boardState.ActiveBuilds.Remove(tile.Position);
                    boardState.ActivePieces.Add(tile.Position);

                    if (turn == PieceColour.Black)
                    {
                        boardState.ActiveBlackBuilds.Remove(tile.Position);
                        boardState.ActiveBlackPieces.Add(tile.Position);
                    }

                    if (turn == PieceColour.White)
                    {
                        boardState.ActiveWhiteBuilds.Remove(tile.Position);
                        boardState.ActiveWhitePieces.Add(tile.Position);
                    }

                    tile.CurrentPiece = new Piece(tile.BuildTileState.BuildingPiece);
                    tile.BuildTileState = new BuildTileState(); // reset build state

                    resolvedBuilds.Add((tile.Position, tile.CurrentPiece.Type));
                }
            }

            return resolvedBuilds;
        }
    }
}