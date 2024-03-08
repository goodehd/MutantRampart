using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextUI : BaseUI
{
    private TextMeshProUGUI _damageText;
    private ParabolicMovement _moveMent;

    private float _value;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _textSize = Vector3.zero;

    protected override void Init()
    {
        base.Init();
        SetUI<TextMeshProUGUI>();

        _damageText = GetUI<TextMeshProUGUI>("DamageText");
        _damageText.text = ((int)_value).ToString();

        _moveMent = _damageText.GetComponent<ParabolicMovement>();

        _moveMent.OnMovementEnd += MoveMentEnd;

        float scale = Normalize(_value);
        _textSize.Set(scale, scale, 1);

        transform.localScale = _textSize;
        transform.position = _startPosition;
        StartMovement(_startPosition, _endPosition);
    }

    public void StartMovement(Vector3 startPos, Vector3 endPos)
    {
        _moveMent.SetPos(startPos, endPos);
        _moveMent.MovementStart();
    }

    public void SetColor(Color color)
    {
        _damageText.color = color;
    }

    public void SetText(float value)
    {
        _value = value;
    }

    public void SetPos(Vector3 startPos, Vector3 endPos)
    {
        _startPosition = startPos;
        _endPosition = endPos;
    }

    private void MoveMentEnd()
    {
        _ui.DestroySubItem(this.gameObject);
    }

    float Normalize(float value)
    {
        if(value > Literals.MaxDamageTextSizeValue)
        {
            return 1.0f;
        }

        if(value < Literals.MinDamageTextSizeValue)
        {
            return 0.5f;
        }

        float range = Literals.MaxDamageTextSizeValue - Literals.MinDamageTextSizeValue;
        float ratio = (value - Literals.MinDamageTextSizeValue) / range;
        float normalizedValue = 0.5f + 0.5f * ratio;
        return normalizedValue;
    }
}
