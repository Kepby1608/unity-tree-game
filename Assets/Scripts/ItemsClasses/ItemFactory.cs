using System.Collections.Generic;
using System;
using UnityEngine;

public static class ItemFactory
{
    private static readonly Dictionary<ItemType, Sprite[]> _spritesByType = new Dictionary<ItemType, Sprite[]>();

    public static void InitializeSprites()
    {
        _spritesByType[ItemType.Wood] = new Sprite[7]
        {
            Resources.Load<Sprite>("Sprites/Wood_Level1"),
            Resources.Load<Sprite>("Sprites/Wood_Level2"),
            Resources.Load<Sprite>("Sprites/Wood_Level3"),
            Resources.Load<Sprite>("Sprites/Wood_Level4"),
            Resources.Load<Sprite>("Sprites/Wood_Level5"),
            Resources.Load<Sprite>("Sprites/Wood_Level6"),
            Resources.Load<Sprite>("Sprites/Wood_Level7")
        };
        _spritesByType[ItemType.Water] = new Sprite[5]
        {
            Resources.Load<Sprite>("Sprites/Water_Level1"),
            Resources.Load<Sprite>("Sprites/Water_Level2"),
            Resources.Load<Sprite>("Sprites/Water_Level3"),
            Resources.Load<Sprite>("Sprites/Water_Level4"),
            Resources.Load<Sprite>("Sprites/Water_Level5")
        };
    }

    public static Item CreateItem(int level, ItemType type)
    {
        if (!_spritesByType.ContainsKey(type))
            throw new ArgumentException($"Спрайты для типа <{type}> не существуют");

        var sprites = _spritesByType[type];

        return type switch
        {
            ItemType.Wood => new WoodItem(level, sprites),
            ItemType.Water => new WaterItem(level, sprites),
            _ => throw new ArgumentException($"Несуществующий тип: {type}")
        };
    }
}
