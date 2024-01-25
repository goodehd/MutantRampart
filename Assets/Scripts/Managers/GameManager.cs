using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManager : IManagers
{

    public int _playerMoney { get; private set; } = 100000; // 플레이어 보유 돈
    private int _playerHp = 5; // 플레이어 체력
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
    //private List<Unit> playerUnits = new List<Unit>(); 
    public List<CharacterData> playerUnits { get; private set; } = new List<CharacterData>(); // 플레이어가 보유한 유닛 리스트
    public List<RoomData> PlayerRooms { get; private set; } = new List<RoomData>(); // 플레이어가 보유한 Room 리스트
    public static bool isGamePaused { get; private set; } // 다른 스크립트에서 쉽게 접근이 가능하도록 메모리에 할당 - static, 읽기전용

    public event Action<int> OnChangeMoney;

    public bool Init()
    {
        playerUnits.Add(Main.Get<DataManager>().Character["Gun"]);
        playerUnits.Add(Main.Get<DataManager>().Character["Jotem"]);
        playerUnits.Add(Main.Get<DataManager>().Character["Warrior"]);
        
        PlayerRooms.Add(Main.Get<DataManager>().roomDatas["Forest"]);
        PlayerRooms.Add(Main.Get<DataManager>().roomDatas["Lava"]);
        PlayerRooms.Add(Main.Get<DataManager>().roomDatas["Snow"]);
        PlayerRooms.Add(Main.Get<DataManager>().roomDatas["Home_2"]);

        return true;
    }

    public void ChangeMoney(int amount)
    {
        _playerMoney += amount;
        OnChangeMoney?.Invoke(_playerMoney);
    }

    // Start is called before the first frame update
    //private void Start()
    //{
        
    //    // 돈 관련 UI 업데이트
    //    RefreshMoneyUI();

    //}

    //private string FormatNumber(int num) // 천의자리마다 ,(콤마) 찍어주는 함수
    //{
    //    return string.Format("{0:#,###}", num);
    //}

    // 돈 관리
    private void RefreshMoneyUI() // UI 에 업데이트해줄 함수 구현 --> event 를 사용한다!!!
    {
        // ShopUI 에 있는 _playerMoneyText 어떻게 델꼬와 ?? --> event 를 사용한다.
        //moneyText.text = FormatNumber(_playerMoney);
        //ShopUI._playerMoneyText.text = FormatNumber(_playerMoney);
    }

    //BuyItem 을 UI 로 옮기고
    //GameManager 에는 산 거를 추가시키는 함수가 있어야 한다. - 산거를 받아서 리스트에 넣는 친구.


    //public void BuyItem(ShopItemData data)
    //{
    //    if (_playerMoney >= data.Price)
    //    {
    //        _playerMoney -= data.Price;
    //        playerUnitInventory.Add(data);
    //        RefreshMoneyUI(); // 돈이 변경될 때마다 UI 업데이트
    //        Debug.Log("구매완료했습니다.");
    //        Debug.Log($"잔액 : {_playerMoney}");
    //    }
    //    else // 보유 금액 부족 시
    //    {
    //        Main.Get<UIManager>().OpenPopup<MoneyErrorUI>("MoneyError_PopupUI"); // 돈이 아이템 금액보다 적으면 돈부족 경고창 띄우기
    //        Debug.Log("돈이 부족해서 구매할 수 없습니다.");
    //    }
    //}



    // Update is called once per frame
    private void Update()
    {
        if (isGamePaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
    public void PauseTime()
    {
        isGamePaused = true;
    }

    public void ResumeTime()
    {
        isGamePaused = false;
    }



    //public void AddMoney(int money)
    //{
    //    _playerMoney += money;
    //    RefreshMoneyUI();
    //}

    //public void Spendmoney(int money)
    //{
    //    if (_playerMoney < money) // 돈 부족 시
    //    {
    //        ShowWarning();
    //        Debug.Log("돈이 부족해 !!");
    //    }

    //    _playerMoney -= money;
    //    RefreshMoneyUI();

    //}

    // 유닛 관리
    //public void AddPlayerUnit(Unit unit)
    //{
    //    playerUnits.Add(unit);
    //}

    //public void RemovePlayerUnit(Unit unit)
    //{
    //    playerUnits.Remove(unit);
    //}

    // 타일 관리
    //public void AddPlayerTile(Tile tile)
    //{
    //    playerTiles.Add(tile);
    //}

    //public void RemovePlayerTile(Tile tile)
    //{
    //    playerTiles.Remove(tile);
    //}

    //public void ResetPlayerResources() // 플레이어의 자원(돈, 유닛, 타일 등)을 초기화하는 함수
    //{
    //    playerMoney = 0;
    //    playerUnits.Clear();
    //    playerTiles.Clear();
    //}

    public void InitGame() // 게임을 초기화하는 함수
    {
        // e.g. 초기 자원 할당 등
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        Main.Get<UIManager>().OpenPopup<StageFail_PopupUI>("StageFail_PopupUI");
    }
    //private void ShowWarning() // "잔액이 부족합니다" 팝업
    //{
    //    //popupMsgUI.SetActive(true);
    //}

}
