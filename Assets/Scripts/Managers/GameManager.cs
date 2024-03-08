using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : IManagers
{
    private TileManager _tile;
    public int PlayerMoney { get; set; }
    public float PlayerMaxHp { get; set; }
    public Vital PlayerHP { get; set; }

    public bool isHomeSet = false;
    public bool isPlayerDead = false;
    public RoomBehavior HomeRoom { get; set; }
    public string PlayerName { get; set; }
    public int CurStage { get; set; }
    public int SetWallCount { get; set; }
   
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
        SetWallCount = 0;
        SetPlayerHp();
        PlayerMoney = 0;
        isHomeSet = false;
        return true;
    }

    public void ChangeMoney(int amount)
    {
        PlayerMoney += amount;
        OnChangeMoney?.Invoke(PlayerMoney);
        if(PlayerMoney < 0) PlayerMoney = 0;
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        isPlayerDead = true;
        StageFail_PopupUI ui = Main.Get<UIManager>().OpenPopup<StageFail_PopupUI>("StageFail_PopupUI");
        ui._curStage = CurStage;
        if(CurStage > 0)
        {
            ui.UpgradePoint += 1;
        }
        else if(CurStage > 30)
        {
            ui.UpgradePoint += 2;
        }
        else if(CurStage > 50)
        {
            ui.UpgradePoint += 3;
        }
    }

    public void RemoveUnit(Character unit) // unit
    {
        if (unit.CurRoom != null) // 유닛이 배치되어있는 경우에
        {
            ((BatRoom)unit.CurRoom).DeleteUnit(unit); // 배치되어있는 유닛 빼면서
        }
        Item[] items = unit.Item; // 아이템 장착되어있는 것도 빼주고
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                items[i].UnEquipItem(unit);
            }
        }
        PlayerUnits.Remove(unit); // 인벤토리에서도 지우고 
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
        saveDataManager.Player.SetWallCount = SetWallCount;
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

        PlayerPrefs.SetInt("SortOption", (int)PlayerSetting.SortSetting);
        PlayerPrefs.Save();

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

    public void AddUnit(Character unit)
    {
        PlayerUnits.Add(unit);

        switch (PlayerSetting.SortSetting)
        {
            case SortOption.levelAscend:
                SortUnitLevel(true);
                break;
            case SortOption.levelDescend:
                SortUnitLevel(false);
                break;
            case SortOption.NameAscend:
                SortUnitName(true);
                break;
            case SortOption.NameDescend:
                SortUnitName(false);
                break;
        }
    }

    public void AddRoom(Room room)
    {
        PlayerRooms.Add(room);

        switch (PlayerSetting.SortSetting)
        {
            case SortOption.levelAscend:
                SortRoomLevel(true);
                break;
            case SortOption.levelDescend:
                SortRoomLevel(false);
                break;
            case SortOption.NameAscend:
                SortRoomName(true);
                break;
            case SortOption.NameDescend:
                SortRoomName(false);
                break;
        }
    }

    // true 이면 오름차순
    public void SortUnitName(bool IsAscend)
    {
        if (IsAscend)
        {
            PlayerUnits.Sort((scr, dest) =>
            {
                int comparison = string.Compare(dest.Data.PrefabName, scr.Data.PrefabName);
                if (comparison != 0)
                {
                    return comparison;
                }
                else
                {
                    int scrLevel = int.Parse(scr.Data.Key[scr.Data.Key.Length - 1].ToString());
                    int destLevel = int.Parse(dest.Data.Key[dest.Data.Key.Length - 1].ToString());

                    return destLevel.CompareTo(scrLevel);
                }
            });
            PlayerSetting.SortSetting = SortOption.NameAscend;
        }
        else
        {
            PlayerUnits.Sort((scr, dest) =>
            {
                int comparison = string.Compare(scr.Data.PrefabName, dest.Data.PrefabName);
                if (comparison != 0)
                {
                    return comparison;
                }
                else
                {
                    int scrLevel = int.Parse(scr.Data.Key[scr.Data.Key.Length - 1].ToString());
                    int destLevel = int.Parse(dest.Data.Key[dest.Data.Key.Length - 1].ToString());

                    return destLevel.CompareTo(scrLevel);
                }
            });
            PlayerSetting.SortSetting = SortOption.NameDescend;
        }
    }

    public void SortUnitLevel(bool IsAscend)
    {
        if (IsAscend)
        {
            PlayerUnits.Sort((scr, dest) =>
            {
                int scrLevel = int.Parse(scr.Data.Key[scr.Data.Key.Length - 1].ToString());
                int destLevel = int.Parse(dest.Data.Key[dest.Data.Key.Length - 1].ToString());

                if (scrLevel != destLevel)
                {
                    return scrLevel.CompareTo(destLevel);
                }
                else
                {
                    return scr.Data.Key.CompareTo(dest.Data.Key);
                }
            });
            PlayerSetting.SortSetting = SortOption.levelAscend;
        }
        else
        {
            PlayerUnits.Sort((scr, dest) =>
            {
                int scrLevel = int.Parse(scr.Data.Key[scr.Data.Key.Length - 1].ToString());
                int destLevel = int.Parse(dest.Data.Key[dest.Data.Key.Length - 1].ToString());

                if (scrLevel != destLevel)
                {
                    return destLevel.CompareTo(scrLevel);
                }
                else
                {
                    return scr.Data.Key.CompareTo(dest.Data.Key);
                }
            });
            PlayerSetting.SortSetting = SortOption.levelDescend;
        }
    }

    public void SortRoomName(bool IsAscend)
    {
        if (IsAscend)
        {
            PlayerRooms.Sort((scr, dest) =>
            {
                int comparison = string.Compare(dest.Data.PrefabName, scr.Data.PrefabName);
                if (comparison != 0)
                {
                    return comparison;
                }
                else
                {
                    int scrLevel = int.Parse(scr.Data.Key[scr.Data.Key.Length - 1].ToString());
                    int destLevel = int.Parse(dest.Data.Key[dest.Data.Key.Length - 1].ToString());

                    return destLevel.CompareTo(scrLevel);
                }
            });
            PlayerSetting.SortSetting = SortOption.NameAscend;
        }
        else
        {
            PlayerRooms.Sort((scr, dest) =>
            {
                int comparison = string.Compare(scr.Data.PrefabName, dest.Data.PrefabName);
                if (comparison != 0)
                {
                    return comparison;
                }
                else
                {
                    int scrLevel = int.Parse(scr.Data.Key[scr.Data.Key.Length - 1].ToString());
                    int destLevel = int.Parse(dest.Data.Key[dest.Data.Key.Length - 1].ToString());

                    return destLevel.CompareTo(scrLevel);
                }
            });
            PlayerSetting.SortSetting = SortOption.NameDescend;
        }
    }

    public void SortRoomLevel(bool IsAscend)
    {
        if (IsAscend)
        {
            PlayerRooms.Sort((scr, dest) =>
            {
                int scrLevel = int.Parse(scr.Data.Key[scr.Data.Key.Length - 1].ToString());
                int destLevel = int.Parse(dest.Data.Key[dest.Data.Key.Length - 1].ToString());

                if (scrLevel != destLevel)
                {
                    return scrLevel.CompareTo(destLevel);
                }
                else
                {
                    return scr.Data.Key.CompareTo(dest.Data.Key);
                }
            });
            PlayerSetting.SortSetting = SortOption.levelAscend;
        }
        else
        {
            PlayerRooms.Sort((scr, dest) =>
            {
                int scrLevel = int.Parse(scr.Data.Key[scr.Data.Key.Length - 1].ToString());
                int destLevel = int.Parse(dest.Data.Key[dest.Data.Key.Length - 1].ToString());

                if (scrLevel != destLevel)
                {
                    return destLevel.CompareTo(scrLevel);
                }
                else
                {
                    return scr.Data.Key.CompareTo(dest.Data.Key);
                }
            });
            PlayerSetting.SortSetting = SortOption.levelDescend;
        }
    }

    public void AddHometoInventory()
    {
        Room room = new Room();
        room.Init(Main.Get<DataManager>().Room["Home1"]);
        PlayerRooms.Add(room);
    }

    public void AddItemToInventory()
    {
        Item item = Main.Get<DataManager>().ItemCDO["TrainingEgg"].Clone();
        item.Init(Main.Get<DataManager>().Item["TrainingEgg"]);
        PlayerItems.Add(item);
    }

    public void SetPlayerHp()
    {
        int hpUpgradeLevel = Main.Get<UpgradeManager>().HpUpgradeLevel;
        float upgradeHp = hpUpgradeLevel * hpUpgradeLevel;

        if(hpUpgradeLevel > 1)
        {
            PlayerHP = new Vital(EstatType.Hp, 5 + upgradeHp);
        }
        else
        {
            PlayerHP = new Vital(EstatType.Hp, 5);
        }
        PlayerHP.OnValueZero += GameOver;
        PlayerMaxHp = PlayerHP.CurValue;
    }

    public void ExitGame()
    {
        if (!Main.Get<TutorialManager>().isTutorial)
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