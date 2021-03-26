﻿using UnityEngine;

public struct BoardPosition : IBoardPosition
{
    public int X { get; }
    public int Y { get; }
    public Vector2 Vector { get; }

    public BoardPosition(int x, int y)
    {
        X = x;
        Y = y;
        Vector = new Vector2(x + 0.5f, y + 0.5f);
    }

    // todo
    public BoardPosition(string algebraicNotation) => throw new System.NotImplementedException();

    // TODO
    public string GetAlgabraicNotation() => X.ToString() + Y.ToString();

    public override string ToString()
    {
        return $"Coordinates: {X}, {Y} \n" +
            $"Vector: {Vector} \n" +
            $"Algabraic Notation: {GetAlgabraicNotation()}";
    }

    public IBoardPosition Add(IBoardPosition boardPosition) => new BoardPosition(X + boardPosition.X, Y + boardPosition.Y);
}