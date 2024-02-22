using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : IManagers
{
    private TileManager _tile;
    public int PlayerMoney { get; set; }
    public Vital PlayerHP { get; set; }

    public bool isHomeSet = false;
    public bool isPlayerDead = false;
    public RoomBehavior HomeRoom { get; set; }
    public string PlayerName { get; set; }

    public int CurStage { get; set; }

    public bool isTutorial { get; set; }

    public bool isPlacingTutorialClear = false; // 배치모드 튜토리얼 클리어했는지 체크.

    public int tutorialIndexX = 1;

    public int tutorialIndexY = 1;


    public List<Character> PlayerUnits { get; private set; }   // 플레이어가 보유한 유닛 리스트
    public List<Room> PlayerRooms { get; private set; }    // 플레이어가 보유한 Room 리스트
    public List<Item> PlayerItems { get; private set; }     // 플레이어가 보유한 아이템 리스트
    public event Action<int> OnChangeMoney;

    public bool Init()
    {
        _tile = Main.Get<TileManager>();
        PlayerUnits = new List<Character>();
        PlayerRooms = new List<Room>();
        PlayerItems = new List<Item>();
        CurStage = 0;
        PlayerHP = new Vital(EstatType.Hp, 5);
        PlayerHP.OnValueZero += GameOver;

        bool tutorial = PlayerPrefs.HasKey("Tutorial");
        if (!tutorial)
        {
            isTutorial = true;
            PlayerMoney = 9000;
        }
        else
        {
            isTutorial = PlayerPrefs.GetInt("Tutorial") == 1 ? false : true;
            PlayerMoney = isTutorial ? 9000 : 3000;
            if (!isTutorial)
            {
                Character newChar = new Character(Main.Get<DataManager>().Character["Warriorr"]);
                PlayerUnits.Add(newChar);

                Room newRoom = new Room(Main.Get<DataManager>().Room["Forest +1"]);
                PlayerRooms.Add(newRoom);
            }
        }

        //Room test = new Room(Main.Get<DataManager>().Room["Temple"]);
        //PlayerRooms.Add(test);

        return true;
    }

    public void ChangeMoney(int amount)
    {
        PlayerMoney += amount;
        OnChangeMoney?.Invoke(PlayerMoney);
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        isPlayerDead = true;
        StageFail_PopupUI ui = Main.Get<UIManager>().OpenPopup<StageFail_PopupUI>("StageFail_PopupUI");
        ui._curStage = CurStage;

    }

    public void RemoveUnit(Character unit) // unit
    {
        if (unit.CurRoom != null) // 유닛이 배치되어있는 경우에
        {
            ((BatRoom)unit.CurRoom).DeleteUnit(unit); // 배치되어있는 유닛 빼면서
        }
        PlayerUnits.Remove(unit); // 인벤토리에서도 지우고 
        Item[] items = unit.Item; // 아이템 장착되어있는 것도 빼주고
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                items[i].IsEquiped = false;
                items[i].Owner = null;
            }
        }
    }

    public void RemoveRoom(Room room) // room
    {
        if (room.IsEquiped) // 배치되어있다면 room 해제
        {
            room.IsEquiped = false;
            _tile.DeleteRoom(room);
        }
        PlayerRooms.Remove(room); // 인벤토리에서 지우고
    }

    public void SaveData()
    {
        SaveDataManager saveDataManager = Main.Get<SaveDataManager>();
        saveDataManager.ClearData();
        saveDataManager.Player.Name = PlayerName;
        saveDataManager.Player.Curstage = CurStage;
        saveDataManager.Player.PlayerMoney = PlayerMoney;
        saveDataManager.Player.PlayerHP = PlayerHP.CurValue;
        saveDataManager.Player.BGMValue = Main.Get<SoundManager>().BGMValue;
        saveDataManager.Player.EffectValue = Main.Get<SoundManager>().EffectValue;
        saveDataManager.Player.UIValue = Main.Get<SoundManager>().UIValue;
        Main.Get<TileManager>().GetMapSize(out saveDataManager.Player.MapSizeX, out saveDataManager.Player.MapSizeY);
        for (int i = 0; i < PlayerUnits.Count; i++)
        {
            saveDataManager.Player.PlayerUnitsSaveData.Add(PlayerUnits[i].CreateSavableUnitData());
        }
        for (int i = 0; i < PlayerRooms.Count; i++)
        {
            saveDataManager.Player.PlayerRoomsSaveData.Add(PlayerRooms[i].CreateSavableRoomData());
        }
        for (int i = 0; i < PlayerItems.Count; i++)
        {
            saveDataManager.Player.PlayerItemsSaveData.Add(PlayerItems[i].CreateSavableItemData());
        }
        for (int i = 0; i < saveDataManager.Player.MapSizeX; i++)
        {
            for (int j = 0; j < saveDataManager.Player.MapSizeY; j++)
            {
                saveDataManager.Player.PlayerRoomsDirSaveData.Add(Main.Get<TileManager>()._roomObjList[i][j].CreateRoomDirSavableData());
            }
        }
        saveDataManager.SaveData();
    }

    public void SetHomeRoom(RoomBehavior home)
    {
        if (home != null)
        {
            isHomeSet = true;
            HomeRoom = home;
        }
        else
        {
            isHomeSet = false;
            HomeRoom = null;
        }
    }

    public void AddHometoInventory()
    {
        Room room = new Room();
        room.Init(Main.Get<DataManager>().Room["Home"]);
        PlayerRooms.Add(room);
    }

    public void ExitGame()
    {
        if (!isTutorial)
        {
            SaveData();
        }
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); 
#endif
    }
}