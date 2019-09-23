using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class Utility
{
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
}

[Serializable]
public class Sight
{
    public float Radius = 1f;
    /// <summary>
    /// Sight Angle
    /// </summary>
    public float Angle = 60f;
    /// <summary>
    /// Start Angle Sweeps Right
    /// </summary>
    public float AngleOffset = 0f;
}

public enum MonsterDirection
{
    Down = 3,
    Up = 1,
    Left = 2,
    Right = 0,
}

public interface ICapturable
{
    bool Capturable { get; set; }
    bool OnHit(float chance);
    void OnCapture();
}

public interface IUseable
{
    void OnUse(PlayerController pc);
}

public class Item
{
    public Sprite InBagSprite;
    public IUseable Useable;

    public Item(Sprite inBagSprite, IUseable useable)
    {
        InBagSprite = inBagSprite;
        Useable = useable;
    }
}

public abstract class Impact { }

public class KnockOff : Impact
{
    public Vector3 Direction;
    public float Force;
    public float Duration;

    public KnockOff(Vector3 direction, float force, float duration)
    {
        Direction = direction;
        Force = force;
        Duration = duration;
    }
}