namespace Models.State.PlayerState
{
    public readonly struct PlayerState
    {
        public PlayerState(int buildPoints)
        {
            BuildPoints = buildPoints;
        }

        public readonly int BuildPoints;
    }
}