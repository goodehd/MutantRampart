using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GachaResult_PopupUI : BaseUI
{
    private Transform gachaResultContent;
    private Button closeButton;
    private GameManager gameManager;

    private bool isArrowRotated = false; // close 버튼 누를 때마다 화살표 회전되는거 방지.

    // Data
    public List<CharacterData> GachaUnitData { get; set; }
    public List<RoomData> GachaRoomData { get; set; }
    public List<ItemData> GachaItemData { get; set; }
    public List<ItemData> ShopGroundData { get; set; }
    public Shop_PopupUI Owner { get; set; }

    protected override void Init()
    {
        SetUI<Transform>();
        SetUI<Button>();

        gameManager = Main.Get<GameManager>();

        gachaResultContent = GetUI<Transform>("GachaResult_Content");

        closeButton = GetUI<Button>("GachaResultCloseBtn");
        SetUICallback(closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);

        SetResultImgUInfo();

        if (gameManager.playerUnits.Count >= 3 && gameManager.PlayerRooms.Count >= 3)
        {
            isArrowRotated = true;
        }
    }

    private void ClickCloseBtn(PointerEventData eventData)
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

        Main.Get<UIManager>().ClosePopup();

        if (gameManager.isTutorial)
        {
            if (gameManager.playerUnits.Count >= 3)
            {
                if (Owner.tweener.IsActive())
                {
                    Owner.tweener.Kill(); // room 가리키는 화살표 Kill.
                }

                if (gameManager.PlayerRooms.Count == 1)
                {
                    TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
                    ui.curTutorialText = "잘하셨어요!\n<color=#E9D038><b>Room</b></color> 을 클릭해서\n 동일하게 <color=#E9D038><b>3회 뽑기</b></color>를 진행해주세요!\n\n튜토리얼이 끝나면 Room 을 배치하는 Ground 와\n유닛의 능력치를 올려주는 Item 도 구매할 수 있어요!";
                    ui.isCloseBtnActive = true;
                    ui.isBackgroundActive = true;

                    Owner.shopArrowImg.gameObject.SetActive(true);
                    Owner.shopArrowTransform.anchoredPosition = new Vector3(-628f, 368f, 0f); // 상점 내 Room 카테고리 가리키는 화살표.
                    Owner.tweener = Owner.shopArrowTransform.DOAnchorPosY(338f, Owner.animationDuration).SetLoops(-1, LoopType.Yoyo);
                }
            }

            if (gameManager.playerUnits.Count >= 3 && gameManager.PlayerRooms.Count >= 4)
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

    private void SaveUnitInInventory(CharacterData data)
    {
        Character newChar = new Character(data);
        Main.Get<GameManager>().playerUnits.Add(newChar);
    }

    private void SaveRoomInInventory(RoomData data)
    {
        Room newRoom = new Room(data);
        Main.Get<GameManager>().PlayerRooms.Add(newRoom);
    }

    private void SaveItemInInventory(ItemData data)
    {
        Item newItem = Main.Get<DataManager>().ItemCDO[data.Key].Clone();
        newItem.Init(data);
        Main.Get<GameManager>().PlayerItems.Add(newItem);
        Debug.Log(newItem.GetType().Name);
    }
}
