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

    protected virtual void OnApplicationQuit()
    {
    }

    protected virtual void Init()
    {
        Main.Get<SceneManager>().Scene = this;
        _resource = Main.Get<ResourceManager>();
        _data = Main.Get<DataManager>();
        _ui = Main.Get<UIManager>();
        _pool = Main.Get<PoolManager>();
    }

    public CharacterBehaviour CreateCharacter(string key)
    {
        CharacterData info = _data.Character[key];
        GameObject obj = _resource.Instantiate($"{Literals.UNIT_PREFABS_PATH}{info.PrefabName}");
        CharacterBehaviour character = Utility.GetAddComponent<CharacterBehaviour>(obj);
        character.Init(info);
        return character;
    }

    public GameObject CreateRoom(string key)
    {
        RoomData data = _data.Room[key];
        GameObject obj = _resource.Instantiate($"{Literals.ROOM_PREFABS_PATH}{data.PrefabName}");
        RoomBehavior room = Utility.GetAddComponent<RoomBehavior>(obj);
        room.Init(data);

        return obj;
    }

}
