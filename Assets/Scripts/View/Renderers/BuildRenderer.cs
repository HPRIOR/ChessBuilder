using Models.State.Board;
using UnityEngine;
using View.Interfaces;

namespace View.Renderers
{
    public class BuildRenderer : IBoardStateChangeRenderer
    {
        public void Render(BoardState previousState, BoardState newState)
        {
            Debug.Log("helloWorld");
        }
    }
}