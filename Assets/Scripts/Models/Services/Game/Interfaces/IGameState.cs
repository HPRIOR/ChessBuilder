using System.Collections.Generic;
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
        BoardState CurrentBoardState { get; }
        public PlayerState BlackState { get; }
        public PlayerState WhiteState { get; }
        public BuildMoves PossibleBuildMoves { get; }
        IDictionary<Position, HashSet<Position>> PossiblePieceMoves { get; }
        void UpdateBoardState(BoardState newState);
        void RetainBoardState();
    }
}