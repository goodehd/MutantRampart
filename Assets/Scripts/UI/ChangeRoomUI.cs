using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChangeRoomUI : MonoBehaviour
{
    public List<GameObject> imageObjects = new List<GameObject>();
    public Dictionary<string, Sprite> images = new Dictionary<string,Sprite>();
    public Dictionary<string, Button> imageButtons = new Dictionary<string, Button>();
    public Dictionary<string, RoomData> roomDatas = new Dictionary<string, RoomData>();
    
    public CSVReader csvReader = new CSVReader();

    //일단 손으로 UI넣어주기
    public TMP_Text name;
    public TMP_Text type;
    public TMP_Text Instruction;
    public Image roomimage;
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

    public void OnclickImage(string a)
    {
        roomimage.sprite = images[a];
        name.text = $"이름 : {roomDatas[a].Key}";
        type.text = $"타입 : {roomDatas[a].Type.ToString()}";
        Instruction.text = $"설명 : {roomDatas[a].Instruction}";
    }
    
}
