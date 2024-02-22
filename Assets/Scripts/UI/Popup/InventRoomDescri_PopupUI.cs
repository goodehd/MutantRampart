using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventRoomDescri_PopupUI : BaseUI
{
    private Button _closeBtn;
    //private Button _upgradeBtn;
    private Button _deleteBtn;

    private TMP_Text _roomName;
    private TMP_Text _roomType;
    private TMP_Text _roomDescription;

    private Image _inventRoomImg;

    public Room RoomData { get; set; }

    public InventRoom_ContentsBtnUI Owner { get; set; }

    protected override void Init()
    {
        SetUI<Button>();
        SetUI<TMP_Text>();
        SetUI<Image>();

        _closeBtn = GetUI<Button>("InventRoomCloseBtn");
        //_upgradeBtn = GetUI<Button>("InventRoomUpgradeBtn");
        _deleteBtn = GetUI<Button>("InventRoomDeleteBtn");

        SetUICallback(_closeBtn.gameObject, EUIEventState.Click, ClickCloseBtn);
        //SetUICallback(_upgradeBtn.gameObject, EUIEventState.Click, ClickUpgradeBtn);
        SetUICallback(_deleteBtn.gameObject, EUIEventState.Click, ClickDeleteBtn);

        _roomName = GetUI<TMP_Text>("InventRoomNameTxt");
        _roomType = GetUI<TMP_Text>("InventRoomTypeTxt");
        _roomDescription = GetUI<TMP_Text>("InventRoomDescriTxt");

        _inventRoomImg = GetUI<Image>("InventRoomImg");

        SetInfo();

        if (Main.Get<GameManager>().isTutorial) // 튜토리얼 진행 중일 때 삭제버튼 비활성화.
        {
            _deleteBtn.gameObject.SetActive(false);
        }
    }

    public void SetInfo()
    {
        _roomName.text = RoomData.Data.Key;
        _roomType.text = RoomData.Data.Type.ToString();
        _roomDescription.text = RoomData.Data.Instruction;
        _inventRoomImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{RoomData.Data.Key}");
    }

    private void ClickCloseBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().ClosePopup();
        Owner._selectCheckImg.gameObject.SetActive(false);
    }

    //private void ClickUpgradeBtn(PointerEventData EventData)
    //{
    //    Main.Get<UIManager>().OpenPopup<PastUpgrade_PopupUI>("PastUpgrade_PopupUI");

    //}

    private void ClickDeleteBtn(PointerEventData EventData)
    {
        if (RoomData.Data.Key == "Home")
        {
            Error_PopupUI errorUI = Main.Get<UIManager>().OpenPopup<Error_PopupUI>();
            errorUI.curErrorText = "Home 은 삭제할 수 없습니다.";
            return;
        }

        Main.Get<GameManager>().RemoveRoom(RoomData);
        Main.Get<UIManager>().ClosePopup(); // 설명창 닫아주고
        Owner.Owner.SetRoomInventory(); // 인벤토리 리프레쉬
    }

    private void OnDestroy()
    {
        Owner.Owner.inventRoomDescri_PopupUI = null; // null 처리 !
    }
}
