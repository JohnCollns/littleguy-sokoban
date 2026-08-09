using Godot;
using System;

public static class MathsHelper
{
    public static Vector2I Vector2IFromVector2(Vector2 vec)
    {
        return new Vector2I((int)vec.X, (int)vec.Y);
    }
}
