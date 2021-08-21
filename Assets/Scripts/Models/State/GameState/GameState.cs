using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;

namespace Models.State.GameState
{
    public class GameState : ICloneable
    {
        public GameState(bool check, bool checkMate, PlayerState.PlayerState blackState,
            PlayerState.PlayerState whiteState,
            ImmutableDictionary<Position, ImmutableHashSet<Position>> possiblePieceMoves,
            BuildMoves possibleBuildMoves, BoardState boardState)
        {
            Check = check;
            CheckMate = checkMate;
            BlackState = blackState;
            WhiteState = whiteState;
            PossiblePieceMoves = possiblePieceMoves;
            PossibleBuildMoves = possibleBuildMoves;
            BoardState = boardState;
        }

        public BoardState BoardState { get; set; }
        public bool Check { get; set; }
        public bool CheckMate { get; set; }
        public PlayerState.PlayerState BlackState { get; set; }
        public PlayerState.PlayerState WhiteState { get; set; }
        public ImmutableDictionary<Position, ImmutableHashSet<Position>> PossiblePieceMoves { get; set; }
        public BuildMoves PossibleBuildMoves { get; set; }

        public object Clone()
        {
            var possibleMoves = new Dictionary<Position, ImmutableHashSet<Position>>(PossiblePieceMoves)
                .ToImmutableDictionary();
            var builds = new HashSet<PieceType>(PossibleBuildMoves.BuildPieces).ToImmutableHashSet();
            var buildPosition = new HashSet<Position>(PossibleBuildMoves.BuildPositions).ToImmutableHashSet();
            var buildMoves = new BuildMoves(buildPosition, builds);
            return new GameState(Check, CheckMate, BlackState, WhiteState, possibleMoves, buildMoves,
                BoardState.Clone());
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Black state: \n");
            stringBuilder.Append($"     Build Points: {BlackState.BuildPoints} \n");
            stringBuilder.Append("White state: \n");
            stringBuilder.Append($"     Build Points: {WhiteState.BuildPoints} \n");
            stringBuilder.Append("Possible Moves: \n");
            PossiblePieceMoves.Keys.ToList().ForEach(piecePosition =>
            {
                stringBuilder.Append(
                    $"     {BoardState.Board[piecePosition.X, piecePosition.Y].CurrentPiece.Type}: \n       ");
                PossiblePieceMoves[piecePosition].ToList().ForEach(move => stringBuilder.Append($"({move}), "));
            });
            stringBuilder.Append("\n");
            stringBuilder.Append("Possible Build Pieces: \n     ");
            PossibleBuildMoves.BuildPieces.ToList().ForEach(piece => stringBuilder.Append($"{piece}, "));
            stringBuilder.Append("\n");
            stringBuilder.Append("Possible Build Positions: \n     ");
            PossibleBuildMoves.BuildPositions.OrderBy(p => p.Y).ToList()
                .ForEach(position => stringBuilder.Append($"({position}), "));
            return stringBuilder.ToString();
        }
    }
}