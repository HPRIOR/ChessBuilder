using Models.State.Board;
using UnityEngine;

namespace View.Interfaces
{
    public interface IStateChangeWithPrefabRenderer
    {
        void Render(BoardState previousState, BoardState newState, GameObject prefab);
    }
}