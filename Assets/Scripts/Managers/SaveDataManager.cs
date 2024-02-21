using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class SaveDataManager : IManagers
{
    private ResourceManager resource;

    public string path;

    public PlayerData Player = new PlayerData();

    public bool isSaveFileExist;

    public bool Init()
    {
        isSaveFileExist = false;
        path = Application.persistentDataPath + "/save";
      
        return true;
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(Player, true);
        File.WriteAllText(path, data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path);
        Player = JsonUtility.FromJson<PlayerData>(data);
    }

    public void ClearData()
    {
        Player = new PlayerData();
    }

    public void DeleteData()
    {
        File.Delete(path);
        isSaveFileExist = false;
    }

    public void ApplyDataToCharacter(CharacterSavableData savedata)
    {
        Character character = new Character(Main.Get<DataManager>().Character[savedata.UnitName]);
        character.CurIndex = savedata.Index;
        character.CurPosX = savedata.CurPosX;
        character.CurPosY = savedata.CurPosY;
        for (int i = 0; i < savedata.itemnumbers.Length; i++)
        {
            character.itemnumbers[i] = savedata.itemnumbers[i];
        }
        for (int i = 0; i < savedata.Item.Length; i++)
        {
            if (savedata.Item[i].IsEquiped)
            {
                Item item = new Item();
                item.Init(Main.Get<DataManager>().Item[savedata.Item[i].ItemName]);
                item.ItemIndex = savedata.Item[i].ItemIndex;
                item.IsEquiped = savedata.Item[i].IsEquiped;
                character.Item[i] = item;
                item.EquipItem(character);
            }
        }
        Main.Get<GameManager>().playerUnits.Add(character);
    }

    public void ApplyDataToRoom(RoomSavableData savedata)
    {
        Room room = new Room(Main.Get<DataManager>().Room[savedata.RoomName]);
        room.IndexX = savedata.IndexX;
        room.IndexY = savedata.IndexY;
        room.IsEquiped = savedata.isEquiped;
        room.Pos = savedata.Pos;
        Main.Get<GameManager>().PlayerRooms.Add(room);
    }

    public void ApplyDataToItem(ItemSavableData savedata)
    {
        Item item = new Item();
        item.Init(Main.Get<DataManager>().Item[savedata.ItemName]);
        item.IsEquiped = savedata.IsEquiped;
        item.ItemIndex = savedata.ItemIndex;
        Main.Get<GameManager>().PlayerItems.Add(item);
    }

    public void ApplyDataToRoomDir(RoomDirSavableData savedata)
    {
        RoomBehavior room = new RoomBehavior();
        room.IndexX = savedata.IndexX;
        room.IndexY = savedata.IndexX;
        room.RoomDir = savedata.RoomDir;
    }


    public void GenerateSaveMap()
    {
        List<Room> playerRooms = Main.Get<GameManager>().PlayerRooms;
        List<Character> playerUnits = Main.Get<GameManager>().playerUnits;

        Main.Get<TileManager>().GenerateMap(Player.MapSizeX, Player.MapSizeY);
        LoadRoomDirSaveData();

        for (int i = 0; i < playerRooms.Count; i++)
        {
            if (playerRooms[i].IsEquiped)
            {
                int x = playerRooms[i].IndexX;
                int y = playerRooms[i].IndexY;
                RoomBehavior originRoom = Main.Get<TileManager>()._roomObjList[x][y];
                originRoom.RoomInfo.UnEquipedRoom();
                originRoom.OnDestroyRoom();
                ERoomDir roomDir = originRoom.RoomDir;
                Main.Get<TileManager>().DestroyRoom(x,y);
                

                GameObject obj = Main.Get<SceneManager>().Scene.CreateRoom(playerRooms[i].Data.PrefabName);
                obj.transform.position = playerRooms[i].Pos;
                obj.transform.parent = Main.Get<TileManager>().GridObject.transform;

                RoomBehavior room = obj.GetComponent<RoomBehavior>();
                room.SetData(playerRooms[i]);
                room.IndexX = x;
                room.IndexY = y;
                room.Pos = playerRooms[i].Pos;
                room.RoomDir = roomDir;
                room.RoomInfo.EquipedRoom();
               
                Main.Get<TileManager>().SetCheckWall(room);

                Main.Get<TileManager>()._roomObjList[room.IndexX][room.IndexY] = room;
            }
        }

        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (playerUnits[i].CurPosX != -1)
            {
                BatRoom room = (BatRoom)Main.Get<TileManager>()._roomObjList[playerUnits[i].CurPosX][playerUnits[i].CurPosY];
                room.CreateUnit(playerUnits[i]);
            }
        }
    }

    public void LoadMyData()
    {
        GameManager _gameManager = Main.Get<GameManager>();
        SoundManager _soundManager = Main.Get<SoundManager>();

        _gameManager.PlayerName = Player.Name;
        _gameManager.PlayerMoney = Player.PlayerMoney;
        _gameManager.CurStage = Player.Curstage;
        _gameManager.PlayerHP.CurValue = Player.PlayerHP;
        _soundManager.BGMValue = Player.BGMValue;
        _soundManager.EffectValue = Player.EffectValue;
        _soundManager.UIValue = Player.UIValue;
        Main.Get<SoundManager>().SetVolume(ESoundType.BGM, _soundManager.BGMValue);
        Main.Get<SoundManager>().SetVolume(ESoundType.Effect, _soundManager.EffectValue);
        Main.Get<SoundManager>().SetVolume(ESoundType.UI, _soundManager.UIValue);

        if (Player.PlayerUnitsSaveData.Count > 0)
        {
            LoadCharacterSaveData();
        }
        if (Player.PlayerRoomsSaveData.Count > 0)
        {
            LoadRoomSaveData();
        }
        if (Player.PlayerItemsSaveData.Count > 0)
        {
            LoadItemSaveData();
        }
        
    }
    private void LoadCharacterSaveData()
    {
        List<CharacterSavableData> playerUnitsSaveData = Player.PlayerUnitsSaveData;

        for (int i = 0; i < playerUnitsSaveData.Count; i++)
        {
            ApplyDataToCharacter(playerUnitsSaveData[i]);
        }
    }

    private void LoadRoomSaveData()
    {
        List<RoomSavableData> playerRoomsSaveData = Player.PlayerRoomsSaveData;
        for (int i = 0; i < playerRoomsSaveData.Count; i++)
        {
            ApplyDataToRoom(playerRoomsSaveData[i]);
        }
    }

    private void LoadItemSaveData()
    {
        List<ItemSavableData> playerItemsSaveData = Player.PlayerItemsSaveData;
        for (int i = 0; i < playerItemsSaveData.Count; i++)
        {
            ApplyDataToItem(playerItemsSaveData[i]);
        }
    }

    private void LoadRoomDirSaveData()
    {
        List<RoomDirSavableData> roomDirSaveData = Player.PlayerRoomsDirSaveData;
        for (int i = 0; i < roomDirSaveData.Count; i++)
        {
            Main.Get<TileManager>().SetRoomDir(roomDirSaveData[i].IndexX, roomDirSaveData[i].IndexY, roomDirSaveData[i].RoomDir);
        }
        
    }
}
