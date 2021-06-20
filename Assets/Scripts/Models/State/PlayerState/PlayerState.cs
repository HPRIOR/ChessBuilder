using Models.State.Interfaces;

namespace Models.State.PlayerState
{
    public readonly struct PlayerState : IPlayerState
    {
        public PlayerState(int buildPoints)
        {
            BuildPoints = buildPoints;
        }

        public int BuildPoints { get; }
    }
}