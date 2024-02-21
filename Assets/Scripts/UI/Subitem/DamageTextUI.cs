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

    protected override void Init()
    {
        base.Init();
        SetUI<TextMeshProUGUI>();

        _damageText = GetUI<TextMeshProUGUI>("DamageText");
        _damageText.text = ((int)_value).ToString();

        _moveMent = _damageText.GetComponent<ParabolicMovement>();

        _moveMent.OnMovementEnd += MoveMentEnd;

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
}
