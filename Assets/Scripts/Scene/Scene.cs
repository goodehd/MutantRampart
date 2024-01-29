using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    protected ResourceManager _resource;
    protected UIManager _ui;
    protected PoolManager _pool;
    protected DataManager _data;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        Main.Get<SceneManager>().Scene = this;
        _resource = Main.Get<ResourceManager>();
        _data = Main.Get<DataManager>();
        _ui = Main.Get<UIManager>();
    }

    public CharacterBehaviour CreateCharacter(string key)
    {
        GameObject obj = _resource.InstantiateWithPoolingOption($"{Literals.UNIT_PREFABS_PATH}{key}");
        CharacterBehaviour character = Utility.GetAddComponent<CharacterBehaviour>(obj);

        character.Init(_data.Character[key]);
        _ui.CreateSubitem<CharacterHpBarUI>("CharacterHpBarUI", obj.transform).transform.localPosition = new Vector3(0, 0.5f, 3f);

        return character;
    }

    public GameObject CreateRoom(string key)
    {
        GameObject obj = _resource.InstantiateWithPoolingOption($"{Literals.ROOM_PREFABS_PATH}{key}");
        RoomBehavior room = Utility.GetAddComponent<RoomBehavior>(obj);
        room.Init(_data.Room[key]);

        return obj;
    }
}
