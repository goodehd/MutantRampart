using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventUnit_ContentsBtnUI : BaseUI
{
    private Image _unitContentsImg;
    private Button _unitContentsBtn;
    public Image _equipCheckImg { get; private set; }

    public Character UnitData { get; set; }

    //public Inventory_PopupUI inventoryPopupUIOwner { get; set; }

    public bool isUnitContentPressed { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();

        _unitContentsImg = GetUI<Image>("InventUnit_ContentsBtnUI");
        _unitContentsBtn = GetUI<Button>("InventUnit_ContentsBtnUI");
        _equipCheckImg = GetUI<Image>("InventUnitEquipCheckImg");

        SetUICallback(_unitContentsBtn.gameObject, EUIEventState.Click, ClickUnitContentBtn);
        SetUICallback(_unitContentsBtn.gameObject, EUIEventState.Hovered, HoveredUnitContentBtn);
        SetUICallback(_unitContentsBtn.gameObject, EUIEventState.Exit, ExitUnitContentBtn);

        SetInfo();
    }

    private void SetInfo()
    {
        _unitContentsImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{UnitData.Data.Key}");
    }

    private void ClickUnitContentBtn(PointerEventData data)
    {
        if (isUnitContentPressed) // 버튼을 눌러 설명창이 기존에 열려있다면 리턴시키고,
        {
            return;
        }
        else if (!isUnitContentPressed) // 버튼을 눌러 설명창이 기존에 안 열려있다면
        {
            _equipCheckImg.gameObject.SetActive(true);
            isUnitContentPressed = true;

            InventUnitDescri_PopupUI ui = Main.Get<UIManager>().OpenPopup<InventUnitDescri_PopupUI>("InventUnitDescri_PopupUI"); // 설명창 열어주고
            ui.UnitData = UnitData; // 데이터 넘겨주고
            ui.Owner = this; // owner 설정해주고
            //ui.inventoryPopupUIOwner = inventoryPopupUIOwner;
        }

    }

    private void HoveredUnitContentBtn(PointerEventData data)
    {
        _unitContentsImg.color = Color.cyan;
        //_equipCheckImg.gameObject.SetActive(true);
    }

    private void ExitUnitContentBtn(PointerEventData data)
    {
        _unitContentsImg.color = Color.white;

        //_equipCheckImg.gameObject.SetActive(false);
    }
}
