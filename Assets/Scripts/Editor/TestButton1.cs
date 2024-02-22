using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HongTestScene))]
public class TestButton1 : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HongTestScene generator = (HongTestScene)target;
        
        if (GUILayout.Button("Gun"))
        {
            Character newChar = new Character(Main.Get<DataManager>().Character["Gun"]);
            Main.Get<GameManager>().PlayerUnits.Add(newChar);
        }

        if (GUILayout.Button("Jotem"))
        {
            Character newChar = new Character(Main.Get<DataManager>().Character["Jotem"]);
            Main.Get<GameManager>().PlayerUnits.Add(newChar);
        }

        if (GUILayout.Button("Warrior"))
        {
            Character newChar = new Character(Main.Get<DataManager>().Character["Warrior"]);
            Main.Get<GameManager>().PlayerUnits.Add(newChar);
        }

        if (GUILayout.Button("Cleric"))
        {
            Character newChar = new Character(Main.Get<DataManager>().Character["Cleric"]);
            Main.Get<GameManager>().PlayerUnits.Add(newChar);
        }

        if (GUILayout.Button("Knight"))
        {
            Character newChar = new Character(Main.Get<DataManager>().Character["Knight"]);
            Main.Get<GameManager>().PlayerUnits.Add(newChar);
        }

        if (GUILayout.Button("Kunoichi"))
        {
            Character newChar = new Character(Main.Get<DataManager>().Character["Kunoichi"]);
            Main.Get<GameManager>().PlayerUnits.Add(newChar);
        }

        if (GUILayout.Button("Priest"))
        {
            Character newChar = new Character(Main.Get<DataManager>().Character["Priest"]);
            Main.Get<GameManager>().PlayerUnits.Add(newChar);
        }

        if (GUILayout.Button("Shaman"))
        {
            Character newChar = new Character(Main.Get<DataManager>().Character["Shaman"]);
            Main.Get<GameManager>().PlayerUnits.Add(newChar);
        }

        // room
        if (GUILayout.Button("Forest"))
        {
            Room newRoom = new Room(Main.Get<DataManager>().Room["Forest"]);
            Main.Get<GameManager>().PlayerRooms.Add(newRoom);
        }

        if (GUILayout.Button("Igloo"))
        {
            Room newRoom = new Room(Main.Get<DataManager>().Room["Igloo"]);
            Main.Get<GameManager>().PlayerRooms.Add(newRoom);
        }

        if (GUILayout.Button("Lava"))
        {
            Room newRoom = new Room(Main.Get<DataManager>().Room["Lava"]);
            Main.Get<GameManager>().PlayerRooms.Add(newRoom);
        }

        if (GUILayout.Button("Livingroom"))
        {
            Room newRoom = new Room(Main.Get<DataManager>().Room["Livingroom"]);
            Main.Get<GameManager>().PlayerRooms.Add(newRoom);
        }

        if (GUILayout.Button("Snow"))
        {
            Room newRoom = new Room(Main.Get<DataManager>().Room["Snow"]);
            Main.Get<GameManager>().PlayerRooms.Add(newRoom);
        }

        if (GUILayout.Button("Temple"))
        {
            Room newRoom = new Room(Main.Get<DataManager>().Room["Temple"]);
            Main.Get<GameManager>().PlayerRooms.Add(newRoom);
        }

        //money
        if (GUILayout.Button("money"))
        {
            Main.Get<GameManager>().ChangeMoney(10000);
        }
    }
}

[CustomEditor(typeof(DefaultTile))]
public class TestButton3 : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DefaultTile generator = (DefaultTile)target;

        if (GUILayout.Button("RightTop"))
        {
            bool test = generator.IsDoorOpen(ERoomDir.RightTop);
            Main.Get<TileManager>().SetRoomDir(generator, ERoomDir.RightTop, !test);
        }

        if (GUILayout.Button("RightBottom"))
        {
            Main.Get<TileManager>().SetRoomDir(generator, ERoomDir.RightBottom, !generator.IsDoorOpen(ERoomDir.RightBottom));
        }

        if (GUILayout.Button("LeftTop"))
        {
            Main.Get<TileManager>().SetRoomDir(generator, ERoomDir.LeftTop, !generator.IsDoorOpen(ERoomDir.LeftTop));
        }

        if (GUILayout.Button("LeftBottom"))
        {
            Main.Get<TileManager>().SetRoomDir(generator, ERoomDir.LeftBottom, !generator.IsDoorOpen(ERoomDir.LeftBottom));
        }
    }
}

