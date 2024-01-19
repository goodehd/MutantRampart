using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChangeRoomUI : BaseUI
{
    public Room selectRoom;
    private RoomData selectRoomData;

    /*
    public List<GameObject> imageObjects = new List<GameObject>();
    public Dictionary<string, Sprite> images = new Dictionary<string,Sprite>();
    public Dictionary<string, Button> imageButtons = new Dictionary<string, Button>();
    
    public Dictionary<string, RoomData> roomDatas = new Dictionary<string, RoomData>();
    public CSVReader csvReader = new CSVReader();

    public string selectRoomName;
*/
    //일단 손으로 UI넣어주기
    private TextMeshProUGUI name;
    private TextMeshProUGUI type;
    private TextMeshProUGUI Instruction;
    private Image roomimage;
    private Button equipButton;
    private Transform _content;
    
    /*
    private void Awake()
    {
        
        //TODO 이미지 오브젝트를 넣어줘야함
        roomDatas = csvReader.LoadToCSVData<RoomData>();
        foreach (var image in imageObjects)
        {
            imageButtons.Add(image.name, image.GetComponent<Button>());
            images.Add(image.name, image.GetComponent<Image>().sprite);
            imageButtons[image.name].onClick.AddListener(() => OnclickImage(image.name));
        }
    }
*/
    protected override void Init()
    {
        SetUI<TextMeshProUGUI>();
        SetUI<Image>();
        SetUI<Transform>();
        SetUI<Button>();
        name = GetUI<TextMeshProUGUI>("NameText");
        type = GetUI<TextMeshProUGUI>("TypeText");
        Instruction = GetUI<TextMeshProUGUI>("InstructionText");
        roomimage = GetUI<Image>("RoomImagesprite");
        _content = GetUI<Transform>("Content");
        equipButton = GetUI<Button>("EquipButton");
        SetUICallback(equipButton.gameObject, EUIEventState.Click, ClickEquipButton);

        List<RoomData> playerrooms = Main.Get<GameManager>().playerRooms;
        
        for (int i = 0; i < playerrooms.Count; i++)
        {
            RoomSelectImage roomSelectImage = Main.Get<UIManager>().CreateSubitem<RoomSelectImage>("RoomSelectImage",_content);

            roomSelectImage.roomData = playerrooms[i];
            roomSelectImage.owner = this;
        }
        
    }
    public void SetSelectRoomInfo(RoomData roomData, Sprite sprite)
    {
        name.text = $"이름 : {roomData.Key}";
        type.text = $"타입 : {roomData.Type.ToString()}";
        Instruction.text = $"설명 : {roomData.Instruction}";
        roomimage.sprite = sprite;
        selectRoomData = roomData;

    }           
    
    private void ClickEquipButton(PointerEventData eventData)
    {
        Vector3 pos = selectRoom.transform.position;
        Main.Get<ResourceManager>().Destroy(selectRoom.gameObject);
        GameObject obj = Main.Get<ResourceManager>().InstantiateWithPoolingOption($"Prefabs/Room/{selectRoomData.Key}");
        obj.transform.position = pos;

    }

    /*
    public void OnclickImage(string a)
    {
        selectRoomName = a;
        roomimage.sprite = images[a];
        name.text = $"이름 : {roomDatas[a].Key}";
        type.text = $"타입 : {roomDatas[a].Type.ToString()}";
        Instruction.text = $"설명 : {roomDatas[a].Instruction}";
    }

    */
}
