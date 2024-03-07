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
    private GameManager _gameManager;

    public bool isTutorial { get; set; }
    public bool isPlacingTutorialClear = false; // 배치모드 튜토리얼 클리어했는지 체크.
    public int tutorialIndexX { get; set; } = 1;
    public int tutorialIndexY { get; set; } = 1;

    private float animationDuration = 0.3f;

    public TutorialArrowUI arrowUI { get; set; }

    private Tweener tweener;

    public bool Init()
    {
        _uiManager = Main.Get<UIManager>();
        _dataManager = Main.Get<DataManager>();
        _gameManager = Main.Get<GameManager>();

        isPlacingTutorialClear = false;
        tutorialIndexX = 1;
        tutorialIndexY = 1;

        bool tutorial = PlayerPrefs.HasKey("Tutorial");
        if (!tutorial)
        {
            isTutorial = true;
            _gameManager.ChangeMoney(9000);
        }
        else
        {
            isTutorial = PlayerPrefs.GetInt("Tutorial") == 1 ? false : true;
            int PlayerMoney = isTutorial ? 9000 : 4000; //뒤에거는 1스테이지 클리어한 금액을 더해줘야함 ex 3000 + 1000(1스테 클리어돈)
            _gameManager.ChangeMoney(PlayerMoney);

            if (!isTutorial && !Main.Get<SaveDataManager>().isSaveFileExist)
            {
                Character newChar = new Character(Main.Get<DataManager>().Character["Warrior2"]);
                _gameManager.PlayerUnits.Add(newChar);

                Room newRoom = new Room(Main.Get<DataManager>().Room["Forest2"]);
                _gameManager.PlayerRooms.Add(newRoom);

                _gameManager.CurStage = 1;
            }
        }

        return true;
    }

    public void CreateArrow()
    {
        arrowUI = _uiManager.CreateSubitem<TutorialArrowUI>();
    }

    public TutorialMsg_PopupUI CreateTutorialPopup(string tutorialTextKey, bool isCloseBtnActive = false, bool isBackgroundActive = false) // 튜토리얼 팝업 만드는 함수 + 튜토리얼 내용 연결
    {
        TutorialMsg_PopupUI ui = _uiManager.OpenPopup<TutorialMsg_PopupUI>();
        ui.curTutorialText = _dataManager.Tutorial[tutorialTextKey].Description;
        ui.isCloseBtnActive = isCloseBtnActive;
        ui.isBackgroundActive = isBackgroundActive;

        ui.GetComponent<Canvas>().sortingOrder = arrowUI.GetComponent<Canvas>().sortingOrder + 1;

        return ui;
    }

    public void SetArrowActive(bool activeState) // 화살표 이미지 SetActive 해주는 함수
    {
        arrowUI.gameObject.SetActive(activeState);
    }

    public void SetArrowPosition(float x, float y) // arrow.achoredPosition 값 받아서 화살표 위치 설정해주는 함수
    {
        arrowUI.arrowTransform.anchoredPosition = new Vector3(x, y, 0f);
    }

    public void RotateArrow(float z) // 화살표 z 회전시켜주는 함수
    {
        arrowUI.arrowTransform.Rotate(0f, 0f, z);
    }

    public void SetDOTweenX(float x) // DOTween x 값 받아서 화살표 움직임 설정해주는 함수
    {
        tweener = arrowUI.arrowTransform.DOAnchorPosX(x, animationDuration).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetDOTweenY( float y) // DOTween y 값 받아서 화살표 움직임 설정해주는 함수
    {
        tweener = arrowUI.arrowTransform.DOAnchorPosY(y, animationDuration).SetLoops(-1, LoopType.Yoyo);
    }

    public void KillDOTween() // DOTween Kill 해주는 함수
    {
        if (tweener.IsActive())
        {
            tweener.Kill();
        }
    }    
}
