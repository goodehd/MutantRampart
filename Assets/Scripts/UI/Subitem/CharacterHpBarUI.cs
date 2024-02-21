using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterHpBarUI : BaseUI
{
    private Image _hpBarImage;
    private Image _hpBaseBarImage;
    private Image _mpBarImage;

    private TextMeshProUGUI _curHpText;
    private TextMeshProUGUI _maxHpText;

    private CharacterBehaviour _owner;
    private Coroutine _coroutine;

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<TextMeshProUGUI>();

        _hpBarImage = GetUI<Image>("HpBar");
        _hpBaseBarImage = GetUI<Image>("HpBaseBar");
        _mpBarImage = GetUI<Image>("MpBar");

        _curHpText = GetUI<TextMeshProUGUI>("CurHPText");
        _maxHpText = GetUI<TextMeshProUGUI>("MaxHPText");

        _owner = transform.parent.GetComponent<CharacterBehaviour>();

        if (_owner != null && _owner.Status != null)
        {
            _owner.Status.GetStat<Vital>(EstatType.Hp).OnValueChanged += SetMaxHpInfo;
            _owner.Status.GetStat<Vital>(EstatType.Hp).OnCurValueChanged += SetHpInfo;
            _owner.Status.GetStat<Vital>(EstatType.Mp).OnCurValueChanged += SetMpInfo;
            SetHpInfo(_owner.Status.GetStat<Vital>(EstatType.Hp).CurValue);
            SetMpInfo(_owner.Status.GetStat<Vital>(EstatType.Mp).CurValue);

            _curHpText.text = ((int)_owner.Status.GetStat<Vital>(EstatType.Hp).CurValue).ToString();
            _maxHpText.text = ((int)_owner.Status.GetStat<Vital>(EstatType.Hp).Value).ToString();
        }

        if (_owner.Status.GetStat<Vital>(EstatType.Mp).Value == 0)
        {
            _mpBarImage.transform.parent.gameObject.SetActive(false);
        }
    }

    private void SetHpInfo(float hp)
    {
        float ratio = _owner.Status.GetStat<Vital>(EstatType.Hp).Normalized();
        _curHpText.text = ((int)hp).ToString();
        _hpBaseBarImage.fillAmount = _hpBarImage.fillAmount;
        _hpBarImage.fillAmount = ratio;

        if(_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(BaseBarAnimation());
    }

    private void SetMaxHpInfo(float hp)
    {
        _maxHpText.text = ((int)_owner.Status.GetStat<Vital>(EstatType.Hp).Value).ToString();
        SetHpInfo((int)_owner.Status.GetStat<Vital>(EstatType.Hp).CurValue);
    }

    private IEnumerator BaseBarAnimation()
    {
        while (_hpBaseBarImage.fillAmount >= _hpBarImage.fillAmount)
        {
            _hpBaseBarImage.fillAmount -= Time.deltaTime / 5f;
            yield return null;
        }
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
