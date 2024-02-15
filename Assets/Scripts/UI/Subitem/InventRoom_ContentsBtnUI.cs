using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventRoom_ContentsBtnUI : BaseUI
{
    private Image _roomContentsImg;
    private Button _roomContentsBtn;
    public Image _selectCheckImg { get; private set; }

    public Room RoomData { get; set; }

    public Inventory_PopupUI Owner { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();

        _roomContentsImg = GetUI<Image>("InventRoom_ContentsBtnUI");
        _roomContentsBtn = GetUI<Button>("InventRoom_ContentsBtnUI");
        _selectCheckImg = GetUI<Image>("InventRoomEquipCheckImg");

        SetUICallback(_roomContentsBtn.gameObject, EUIEventState.Click, ClickRoomContentBtn);
        SetUICallback(_roomContentsBtn.gameObject, EUIEventState.Hovered, HoveredUnitContentBtn);
        SetUICallback(_roomContentsBtn.gameObject, EUIEventState.Exit, ExitUnitContentBtn);

        SetInfo();
    }

    private void SetInfo()
    {
        _roomContentsImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{RoomData.Data.Key}");
    }

    private void ClickRoomContentBtn(PointerEventData data)
    {
        if (Owner.inventUpgrade_PopupUI != null) // 업그레이드창이 열려있다면
        {
            if (Owner.inventUpgrade_PopupUI.Count >= 3) // 3개 가득 찬 경우 예외처리 해주기
            {
                Error_PopupUI ui = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI");
                ui.curErrorText = "슬롯이 가득 찼습니다!";
                Debug.Log("슬롯가득참!");
                return;
            }

            Owner.inventUpgrade_PopupUI.AddUpgradeRoomSlot(RoomData);
        }
        else if (Owner.inventRoomDescri_PopupUI == null) // 설명 팝업이 안 떠 있다면
        {
            Owner.inventRoomDescri_PopupUI = Main.Get<UIManager>().OpenPopup<InventRoomDescri_PopupUI>(); // 설명창 열어주고
            Owner.inventRoomDescri_PopupUI.RoomData = RoomData; // 데이터 넘겨주고
            Owner.inventRoomDescri_PopupUI.Owner = this; // owner 설정해주고

            _selectCheckImg.gameObject.SetActive(true);
        }

        else // 설명 팝업이 이미 떠 있다면
        {
            Owner.inventRoomDescri_PopupUI.Owner._selectCheckImg.gameObject.SetActive(false); // 선택표시가 기존에 활성화되어있다면 일단 꺼준다.

            Owner.inventRoomDescri_PopupUI.RoomData = RoomData; // 데이터 넘겨주고
            Owner.inventRoomDescri_PopupUI.SetInfo(); // 데이터 갱신
            Owner.inventRoomDescri_PopupUI.Owner = this; // owner 업데이트

            _selectCheckImg.gameObject.SetActive(true); // 그리고 다시 선택표시가 active 해주기.

        }
    }

    private void HoveredUnitContentBtn(PointerEventData data)
    {
        _roomContentsImg.color = Color.cyan;
    }

    private void ExitUnitContentBtn(PointerEventData data)
    {
        _roomContentsImg.color = Color.white;
    }

}
