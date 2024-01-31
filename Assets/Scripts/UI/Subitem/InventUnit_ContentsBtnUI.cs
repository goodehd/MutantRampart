using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventUnit_ContentsBtnUI : BaseUI
{
    private Image _unitContentsImg;
    private Button _unitContentsBtn;
    public Image _equipCheckImg { get; private set; }

    public Character UnitData { get; set; }

    public Inventory_PopupUI Owner { get; set; }

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
        if (Owner.inventUnitDescri_PopupUI == null) // 설명창이 안 열려 있다면
        {
            Owner.inventUnitDescri_PopupUI = Main.Get<UIManager>().OpenPopup<InventUnitDescri_PopupUI>("InventUnitDescri_PopupUI"); // 설명창 열어주고
            Owner.inventUnitDescri_PopupUI.UnitData = UnitData; // 데이터 넘겨주고
            Owner.inventUnitDescri_PopupUI.Owner = this; // owner 설정해주고

            _equipCheckImg.gameObject.SetActive(true);
            //if (UnitData)

        }
        else // 설명창이 이미 열려 있다면
        {
            Owner.inventUnitDescri_PopupUI.UnitData = UnitData; // 데이터 넘겨주고
            Owner.inventUnitDescri_PopupUI.SetInfo(); // 데이터 갱신 !

            _equipCheckImg.gameObject.SetActive(true);
        }

    }

    private void HoveredUnitContentBtn(PointerEventData data)
    {
        _unitContentsImg.color = Color.cyan;
    }

    private void ExitUnitContentBtn(PointerEventData data)
    {
        _unitContentsImg.color = Color.white;
    }
}
