using UnityEngine;

public class WaterItem : Item
{
    private readonly Sprite[] _levelSprites;

    public override int MaxLevel => 5;

    public override ItemType Type => ItemType.Water;

    public WaterItem(int level, Sprite[] levelSprites) : base(level)
    {
        _levelSprites = levelSprites;
    }

    public override Item TryMerge(Item otherItem)
    {
        if (otherItem is WaterItem && otherItem.Level == this.Level && this.Level < MaxLevel)
        {
            return new WaterItem(this.Level + 1, _levelSprites);
        }
        return null;
    }

    public override Sprite GetSprite()
    {
        if (_levelSprites != null && Level - 1 < _levelSprites.Length)
            return _levelSprites[Level - 1];
        return null;
    }
}

