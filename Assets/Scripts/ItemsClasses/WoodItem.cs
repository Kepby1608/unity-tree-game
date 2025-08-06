using UnityEngine;

public class WoodItem : Item
{
    private readonly Sprite[] _levelSprites;

    public override int MaxLevel => 7;

    public override ItemType Type => ItemType.Wood;

    public WoodItem(int level, Sprite[] levelSprites) : base(level)
    {
        _levelSprites = levelSprites;
    }

    public override Item TryMerge(Item otherItem)
    {
        if (otherItem is WoodItem && otherItem.Level == this.Level && this.Level < MaxLevel)
        {
            return new WoodItem(this.Level + 1, _levelSprites);
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
