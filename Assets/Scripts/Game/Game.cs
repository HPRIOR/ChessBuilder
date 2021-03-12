using System;
using Zenject;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Game : MonoBehaviour
{
    private IBoardState _boardState;

    [Inject]
    public void Construct( IBoardState boardState)
    {
        _boardState = boardState;
    }

    public void Start()
    {
        
    }
}
