using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public class SelectScene_SceneUI : BaseUI
{
    private Image _setPlayerNameImage;
    private Image _playerInfoImage;

    private Button _startNewGameBtn;
    private Button _continueGameBtn;
    private Button _upgradeBtn;
    private Button _setPlayerNameCheckBtn;

    private TextMeshProUGUI _playerNameTxt;
    private TextMeshProUGUI _playerDayTxt;
    private TextMeshProUGUI _playerGoldTxt;
    private TextMeshProUGUI _inputFieldTxt;

    private bool _saveFile;

    private SaveDataManager _saveDataManager;
    protected override void Init()
    {
        _saveDataManager = Main.Get<SaveDataManager>();

        SetUI<Button>();
        SetUI<Image>();
        SetUI<TextMeshProUGUI>();

        _setPlayerNameImage = GetUI<Image>("SetPlayerNameImg");
        _playerInfoImage = GetUI<Image>("PlayerInfoImg");

        _startNewGameBtn = GetUI<Button>("StartNewBtn");
        _continueGameBtn = GetUI<Button>("ContinueBtn");
        _upgradeBtn = GetUI<Button>("UpgradeBtn");
        _setPlayerNameCheckBtn = GetUI<Button>("SetPlayerNameCheckBtn");

        _playerNameTxt = GetUI<TextMeshProUGUI>("PlayerNameTxt");
        _playerDayTxt = GetUI<TextMeshProUGUI>("PlayerDayTxt");
        _playerGoldTxt = GetUI<TextMeshProUGUI>("PlayerGoldTxt");
        _inputFieldTxt = GetUI<TextMeshProUGUI>("InputFieldTxt");

        SetUICallback(_startNewGameBtn.gameObject, EUIEventState.Click, ClickStartNewBtn);
        SetUICallback(_continueGameBtn.gameObject, EUIEventState.Click, ClickContinueBtn);
        SetUICallback(_setPlayerNameCheckBtn.gameObject, EUIEventState.Click, ClickSetPlayerNameCheckBtn);


        if (File.Exists(_saveDataManager.path))
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
            _saveFile = false;
        }
    }

    private void ClickStartNewBtn(PointerEventData data)
    {
        _saveDataManager.ClearData();
        _setPlayerNameImage.gameObject.SetActive(true);
        
    }

    private void ClickContinueBtn(PointerEventData data)
    {
        // 1. 저장된 데이터가 있을때
        if (_saveFile)
        {
            _saveDataManager.LoadData();
            SetGameData();
            Main.Get<SceneManager>().ChangeScene<HongTestScene>();
            Main.Get<GameManager>().isTutorial = false;
        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다.");
        }
    }

    private void ClickSetPlayerNameCheckBtn(PointerEventData data)
    {
        if (_inputFieldTxt == null) return;
        _saveDataManager.DeleteData();
        _saveDataManager.Player.Name = _inputFieldTxt.text;
        _saveDataManager.Player.PlayerMoney = 15000;
        _saveDataManager.Player.Curstage = 0;
        SetGameData();
        _saveDataManager.SaveData();
        Debug.Log($"{_saveDataManager.Player.Name}");
        Main.Get<SceneManager>().ChangeScene<HongTestScene>();
    }

    private void SetGameData()
    {
        Main.Get<GameManager>().PlayerMoney = _saveDataManager.Player.PlayerMoney;
        Main.Get<GameManager>().CurStage = _saveDataManager.Player.Curstage;
    }
}


