using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public class SelectScene_SceneUI : BaseUI
{
    private Image _setPlayerImage;
    private Image _tutorialSkipImage;

    private Button _startNewGameBtn;
    private Button _continueGameBtn;
    private Button _tutorialSkipYesBtn;
    private Button _tutorialSkipNoBtn;
    private Button _upgradeBtn;
    private Button _setPlayerNameCheckBtn;
    private Button _setPlayerCloseBtn;
    private Button _tutorialSkipCloseBtn;

    private TextMeshProUGUI _playerNameTxt;
    private TextMeshProUGUI _playerDayTxt;
    private TextMeshProUGUI _playerGoldTxt;
    private TextMeshProUGUI _inputFieldTxt;

    private bool _saveFile;

    private SaveDataManager _saveDataManager;
    protected override void Init()
    {
        _saveDataManager = Main.Get<SaveDataManager>();
        _gameManager = Main.Get<GameManager>();

        SetUI<Button>();
        SetUI<Image>();
        SetUI<TextMeshProUGUI>();

        _setPlayerImage = GetUI<Image>("SetPlayerImg");
        _tutorialSkipImage = GetUI<Image>("TutorialSkipImg");

        _startNewGameBtn = GetUI<Button>("StartNewBtn");
        _continueGameBtn = GetUI<Button>("ContinueBtn");
        _upgradeBtn = GetUI<Button>("UpgradeBtn");
        _setPlayerNameCheckBtn = GetUI<Button>("SetPlayerNameCheckBtn");
        _setPlayerCloseBtn = GetUI<Button>("SetPlayerCloseBtn");
        _tutorialSkipYesBtn = GetUI<Button>("TutorialSkipYesBtn");
        _tutorialSkipNoBtn = GetUI<Button>("TutorialSkipNoBtn");
        _tutorialSkipCloseBtn = GetUI<Button>("TutorialSkipCloseBtn");

        _playerNameTxt = GetUI<TextMeshProUGUI>("PlayerNameTxt");
        _playerDayTxt = GetUI<TextMeshProUGUI>("PlayerDayTxt");
        _playerGoldTxt = GetUI<TextMeshProUGUI>("PlayerGoldTxt");
        _inputFieldTxt = GetUI<TextMeshProUGUI>("InputFieldTxt");

        SetUICallback(_startNewGameBtn.gameObject, EUIEventState.Click, ClickStartNewBtn);
        SetUICallback(_continueGameBtn.gameObject, EUIEventState.Click, ClickContinueBtn);
        SetUICallback(_setPlayerNameCheckBtn.gameObject, EUIEventState.Click, ClickSetPlayerNameCheckBtn);
        SetUICallback(_setPlayerCloseBtn.gameObject, EUIEventState.Click, ClickSetPlayerCloseBtn);
        SetUICallback(_tutorialSkipYesBtn.gameObject, EUIEventState.Click, ClickTutorialSkipYesBtn);
        SetUICallback(_tutorialSkipNoBtn.gameObject, EUIEventState.Click, ClickTutorialSkipNoBtn);
        SetUICallback(_tutorialSkipCloseBtn.gameObject, EUIEventState.Click, ClickTutorialSkipCloseBtn);


        if (File.Exists(_saveDataManager.path) && !_gameManager.isTutorial)
        {
            _saveFile = true;
            _saveDataManager.LoadData();
            _playerNameTxt.text = _saveDataManager.Player.Name;
            _playerDayTxt.text = _saveDataManager.Player.Curstage.ToString();
            _playerGoldTxt.text = _saveDataManager.Player.PlayerMoney.ToString();
        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다.");
            _playerNameTxt.text = null;
            _playerDayTxt.text = null;
            _playerGoldTxt.text = null;
            _saveFile = false;
        }
    }

    private bool HasValidCharacters(string text)
    {
        return text.Replace(" ", "").Length > 1;
    }

    private void ClickStartNewBtn(PointerEventData data)
    {
        _setPlayerImage.gameObject.SetActive(true);
        
    }

    private void ClickContinueBtn(PointerEventData data)
    {
        // 1. 저장된 데이터가 있을때
        if (_saveFile && !_gameManager.isTutorial)
        {
            Main.Get<SaveDataManager>().isSaveFileExist = true;
            _saveDataManager.LoadData();
            _saveDataManager.LoadMyData();
            Main.Get<SceneManager>().ChangeScene<HongTestScene>();
            //Main.Get<GameManager>().isTutorial = false;
        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다.");
        }
    }

    private void ClickSetPlayerNameCheckBtn(PointerEventData data)
    {
        string text = _inputFieldTxt.text;
        if (text.Length == 1 || !HasValidCharacters(text))
        {
            Debug.Log("입력된 값이 없습니다");
            return;
        }
        _saveDataManager.DeleteData();
        _saveDataManager.Player.Name = _inputFieldTxt.text;
        Main.Get<GameManager>().PlayerName = _saveDataManager.Player.Name;
        _saveDataManager.SaveData();
        if (!Main.Get<GameManager>().isTutorial)
        {
            _setPlayerImage.gameObject.SetActive(false);
            _tutorialSkipImage.gameObject.SetActive(true);
        }
        else
        {
            SetTutorial();
        }
    }

    private void ClickTutorialSkipYesBtn(PointerEventData data)
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        Main.Get<GameManager>().Init();
        SetTutorial();
    }
    private void ClickTutorialSkipNoBtn(PointerEventData data)
    {
        PlayerPrefs.SetInt("Tutorial", 0);
        Main.Get<GameManager>().Init();
        SetTutorial();
    }
    private void ClickTutorialSkipCloseBtn(PointerEventData data)
    {
        _tutorialSkipImage.gameObject.SetActive(false);
    }

    private void ClickSetPlayerCloseBtn(PointerEventData data)
    {
        _setPlayerImage.gameObject.SetActive(false);
    }

    private void SetTutorial()
    {
        _setPlayerImage.gameObject.SetActive(false);
        Main.Get<GameManager>().AddHometoInventory();
        Main.Get<SceneManager>().ChangeScene<HongTestScene>();
    }

}


