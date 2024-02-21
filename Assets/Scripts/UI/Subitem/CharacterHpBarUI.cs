using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterHpBarUI : BaseUI
{
    private Image _hpBarImage;
    private Image _mpBarImage;
    private CharacterBehaviour _owner;

    protected override void Init()
    {
        SetUI<Image>();

        _hpBarImage = GetUI<Image>("HpBar");
        _mpBarImage = GetUI<Image>("MpBar");

        _owner = transform.parent.GetComponent<CharacterBehaviour>();

        if (_owner != null && _owner.Status != null)
        {
            _owner.Status.GetStat<Vital>(EstatType.Hp).OnCurValueChanged += SetHpInfo;
            _owner.Status.GetStat<Vital>(EstatType.Mp).OnCurValueChanged += SetMpInfo;
            SetHpInfo(_owner.Status.GetStat<Vital>(EstatType.Hp).CurValue);
            SetMpInfo(_owner.Status.GetStat<Vital>(EstatType.Mp).CurValue);
        }

        if(_owner.Status.GetStat<Vital>(EstatType.Mp).Value == 0)
        {
            _mpBarImage.transform.parent.gameObject.SetActive(false);
        }
    }

    private void SetHpInfo(float hp)
    {
        float ratio = _owner.Status.GetStat<Vital>(EstatType.Hp).Normalized();
        _hpBarImage.fillAmount = ratio;
    }

    private void SetMpInfo(float mp)
    {
        float ratio = _owner.Status.GetStat<Vital>(EstatType.Mp).Normalized();
        _mpBarImage.fillAmount = ratio;
    }

    private void OnDestroy()
    {
        if(_owner != null)
        {
            _owner.Status.GetStat<Vital>(EstatType.Hp).OnCurValueChanged -= SetHpInfo;
            _owner.Status.GetStat<Vital>(EstatType.Mp).OnCurValueChanged -= SetMpInfo;
        }
    }
}
