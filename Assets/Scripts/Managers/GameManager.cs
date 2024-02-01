using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameManager : IManagers
{
    public int _playerMoney { get; private set; } = 5000;
    public Vital PlayerHP { get; private set; }

    public bool isHomeSet = false;

    public List<Character> playerUnits { get; private set; } = new List<Character>();   // 플레이어가 보유한 유닛 리스트
    public List<ThisRoom> PlayerRooms { get; private set; } = new List<ThisRoom>();     // 플레이어가 보유한 Room 리스트
    public List<Item> PlayerItems { get; private set; } = new List<Item>();             // 플레이어가 보유한 아이템 리스트
    public event Action<int> OnChangeMoney;

    public bool Init()
    {
        //PlayerRooms.Add(Main.Get<DataManager>().Room["Home_2"]);
        ThisRoom room = new ThisRoom();
        room.Init(Main.Get<DataManager>().Room["Home_2"]);
        PlayerRooms.Add(room);

        ThisRoom room2 = new ThisRoom();
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
        PlayerItems.Add(item4);

        PlayerHP = new Vital(EstatType.Hp, 5);
        PlayerHP.OnValueZero += GameOver;

        return true;
    }

    public void ChangeMoney(int amount)
    {
        _playerMoney += amount;
        OnChangeMoney?.Invoke(_playerMoney);
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        Main.Get<UIManager>().OpenPopup<StageFail_PopupUI>("StageFail_PopupUI");
    }
}
