using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GachaResult_PopupUI : BaseUI
{
    private Transform gachaResultContent;
    private Button closeButton;
    private Button retryButton;

    // Data
    public List<CharacterData> GachaUnitData { get; set; }
    public List<RoomData> GachaRoomData { get; set; }
    public List<ItemData> GachaItemData { get; set; }
    public List<ItemData> ShopGroundData { get; set; }
    public Shop_PopupUI Owner { get; set; }

    protected override void Init()
    {
        base.Init();
        SetUI<Transform>();
        SetUI<Button>();

        gachaResultContent = GetUI<Transform>("GachaResult_Content");
        
        closeButton = GetUI<Button>("GachaResultCloseBtn");
        retryButton = GetUI<Button>("GachaReTryBtn");

        SetUICallback(closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);
        SetUICallback(retryButton.gameObject, EUIEventState.Click, ClickRetryBtn);

        SetResultImgUInfo();
    }

    private void ClickRetryBtn(PointerEventData data)
    {
        if (_gameManager.isTutorial)
        {
            return;
        }
        SaveData();
        _ui.ClosePopup();
        Owner.ReTryAction(data);
    }

    private void ClickCloseBtn(PointerEventData eventData)
    {
        SaveData();
        _ui.ClosePopup();

        if (_gameManager.isTutorial)
        {
            if (_gameManager.PlayerUnits.Count >= 3)
            {
                if (Owner.tweener.IsActive())
                {
                    Owner.tweener.Kill(); // room 가리키는 화살표 Kill.
                }

                if (_gameManager.PlayerRooms.Count == 1)
                {
                    TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
                    ui.curTutorialText = Main.Get<DataManager>().Tutorial["T2"].Description;
                    ui.isCloseBtnActive = true;
                    ui.isBackgroundActive = true;

                    Owner.shopArrowImg.gameObject.SetActive(true);
                    Owner.shopArrowTransform.anchoredPosition = new Vector3(369f, 376f, 0f); // 상점 내 Room 카테고리 가리키는 화살표.
                    Owner.tweener = Owner.shopArrowTransform.DOAnchorPosY(346f, Owner.animationDuration).SetLoops(-1, LoopType.Yoyo);
                }
            }

            if (_gameManager.PlayerUnits.Count >= 3 && _gameManager.PlayerRooms.Count >= 4)
            {
                Owner.backButton.gameObject.SetActive(true);
                Owner.shopArrowImg.gameObject.SetActive(true);

                Owner.shopArrowTransform.anchoredPosition = new Vector3(-770f, 484f, 0f); // 상점 뒤로가기 버튼 가리키는 화살표.
                Owner.shopArrowTransform.Rotate(0f, 0f, -90f);
                Owner.tweener = Owner.shopArrowTransform.DOAnchorPosX(-800f, 0.3f).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }

    private void SetResultImgUInfo()
    {
        if (GachaUnitData != null)
        {
            for (int i = 0; i < GachaUnitData.Count; i++)
            {
                GachaResultImgUI gachaResultImg = Main.Get<UIManager>().CreateSubitem<GachaResultImgUI>("GachaResultImgUI", gachaResultContent);
                gachaResultImg.GachaUnitData = GachaUnitData[i];
            }
        }
        else if (GachaRoomData != null)
        {
            for (int i = 0; i < GachaRoomData.Count; i++)
            {
                GachaResultImgUI gachaResultImg = Main.Get<UIManager>().CreateSubitem<GachaResultImgUI>("GachaResultImgUI", gachaResultContent);
                gachaResultImg.GachaRoomData = GachaRoomData[i];
            }
        }
        else if (GachaItemData != null)
        {
            for (int i = 0; i < GachaItemData.Count; i++)
            {
                GachaResultImgUI gachaResultImg = Main.Get<UIManager>().CreateSubitem<GachaResultImgUI>("GachaResultImgUI", gachaResultContent);
                gachaResultImg.GachaItemData = GachaItemData[i];
            }
        }
    }

    private void SaveData()
    {
        if (GachaUnitData != null) //인벤토리로 옮기려는 데이터가 유닛 일 때
        {
            for (int i = 0; i < GachaUnitData.Count; i++)
            {
                SaveUnitInInventory(GachaUnitData[i]);
            }
        }
        else if (GachaRoomData != null) // Room
        {
            for (int i = 0; i < GachaRoomData.Count; i++)
            {
                SaveRoomInInventory(GachaRoomData[i]);
            }
        }
        else if (GachaItemData != null) // Item
        {
            for (int i = 0; i < GachaItemData.Count; i++)
            {
                SaveItemInInventory(GachaItemData[i]);
            }
        }

        if (!_gameManager.isTutorial)
        {
            _gameManager.SaveData();
        }
    }

    private void SaveUnitInInventory(CharacterData data)
    {
        Character newChar = new Character(data);
        _gameManager.PlayerUnits.Add(newChar);
    }

    private void SaveRoomInInventory(RoomData data)
    {
        Room newRoom = new Room(data);
        _gameManager.PlayerRooms.Add(newRoom);
    }

    private void SaveItemInInventory(ItemData data)
    {
        Item newItem = Main.Get<DataManager>().ItemCDO[data.Key].Clone();
        newItem.Init(data);
        _gameManager.PlayerItems.Add(newItem);
    }
}
