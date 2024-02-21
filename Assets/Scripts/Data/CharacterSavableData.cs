using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class CharacterSavableData
{
    public int CurPosX;
    public int CurPosY;
    public int Index;
    public bool isEquiped;
    public string UnitName;
    public int[] itemnumbers = new int[3];
    public ItemSavableData[] Item = new ItemSavableData[3];
    public CharacterSavableData(Character character)
    {
        UnitName = character.Data.Key;
        CurPosX = character.CurPosX;
        CurPosY = character.CurPosY;
        Index = character.CurIndex;
        isEquiped = character.isEquiped;
        for (int i = 0; i < itemnumbers.Length; i++)
        {
            itemnumbers[i] = character.itemnumbers[i];
        }
        for (int i = 0; i < character.Item.Length; i++)
        {
            if (character.Item[i] != null)
            {
                Item[i] = character.Item[i].CreateSavableItemData();
            }
        }
    }
}