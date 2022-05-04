using System;

namespace Networking
{
    public enum NetworkEvent
    {
        PlayerPrefabReady,
    }

    public class NetworkEvents
    {
        private event Action PlayerPrefabReadyEvent;

        public void RegisterEventCallBack(NetworkEvent networkEvent, Action callback)
        {
            switch (networkEvent)
            {
                case NetworkEvent.PlayerPrefabReady:
                    PlayerPrefabReadyEvent += callback;
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(networkEvent), networkEvent, null);
            }
        }
        

    }
}