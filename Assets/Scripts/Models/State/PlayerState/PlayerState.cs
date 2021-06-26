namespace Models.State.PlayerState
{
    public readonly struct PlayerState
    {
        public PlayerState(int buildPoints)
        {
            BuildPoints = buildPoints;
        }

        public int BuildPoints { get; }
    }
}