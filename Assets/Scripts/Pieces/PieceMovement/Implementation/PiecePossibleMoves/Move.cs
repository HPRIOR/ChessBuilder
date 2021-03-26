using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Move
{
    private static IDictionary<Direction, IBoardPosition> directionMovement = new Dictionary<Direction, IBoardPosition>()
    {
        {Direction.N, new BoardPosition(0 , 1)},
        {Direction.E, new BoardPosition(1 , 0)},
        {Direction.S, new BoardPosition(0 , -1)},
        {Direction.W, new BoardPosition(-1 , 0)},
        {Direction.NE, new BoardPosition(1 , 1)},
        {Direction.SE, new BoardPosition(1 , -1)},
        {Direction.SW, new BoardPosition(-1 , -1)},
        {Direction.NW, new BoardPosition(-1 , 1)},
    };

    public static IBoardPosition In(Direction direction) => directionMovement[direction];

}
