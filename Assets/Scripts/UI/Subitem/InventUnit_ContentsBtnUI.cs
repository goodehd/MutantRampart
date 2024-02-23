using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventUnit_ContentsBtnUI : BaseUI
{
    private Image _unitContentsImg;
    private Button _unitContentsBtn;
    public Image _selectCheckImg { get; private set; }

    private TextMeshProUGUI _equipText;

    public Character UnitData { get; set; }

    public Inventory_PopupUI Owner { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();
        SetUI<TextMeshProUGUI>();

        _unitContentsImg = GetUI<Image>("InventUnit_ContentsBtnUI");
        _unitContentsBtn = GetUI<Button>("InventUnit_ContentsBtnUI");
        _selectCheckImg = GetUI<Image>("InventUnitEquipCheckImg");

        _equipText = GetUI<TextMeshProUGUI>("EquipText");

        SetUICallback(_unitContentsBtn.gameObject, EUIEventState.Click, ClickUnitContentBtn);
        SetUICallback(_unitContentsBtn.gameObject, EUIEventState.Hovered, HoveredUnitContentBtn);
        SetUICallback(_unitContentsBtn.gameObject, EUIEventState.Exit, ExitUnitContentBtn);

        SetInfo();
    }

    private void SetInfo()
    {
        _unitContentsImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{UnitData.Data.Key}");
        _equipText.gameObject.SetActive(UnitData.CurRoom != null);
    }

    private void ClickUnitContentBtn(PointerEventData data)
    {
        if (Owner.inventUpgrade_PopupUI != null) // 업그레이드창이 열려있다면
        {
            if (Owner.inventUpgrade_PopupUI.Count >= 3) // 3개 가득 찬 경우 예외처리 해주기
            {
                Error_PopupUI ui = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI");
                ui.curErrorText = "슬롯이 가득 찼습니다!";
                return;
            }

            if (UnitData.Data.NextKey != "")
            {
                Owner.inventUpgrade_PopupUI.AddUpgradeUnitSlot(UnitData);
            }
        }
        else if (Owner.inventUnitDescri_PopupUI == null) // 설명창이 안 열려 있다면
        {
            Owner.inventUnitDescri_PopupUI = Main.Get<UIManager>().OpenPopup<InventUnitDescri_PopupUI>("InventUnitDescri_PopupUI"); // 설명창 열어주고
            Owner.inventUnitDescri_PopupUI.UnitData = UnitData; // 데이터 넘겨주고
            Owner.inventUnitDescri_PopupUI.Owner = this; // owner 설정해주고

            _selectCheckImg.gameObject.SetActive(true);
        }
        else                                            // 설명창이 이미 열려 있다면
        {
            Owner.inventUnitDescri_PopupUI.Owner._selectCheckImg.gameObject.SetActive(false); // 선택표시가 기존에 활성화되어있다면 일단 꺼준다.

            Owner.inventUnitDescri_PopupUI.UnitData = UnitData; // 데이터 넘겨주고
            Owner.inventUnitDescri_PopupUI.SetInfo(); // 데이터 갱신 !
            Owner.inventUnitDescri_PopupUI.Owner = this; // owner 업데이트

            _selectCheckImg.gameObject.SetActive(true); // 그리고 다시 선택표시가 active 해주기.
        }

    }

    private void HoveredUnitContentBtn(PointerEventData data)
    {
        if(UnitData.Owner != null)
        {
            Vector2 pos = UnitData.CurRoom.transform.position + Literals.BatPos[UnitData.CurIndex];
            pos.x += 0.1f;
            pos.y += 0.7f;
            ((HongTestScene)Main.Get<SceneManager>().Scene).ActiveUnitArrow(pos);
        }
        _unitContentsImg.color = Color.cyan;
    }

    private void ExitUnitContentBtn(PointerEventData data)
    {
        if (UnitData.Owner != null)
        {
            ((HongTestScene)Main.Get<SceneManager>().Scene).InActiveUnitArrow();
        }
        _unitContentsImg.color = Color.white;
    }
}
