namespace Models.Services.AI
{
    public readonly struct BoardScore
    {
        public BoardScore(int blackPoints, int whitePoints)
        {
            BlackPoints = blackPoints;
            WhitePoints = whitePoints;
        }
        private int BlackPoints { get; }
        private int WhitePoints { get; }
    }
}