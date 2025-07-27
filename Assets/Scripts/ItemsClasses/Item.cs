using System;
using UnityEngine;

public abstract class Item
{
    public abstract int MaxLevel { get; }
    public int Level { get; protected set; }
    public abstract ItemType Type { get; }

    protected Item(int level)
    {
        Level = Math.Min(level, MaxLevel);
    }

    public abstract Item TryMerge(Item other);

    public abstract Sprite GetSprite();
}
