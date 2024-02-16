using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : IManagers
{
    private TileManager _tile;
    public int PlayerMoney { get; set; }
    public Vital PlayerHP { get; private set; }

    public bool isHomeSet = false;

    public int CurStage;

    public List<Character> playerUnits { get; private set; } = new List<Character>();   // 플레이어가 보유한 유닛 리스트
    public List<Room> PlayerRooms { get; private set; } = new List<Room>();     // 플레이어가 보유한 Room 리스트
    public List<Item> PlayerItems { get; private set; } = new List<Item>();             // 플레이어가 보유한 아이템 리스트
    public event Action<int> OnChangeMoney;

    public bool Init()
    {
        _tile = Main.Get<TileManager>();

        //PlayerRooms.Add(Main.Get<DataManager>().Room["Home"]);
        Room room = new Room();
        room.Init(Main.Get<DataManager>().Room["Home"]);
        PlayerRooms.Add(room);

        /* Room room2 = new Room();
         room2.Init(Main.Get<DataManager>().Room["Forest"]);
         PlayerRooms.Add(room2);

         playerUnits.Add(new Character(Main.Get<DataManager>().Character["Gun"]));
         playerUnits.Add(new Character(Main.Get<DataManager>().Character["Jotem"]));


         Item item1 = new Item();
         item1.Init(Main.Get<DataManager>().Item["Meat"]);
         PlayerItems.Add(item1);

         Item item2 = new Item();
         item2.Init(Main.Get<DataManager>().Item["BlueBook"]);
         PlayerItems.Add(item2);
         Item item3 = new Item();
         item3.Init(Main.Get<DataManager>().Item["RedBook"]);
         PlayerItems.Add(item3);

         Item item4 = new Item();
         item4.Init(Main.Get<DataManager>().Item["SilverCoin"]);
         PlayerItems.Add(item4);*/

        PlayerHP = new Vital(EstatType.Hp, 5);
        PlayerHP.OnValueZero += GameOver;

        PlayerMoney = 20000;

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
        StageFail_PopupUI ui = Main.Get<UIManager>().OpenPopup<StageFail_PopupUI>("StageFail_PopupUI");
        ui._curStage = CurStage;

    }

    public void RemoveUnit(Character unit) // unit
    {
        if (unit.CurRoom != null) // 유닛이 배치되어있는 경우에
        {
            ((BatRoom)unit.CurRoom).DeleteUnit(unit); // 배치되어있는 유닛 빼면서
        }
        playerUnits.Remove(unit); // 인벤토리에서도 지우고 
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
        Main.Get<SaveDataManager>().Player.Curstage = CurStage;
        Main.Get<SaveDataManager>().Player.PlayerMoney = PlayerMoney;
        Main.Get<SaveDataManager>().SaveData();
    }

    // todo : Item 뺄 때 해야하는 것들 함수로 작동시키기 !
    //public void RemoveItem(Item item) // item -- 인벤토리에서 삭제, 장착되어있는 것에서 해제
    //{

    //}
}