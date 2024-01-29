using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : IManagers
{
    public int _playerMoney { get; private set; } = 5000;
    private int _playerHp = 5;
    public int PlayerHp
    {
        get 
        { 
            return _playerHp; 
        }
        set
        {
            _playerHp = value;
            if (_playerHp <= 0)
            {
                GameOver();
            }
            Debug.Log($"플레이어 체력 : {_playerHp}");
        }
    }

    public List<Character> playerUnits { get; private set; } = new List<Character>();   // 플레이어가 보유한 유닛 리스트
    public List<ThisRoom> PlayerRooms { get; private set; } = new List<ThisRoom>();             // 플레이어가 보유한 Room 리스트

    public event Action<int> OnChangeMoney;

    public bool Init()
    {
        //PlayerRooms.Add(Main.Get<DataManager>().Room["Home_2"]);
        ThisRoom room = new ThisRoom();
        room.Init(Main.Get<DataManager>().Room["Forest"]);
        PlayerRooms.Add(room);

        ThisRoom room2 = new ThisRoom();
        room2.Init(Main.Get<DataManager>().Room["Lava"]);
        PlayerRooms.Add(room2);

        playerUnits.Add(new Character(Main.Get<DataManager>().Character["Gun"]));
        playerUnits.Add(new Character(Main.Get<DataManager>().Character["Jotem"]));

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
