using Models.State.PieceState;

namespace Models.Services.AI.Implementations
{
    public readonly struct BoardScore
    {
        public BoardScore(int blackPoints, int whitePoints)
        {
            _blackPoints = blackPoints;
            _whitePoints = whitePoints;
        }

        private readonly int _blackPoints;
        private readonly int _whitePoints;

        public int GetPoints(PieceColour turn) =>
            turn == PieceColour.Black ? _blackPoints : _whitePoints;
    }
}