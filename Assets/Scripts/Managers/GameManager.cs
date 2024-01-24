using System.Collections.Generic;
using UnityEngine;


public class GameManager : IManagers
{

    private int _playerMoney; // 플레이어 보유 돈
    private int _playerHp = 100; // 플레이어 체력
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
        }
    }
    //private List<Unit> playerUnits = new List<Unit>(); // todo : 플레이어가 보유한 유닛 리스트 -- 리스트 자료형 체크
    public List<CharacterData> playerUnits = new List<CharacterData>(); // todo : 플레이어가 보유한 유닛 리스트 -- 리스트 자료형 체크
    public List<RoomData> PlayerRooms { get; private set; } = new List<RoomData>(); // todo : 플레이어가 보유한 타일 리스트 -- 리스트 자료형 체크
    public List<ShopItemData> ShopUnitItems { get; private set; } = new List<ShopItemData>(); // 상점 - UnitItems
    public List<ShopItemData> ShopRoomItems { get; private set; } = new List<ShopItemData>(); // 상점 - RoomItems
    public static bool isGamePaused { get; private set; } // 다른 스크립트에서 쉽게 접근이 가능하도록 메모리에 할당 - static, 읽기전용

    public bool Init()
    {
        playerUnits.Add(Main.Get<DataManager>().unit["Gun"]);
        playerUnits.Add(Main.Get<DataManager>().unit["Jotem"]);
        playerUnits.Add(Main.Get<DataManager>().unit["Warrior"]);
        
        PlayerRooms.Add(Main.Get<DataManager>().roomDatas["Forest"]);
        PlayerRooms.Add(Main.Get<DataManager>().roomDatas["Lava"]);
        PlayerRooms.Add(Main.Get<DataManager>().roomDatas["Snow"]);
        PlayerRooms.Add(Main.Get<DataManager>().roomDatas["Home_2"]);

        ShopUnitItems.Add(Main.Get<DataManager>().shopItemData["Gun"]);
        ShopUnitItems.Add(Main.Get<DataManager>().shopItemData["Jotem"]);
        ShopUnitItems.Add(Main.Get<DataManager>().shopItemData["Warrior"]);

        ShopRoomItems.Add(Main.Get<DataManager>().shopItemData["Forest"]);
        ShopRoomItems.Add(Main.Get<DataManager>().shopItemData["Igloo"]);
        ShopRoomItems.Add(Main.Get<DataManager>().shopItemData["Lava"]);
        ShopRoomItems.Add(Main.Get<DataManager>().shopItemData["LivingRoom"]);
        ShopRoomItems.Add(Main.Get<DataManager>().shopItemData["Molar"]);
        ShopRoomItems.Add(Main.Get<DataManager>().shopItemData["Snow"]);
        ShopRoomItems.Add(Main.Get<DataManager>().shopItemData["Temple"]);


        return true;
    }

    // Start is called before the first frame update
    private void Start()
    {
        // 돈 관련 UI 업데이트
        Refresh();

    }

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

    // 돈 관리
    private void Refresh() // UI 에 Update 해줄 함수 구현
    {
        //moneyText.text = playerMoney.ToString();
    }

    public void AddMoney(int money)
    {
        _playerMoney += money;
        Refresh();
    }

    public void Spendmoney(int money)
    {
        if (_playerMoney < money) // 돈 부족 시
        {
            ShowWarning();
            Debug.Log("돈이 부족해 !!");
        }

        _playerMoney -= money;
        Refresh();

    }

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

    }
    private void ShowWarning() // "잔액이 부족합니다" 팝업
    {
        //popupMsgUI.SetActive(true);
    }

}
