namespace Networking
{
    public class PlayerFactory
    {
        private readonly Player.Factory _playerPlayerFactory;

        public PlayerFactory(Player.Factory playerFactory)
        {
            _playerPlayerFactory = playerFactory;
        }

        public Player Create() => _playerPlayerFactory.Create();
    }
}