using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : IManagers
{
    private UIManager _uiManager;
    private DataManager _dataManager;

    private float animationDuration = 0.3f;

    public bool Init()
    {
        _uiManager = Main.Get<UIManager>();
        _dataManager = Main.Get<DataManager>();

        return true;
    }

    public TutorialMsg_PopupUI CreateTutorialPopup(string tutorialTextKey, bool isCloseBtnActive = false, bool isBackgroundActive = false) // 튜토리얼 팝업 만드는 함수 + 튜토리얼 내용 연결
    {
        TutorialMsg_PopupUI ui = _uiManager.OpenPopup<TutorialMsg_PopupUI>();
        ui.curTutorialText = _dataManager.Tutorial[tutorialTextKey].Description;
        ui.isCloseBtnActive = isCloseBtnActive;
        ui.isBackgroundActive = isBackgroundActive;

        return ui;
    }
    
    public void SetArrowActive(Image image, bool activeState) // 화살표 이미지 SetActive 해주는 함수
    {
        image.gameObject.SetActive(activeState);
    }
    
    public void SetArrowPosition(RectTransform arrowTransform, float x, float y) // arrow.achoredPosition 값 받아서 화살표 위치 설정해주는 함수
    {
        arrowTransform.anchoredPosition = new Vector3(x, y, 0f);
    }

    public void RotateArrow(RectTransform arrowTransform, float z) // 화살표 z 회전시켜주는 함수
    {
        arrowTransform.Rotate(0f, 0f, z);
    }

    public Tweener SetDOTweenX(RectTransform arrowTransform, float x) // DOTween x 값 받아서 화살표 움직임 설정해주는 함수
    {
        return arrowTransform.DOAnchorPosX(x, animationDuration).SetLoops(-1, LoopType.Yoyo);
    }

    public Tweener SetDOTweenY(RectTransform arrowTransform, float y) // DOTween y 값 받아서 화살표 움직임 설정해주는 함수
    {
        return arrowTransform.DOAnchorPosY(y, animationDuration).SetLoops(-1, LoopType.Yoyo);
    }    

    public void KillDOTween(Tweener tweener) // DOTween Kill 해주는 함수
    {
        if (tweener.IsActive())
        {
            tweener.Kill();
        }
    }
}
