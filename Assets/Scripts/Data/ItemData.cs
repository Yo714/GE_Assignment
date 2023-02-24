using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        IT_Gun,
        IT_Grenade,
        IT_Attachment,
        IT_Expendible,
    }

    public ItemType Type;
    public GameObject ItemEquipmentPrefab;
    public string ItemName;
    public uint ItemID;
    public Sprite ItemSprite;
}