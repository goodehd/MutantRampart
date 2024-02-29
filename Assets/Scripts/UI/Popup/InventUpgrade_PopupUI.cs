using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class InventUpgrade_PopupUI : BaseUI
{
    private Button _closeButton;
    private Button[] _upgradeSlots = new Button[3];
    private Button _upgradeButton;
    private Button _autoButton;

    private Image _itemImg;
    private Image[] _upgradeSlotsImgs = new Image[3];
    private Image[] _upgradeCancelImgs = new Image[3];

    public Inventory_PopupUI Owner { get; set; }

    // Upgrade Slot 관련
    private Character[] UpgradeUnitSlots = new Character[3];
    private Room[] UpgradeRoomSlots = new Room[3];
    public int Count { get; private set; } // slot 얼마나 찼는지 체크해주는 역할.
    public ItemData ItemData { get; set; }

    protected override void Init()
    {
        base.Init();
        SetUI<Button>();
        SetUI<Image>();

        _closeButton = GetUI<Button>("InventUpgradeCloseBtn");

        for (int i = 0; i < _upgradeSlots.Length; i++)
        {
            _upgradeSlots[i] = GetUI<Button>($"UpgradeSlotBtn{i + 1}");
            _upgradeSlotsImgs[i] = GetUI<Image>($"UpgradeSlotBtn{i + 1}");
            _upgradeCancelImgs[i] = GetUI<Image>($"UpgradeCancelImg{i + 1}");
        }

        _upgradeButton = GetUI<Button>("UpgradeBtn");
        _autoButton = GetUI<Button>("AutoSelectBtn"); 

        SetUICallback(_closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);
        SetUICallback(_upgradeButton.gameObject, EUIEventState.Click, ClickUpgradeBtn);
        SetUICallback(_autoButton.gameObject, EUIEventState.Click, ClickAutoSelectBtn);
        SetUICallback(_upgradeSlots[0].gameObject, EUIEventState.Click, ClickFirstSlot);
        SetUICallback(_upgradeSlots[1].gameObject, EUIEventState.Click, ClickSecondSlot);
        SetUICallback(_upgradeSlots[2].gameObject, EUIEventState.Click, ClickThirdSlot);
        SetUICallback(_upgradeSlots[0].gameObject, EUIEventState.Hovered, HoveredFirstSlot);
        SetUICallback(_upgradeSlots[0].gameObject, EUIEventState.Exit, ExitFirstSlot);
        SetUICallback(_upgradeSlots[1].gameObject, EUIEventState.Hovered, HoveredSecondSlot);
        SetUICallback(_upgradeSlots[1].gameObject, EUIEventState.Exit, ExitSecondSlot);
        SetUICallback(_upgradeSlots[2].gameObject, EUIEventState.Hovered, HoveredThirdSlot);
        SetUICallback(_upgradeSlots[2].gameObject, EUIEventState.Exit, ExitThirdSlot);

        Owner.inventUpgrade_PopupUI = this;
        Count = 0;

        if (_gameManager.isTutorial)
        {
            _closeButton.gameObject.SetActive(false);
            _autoButton.gameObject.SetActive(false);
        }
    }

    private void ClickAutoSelectBtn(PointerEventData data)
    {
        if (Owner.GetCurUintInven())
        {
            List<Character> characters = FindCharacters();

            for (int i = 0; i < characters.Count; i++)
            {
                ClickSlot(i);
                UpgradeUnitSlots[i] = characters[i];
                SetUnitInfo(i);
                Count++;
            }
        }

        if (Owner.GetCurRoomInven())
        {
            List<Room> rooms = FindRooms();

            for (int i = 0; i < rooms.Count; i++)
            {
                ClickSlot(i);
                UpgradeRoomSlots[i] = rooms[i];
                SetRoomInfo(i);
                Count++;
            }
        }
    }

    private List<Character> FindCharacters()
    {
        Dictionary<string, int> countDictionary = new Dictionary<string, int>();
        List<Character> foundCharacters = new List<Character>();

        foreach (Character character in _gameManager.PlayerUnits)
        {
            if(character.Data.NextKey == "")
            {
                continue;
            }

            string key = $"{character.Data.Key}-{character.Data.Key[character.Data.Key.Length - 1]}";
            if (!countDictionary.ContainsKey(key))
            {
                countDictionary[key] = 1;
            }
            else
            {
                countDictionary[key]++;
            }

            if (countDictionary[key] == 3)
            {
                foundCharacters.Clear();
                foreach (Character chara in _gameManager.PlayerUnits)
                {
                    if (chara.Data.Key == character.Data.Key && chara.Data.Key[character.Data.Key.Length - 1] == character.Data.Key[character.Data.Key.Length - 1])
                    {
                        foundCharacters.Add(chara);
                        
                        if(foundCharacters.Count > 2)
                        {
                            break;
                        }
                    }
                }
                break;
            }
        }
        return foundCharacters;
    }

    private List<Room> FindRooms()
    {
        Dictionary<string, int> countDictionary = new Dictionary<string, int>();
        List<Room> foundCharacters = new List<Room>();

        foreach (Room room in _gameManager.PlayerRooms)
        {
            if (room.Data.NextKey == "")
            {
                continue;
            }

            string key = $"{room.Data.Key}-{room.Data.Key[room.Data.Key.Length - 1]}";
            if (!countDictionary.ContainsKey(key))
            {
                countDictionary[key] = 1;
            }
            else
            {
                countDictionary[key]++;
            }

            if (countDictionary[key] == 3)
            {
                foundCharacters.Clear();
                foreach (Room chara in _gameManager.PlayerRooms)
                {
                    if (chara.Data.Key == room.Data.Key && chara.Data.Key[room.Data.Key.Length - 1] == room.Data.Key[room.Data.Key.Length - 1])
                    {
                        foundCharacters.Add(chara);

                        if (foundCharacters.Count > 2)
                        {
                            break;
                        }
                    }
                }
                break;
            }
        }
        return foundCharacters;
    }

    public void SetUnitInfo(int index) // Unit
    {
        _upgradeSlotsImgs[index].sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{UpgradeUnitSlots[index].Data.Key}");
        _upgradeSlotsImgs[index].enabled = true;
    }

    public void SetRoomInfo(int index) // Room
    {
        _upgradeSlotsImgs[index].sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{UpgradeRoomSlots[index].Data.Key}");
        _upgradeSlotsImgs[index].enabled = true;
    }

    private void ClickCloseBtn(PointerEventData data)
    {
        _ui.ClosePopup();
    }

    private void ClickUpgradeBtn(PointerEventData data)
    {
        // slot 이 null 인 경우 예외처리할 것
        if (_upgradeSlotsImgs[0].sprite == null || _upgradeSlotsImgs[1].sprite == null || _upgradeSlotsImgs[2].sprite == null)
        {
            Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>("Error_PopupUI");
            ui.curErrorText = "슬롯이 비어있습니다!";
            return;
        }

        if (UpgradeUnitSlots[0] != null && UpgradeUnitSlots[1] != null && UpgradeUnitSlots[2] != null) // Unit slot 구분
        {
            // slot 3개에 모두 동일한 데이터가 들어왔다면.
            if (UpgradeUnitSlots[0].Data.Key == UpgradeUnitSlots[1].Data.Key && UpgradeUnitSlots[1].Data.Key == UpgradeUnitSlots[2].Data.Key)
            {
                for (int i = 0; i < UpgradeUnitSlots.Length; i++)
                {
                    _upgradeSlotsImgs[i].sprite = null; // slot image 빼기.
                    _upgradeSlotsImgs[i].enabled = false; // image 컴포넌트 체크 해제.

                    _gameManager.RemoveUnit(UpgradeUnitSlots[i]);
                    Owner.SetUnitInventory();
                }

                // 합성 후 새롭게 능력 부여된 아이템 제공 - NextKey 통해.
                _gameManager.PlayerUnits.Add(new Character(Main.Get<DataManager>().Character[UpgradeUnitSlots[0].Data.NextKey]));
                Owner.SetUnitInventory();

                if (_gameManager.isTutorial) // 튜토리얼 중이라면
                {                    
                    if (_gameManager.PlayerUnits.Count == 1 && _gameManager.PlayerRooms.Count == 2) // 유닛 업그레이드 했다면
                    {
                        if (Owner.tweener.IsActive())
                        {
                            Owner.tweener.Kill();
                        }

                        Owner.inventArrowImg.gameObject.SetActive(true);
                        Owner.inventArrowTransform.anchoredPosition = new Vector3(526f, 90f, 0f); // 보유 unit img 화살표
                        Owner.tweener = Owner.inventArrowTransform.DOAnchorPosY(120f, Owner.animationDuration).SetLoops(-1, LoopType.Yoyo);

                        TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
                        ui.curTutorialText = "잘하셨어요!\n이번에는 <color=#E9D038><b>아이템 장착</b></color>에 대해 설명드릴게요.\n\n아이템을 장착하려면\n먼저 보유한 <color=#E9D038><b>Unit</b></color>을 클릭한 뒤,\n보유한 아이템 목록에서 <color=#E9D038><b>아이템</b></color>을 클릭하시면 돼요."; // <color=#E9D038><b>
                        ui.isBackgroundActive = true;
                        ui.isCloseBtnActive = true;
                    }
                }

                Array.Clear(UpgradeUnitSlots, 0, UpgradeUnitSlots.Length);
                Count = 0;
            }
            else
            {
                Error_PopupUI ui = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI");
                ui.curErrorText = "동일한 종류,\n레벨의 유닛을 넣어주세요!";
            }
        }

        if (UpgradeRoomSlots[0] != null && UpgradeRoomSlots[1] != null && UpgradeRoomSlots[2] != null) // Room slot 구분
        {
            // slot 3개에 모두 동일한 데이터가 들어왔다면.
            if (UpgradeRoomSlots[0].Data.Key == UpgradeRoomSlots[1].Data.Key && UpgradeRoomSlots[1].Data.Key == UpgradeRoomSlots[2].Data.Key)
            {
                for (int i = 0; i < UpgradeRoomSlots.Length; i++)
                {
                    _upgradeSlotsImgs[i].sprite = null; // slot image 빼기.
                    _upgradeSlotsImgs[i].enabled = false; // image 컴포넌트 체크 해제.

                    _gameManager.RemoveRoom(UpgradeRoomSlots[i]);
                    Owner.SetRoomInventory();
                }

                // 합성 후 새롭게 능력 부여된 아이템 제공 - NextKey 통해.
                _gameManager.PlayerRooms.Add(new Room(Main.Get<DataManager>().Room[UpgradeRoomSlots[0].Data.NextKey]));
                Owner.SetRoomInventory();
                
                if (_gameManager.isTutorial) // 튜토리얼 중이라면
                {
                    if (_gameManager.PlayerRooms.Count == 4)
                    {
                        if (Owner.tweener.IsActive())
                        {
                            Owner.tweener.Kill();
                        }
                        Owner.inventArrowImg.gameObject.SetActive(false);

                    }
                    if (_gameManager.PlayerRooms.Count == 2)
                    {
                        if (Owner.tweener.IsActive())
                        {
                            Owner.tweener.Kill();
                        }

                        Owner.inventArrowTransform.anchoredPosition = new Vector3(592f, 270f, 0f); // unit 버튼 화살표
                        Owner.tweener = Owner.inventArrowTransform.DOAnchorPosY(300f, Owner.animationDuration).SetLoops(-1, LoopType.Yoyo);

                        TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>(); //
                        ui.curTutorialText = "같은 방식으로\n보유한 <color=#E9D038><b>Unit</b></color> 도 업그레이드를 진행해주세요!"; // <color=#E9D038><b>
                        ui.isBackgroundActive = true;
                        ui.isCloseBtnActive= true;
                    }
                }

                Array.Clear(UpgradeRoomSlots, 0, UpgradeRoomSlots.Length);
                Count = 0;
            }
            else
            {
                Error_PopupUI ui = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI");
                ui.curErrorText = "동일한 종류,\n레벨의 룸을 넣어주세요!";
            }
        }
    }

    private void ClickFirstSlot(PointerEventData data)
    {
        ClickSlot(0); // slot0 에서 Remove
    }

    private void ClickSecondSlot(PointerEventData data)
    {
        ClickSlot(1); // slot1 에서 Remove
    }

    private void ClickThirdSlot(PointerEventData data)
    {
        ClickSlot(2); // slot2 에서 Remove
    }

    private void ClickSlot(int i)
    {
        if (UpgradeUnitSlots == null || UpgradeRoomSlots == null) return;

        Count--;

        if (UpgradeUnitSlots[i] != null)
        {
            UpgradeUnitSlots[i] = null;
        }
        else if (UpgradeRoomSlots[i] != null)
        {
            UpgradeRoomSlots[i] = null;
        }

        _upgradeSlotsImgs[i].sprite = null; // slot image 빼기
        _upgradeSlotsImgs[i].enabled = false; // image 컴포넌트 체크 해제.
    }

    private void HoveredFirstSlot(PointerEventData data)
    {
        _upgradeCancelImgs[0].gameObject.SetActive(true);
    }

    private void ExitFirstSlot(PointerEventData data)
    {
        _upgradeCancelImgs[0].gameObject.SetActive(false);
    }

    private void HoveredSecondSlot(PointerEventData data)
    {
        _upgradeCancelImgs[1].gameObject.SetActive(true);
    }

    private void ExitSecondSlot(PointerEventData data)
    {
        _upgradeCancelImgs[1].gameObject.SetActive(false);
    }

    private void HoveredThirdSlot(PointerEventData data)
    {
        _upgradeCancelImgs[2].gameObject.SetActive(true);
    }

    private void ExitThirdSlot(PointerEventData data)
    {
        _upgradeCancelImgs[2].gameObject.SetActive(false);
    }

    public void AddUpgradeUnitSlot(Character unit)
    {
        int index = -1; // 비어있는 배열 요소의 index 찾았는지 아닌지 판별을 위한 친구. (못찾았다면 -1)

        for (int i = 0; i < UpgradeUnitSlots.Length; i++)
        {
            if (UpgradeUnitSlots[i] == null && index == -1)
            {
                index = i;
            }

            if (UpgradeUnitSlots[i] == unit) // 똑같은 친구가 있으면 return.
            {
                return;
            }
        }

        if (index == -1) // UpgradeUnitSlots 배열이 꽉 차있다면 return.
        {
            return;
        }

        UpgradeUnitSlots[index] = unit;
        SetUnitInfo(index);
        Count++;
    }

    public void AddUpgradeRoomSlot(Room room)
    {
        int index = -1;

        for (int i = 0; i < UpgradeRoomSlots.Length; i++)
        {
            if (UpgradeRoomSlots[i] == null && index == -1)
            {
                index = i;
            }
            if (UpgradeRoomSlots[i] == room)
            {
                return;
            }
        }

        if (index == -1)
        {
            return;
        }

        UpgradeRoomSlots[index] = room;
        SetRoomInfo(index);
        Count++;
    }
}
