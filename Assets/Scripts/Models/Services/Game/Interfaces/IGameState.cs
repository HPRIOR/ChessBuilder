using System.Collections.Immutable;
using Models.State.Board;
using Models.State.BuildState;
using Models.State.PieceState;
using Models.State.PlayerState;

namespace Models.Services.Game.Interfaces
{
    public interface IGameState
    {
        PieceColour Turn { get; }
        bool Check { get; }
        bool CheckMate { get; }
        BoardState CurrentBoardState { get; }
        public PlayerState BlackState { get; }
        public PlayerState WhiteState { get; }
        public BuildMoves PossibleBuildMoves { get; }
        ImmutableDictionary<Position, ImmutableHashSet<Position>> PossiblePieceMoves { get; }
        void UpdateBoardState(BoardState newState);
        void RetainBoardState();
    }
}