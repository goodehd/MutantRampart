using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterHpBarUI : BaseUI
{
    private Image _barImage;
    private Character _owner;

    protected override void Init()
    {
        SetUI<Image>();

        _barImage = GetUI<Image>("HpBar");

        _owner = transform.parent.GetComponent<Character>();

        if (_owner != null)
        {
            _owner.Status.GetStat<Vital>(EstatType.Hp).OnCurValueChanged += SetInfo;
        }
    }

    private void SetInfo(float hp)
    {
        float ratio = _owner.Status.GetStat<Vital>(EstatType.Hp).Normalized();
        _barImage.fillAmount = ratio;
    }

    private void OnDestroy()
    {
        _owner.Status.GetStat<Vital>(EstatType.Hp).OnCurValueChanged -= SetInfo;
    }
}
