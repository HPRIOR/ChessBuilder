using System;

namespace Networking
{
    public enum NetworkEvent
    {
        PlayerPrefabReady,
        ContextReady,
        GameReady
    }

    public class NetworkEvents
    {
        private event Action PlayerPrefabReadyEvent;
        private event Action ContextReadyEvent;
        private event Action GameReadyEvent;

        public void RegisterEventCallBack(NetworkEvent networkEvent, Action callback)
        {
            switch (networkEvent)
            {
                case NetworkEvent.PlayerPrefabReady:
                    PlayerPrefabReadyEvent += callback;
                    break;
                case NetworkEvent.ContextReady:
                    ContextReadyEvent += callback;
                    break;
                case NetworkEvent.GameReady:
                    GameReadyEvent += callback;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(networkEvent), networkEvent, null);
            }
        }

        public void InvokeEvent(NetworkEvent networkEvent)
        {
            switch (networkEvent)
            {
                case NetworkEvent.PlayerPrefabReady:
                    PlayerPrefabReadyEvent?.Invoke();
                    break;
                case NetworkEvent.ContextReady:
                    ContextReadyEvent?.Invoke();
                    break;
                case NetworkEvent.GameReady:
                    GameReadyEvent?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(networkEvent), networkEvent, null);
            }
        }
    }
}