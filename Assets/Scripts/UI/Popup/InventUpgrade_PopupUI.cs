using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventUpgrade_PopupUI : BaseUI
{
    private Button _closeButton;
    private Button[] _upgradeSlots = new Button[3];
    private Button _upgradeButton;

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

        SetUICallback(_closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);
        SetUICallback(_upgradeButton.gameObject, EUIEventState.Click, ClickUpgradeBtn);
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
    }

    public void SetUnitInfo(int index) // Unit
    {
        _upgradeSlotsImgs[index].sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{UpgradeUnitSlots[index].Data.Key}");
        _upgradeSlotsImgs[index].enabled = true;

        //if (UpgradeUnitSlots.Length > 3) // 3개 가득 찬 경우 예외처리 해주기
        //{
        //    Debug.Log("슬롯이 가득 찼어유~");
        //    return;
        //}

        //for (int i = 0; i < UpgradeUnitSlots.Length; i++)
        //{
        //    if (_upgradeSlotsImgs[i].sprite == null) // 기존 슬롯에 이미 데이터가 들어가있으면 다음 슬롯으로 들어가게끔 !
        //    {
        //        _upgradeSlotsImgs[i].sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{unitData.Data.Key}");
        //        _upgradeSlotsImgs[i].enabled = true; // image 컴포넌트 체크 설정.

        //    }
        //}
    }

    public void SetRoomInfo(int index) // Room
    {
        _upgradeSlotsImgs[index].sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{UpgradeRoomSlots[index].Data.Key}");
        _upgradeSlotsImgs[index].enabled = true;
    }

    //public void SetUnitInfo(Character unitData)
    //{
    //    // UnitData

    //    _upgradeSlotsImgs[0].sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{unitData.Data.Key}");
    //    _upgradeSlotsImgs[0].enabled = true; // image 컴포넌트 체크 설정.

    //}

    //public void AddUnitToSlot(Character unitData)
    //{

    //    for (int i = 0; i < _upgradeSlots.Length; i++) // 첫 번째 빈 슬롯을 찾아 아이템을 추가
    //    {
    //        if (UpgradeUnitSlots.Count <= i)
    //        {
    //            UpgradeUnitSlots.Add(unitData); // 아이템 추가
    //            _upgradeSlotsImgs[i].sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{unitData.Data.Key}");
    //            _upgradeSlotsImgs[i].enabled = true; // image 컴포넌트 체크 설정.
    //            return;
    //        }
    //        else if (UpgradeUnitSlots[i] == null)
    //        {
    //            slots[i] = item; // 아이템 추가
    //            Debug.Log("Added item to slot " + (i + 1));
    //            return;
    //        }
    //    }
    //    Debug.Log("All slots are full.");


    //    //for (int i = 0; i < 3; i++) // 슬롯 List 를 반복해서 첫 번째 빈 슬롯을 찾기
    //    //{
    //    //    // 슬롯에 데이터가 없는 경우
    //    //    if (UpgradeUnitSlots[i] == null)
    //    //    {
    //    //        UpgradeUnitSlots.Add(unitData); // 슬롯에 유닛 추가
    //    //        _upgradeSlotsImgs[i].sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{unitData.Data.Key}");
    //    //        _upgradeSlotsImgs[i].enabled = true; // image 컴포넌트 체크 설정.

    //    //        return;
    //    //    }
    //    //}
    //    //Debug.Log("All slots are full.");
    //}

    private void ClickCloseBtn(PointerEventData data)
    {
        //Array.Clear(UpgradeUnitSlots, 0, UpgradeUnitSlots.Length); // unit slot 배열 초기화
        //Array.Clear(UpgradeRoomSlots, 0, UpgradeRoomSlots.Length); // room slot 배열 초기화
        //Count = 0;

        Main.Get<UIManager>().ClosePopup();
        //Owner.inventUnitDescri_PopupUI.Owner._selectCheckImg.gameObject.SetActive(false);
    }

    private void ClickUpgradeBtn(PointerEventData data)
    {
        // slot 이 null 인 경우 예외처리할 것
        if (_upgradeSlotsImgs[0].sprite == null || _upgradeSlotsImgs[1].sprite == null || _upgradeSlotsImgs[2].sprite == null)
        {
            Error_PopupUI ui = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI");
            ui.curErrorText = "슬롯이 비어있습니다!";
            Debug.Log("슬롯이 비어있습니다!");
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

                    Main.Get<GameManager>().RemoveUnit(UpgradeUnitSlots[i]);
                    Owner.SetUnitInventory();
                }

                // 합성 후 새롭게 능력 부여된 아이템 제공 - NextKey 통해.
                Main.Get<GameManager>().playerUnits.Add(new Character(Main.Get<DataManager>().Character[UpgradeUnitSlots[0].Data.NextKey]));
                Owner.SetUnitInventory();

                Array.Clear(UpgradeUnitSlots, 0, UpgradeUnitSlots.Length);
                Count = 0;
            }
            else
            {
                Error_PopupUI ui = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI");
                ui.curErrorText = "동일한 종류,\n레벨의 유닛을 넣어주세요!";
                Debug.Log("동일한 유닛을 넣어주세요!");
            }
            // slot 에 있는 데이터는 다 지워주고 새롭게 능력이 부여된 아이템 획득
            // slot[i] == null 이면 return; 해주기
            //_upgradeSlotsImgs[i].sprite = null; // slot image 빼버리고
            //_upgradeSlotsImgs[i].enabled = false; // image 컴포넌트 체크 해제.
            //// 새롭게 능력이 부여된 아이템 획득
            //Character newChar = new Character(data);
            //Main.Get<GameManager>().playerUnits.Add(newChar);
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

                    Main.Get<GameManager>().RemoveRoom(UpgradeRoomSlots[i]);
                    Owner.SetRoomInventory();
                }

                // 합성 후 새롭게 능력 부여된 아이템 제공 - NextKey 통해.
                Main.Get<GameManager>().PlayerRooms.Add(new Room(Main.Get<DataManager>().Room[UpgradeRoomSlots[0].Data.NextKey]));
                Owner.SetRoomInventory();

                Array.Clear(UpgradeRoomSlots, 0, UpgradeRoomSlots.Length);
                Count = 0;
            }
            else
            {
                Error_PopupUI ui = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI");
                ui.curErrorText = "동일한 종류,\n레벨의 룸을 넣어주세요!";
                Debug.Log("동일한 룸을 넣어주세요!");
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

    // todo : public void AddUpgradeItemSlot()
    //{

    //}
}
