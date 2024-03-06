using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialArrowUI : BaseUI
{
    public Image arrow { get; set; }
    public RectTransform arrowTransform { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<RectTransform>();

        arrow = GetUI<Image>("TutorialArrowImg");

        arrowTransform = arrow.GetComponent<RectTransform>();
    }
}
