using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// # 리터럴즈 (상수형 데이터)
/// </summary>
public static class Literals 
{
    public const string UI_SCENE_PATH = "Prefabs/UI/Scene/";
    public const string UI_POPUP_PATH = "Prefabs/UI/Popup/";
    public const string UI_SUBITEM_PATH = "Prefabs/UI/Subitem/";

    public const string UNIT_PREFABS_PATH = "Prefabs/Character/";
    public const string UNIT_SPRITE_PATH = "Sprites/Unit/";

    public const string ROOM_PREFABS_PATH = "Prefabs/Room/";
    public const string ROOM_SPRITES_PATH = "Sprites/Room/";

    public const string ITEM_SPRITE_PATH = "Sprites/Item/";

    public const string FX_PATH = "Prefabs/Fx/";

    public static readonly int Idle = Animator.StringToHash("IsIdle");
    public static readonly int Move = Animator.StringToHash("IsMove");
    public static readonly int Attack = Animator.StringToHash("IsAttack");
    public static readonly int Dead = Animator.StringToHash("IsDead");
    public static readonly int Skill = Animator.StringToHash("IsSkill");
    public static readonly int StageStart = Animator.StringToHash("StageStart");
    public static readonly int StageEnd = Animator.StringToHash("StageEnd");

    public static readonly List<Vector3> BatPos = new List<Vector3>
    {
        new Vector3(-0.1f, 2.3f, 3.0f),
        new Vector3(0.4f, 2.05f, 3.0f),
        new Vector3(0.9f, 1.8f, 3.0f)
    };

    public static readonly List<Vector3> EnemyPos = new List<Vector3>
    {
        new Vector3(-1.1f, 1.8f, 3.0f),
        new Vector3(-0.85f, 1.65f, 3.0f),
        new Vector3(-0.6f, 1.6f, 3.0f),
        new Vector3(-0.4f, 1.4f, 3.0f),
        new Vector3(-0.15f, 1.25f, 3.0f),
        new Vector3(0.12f, 1.1f, 3.0f)
    };
    //public static readonly List<Vector3> TrapEnemyPos = new List<Vector3>
    //{
    //    new Vector3(0.7f, 2.2f, 3.0f),
    //    new Vector3(0.5f, 1.9f, 3.0f),
    //    new Vector3(0f, 1.6f, 3.0f),
    //    new Vector3(-0.5f, 1.4f, 3.0f),
    //    new Vector3(0f, 1.75f, 3.0f),
    //    new Vector3(0.12f, 1.1f, 3.0f)
    //};
    public static readonly List<Vector3> TrapEnemyPos = new List<Vector3>
    {
        new Vector3(-0.3f, 2.2f, 3.0f),
        new Vector3(1.5f, 1.9f, 3.0f),
        new Vector3(0f, 1.6f, 3.0f),
        new Vector3(-2.5f, 1.4f, 3.0f),
        new Vector3(-2.0f, 1.75f, 3.0f),
        new Vector3(0.12f, 1.1f, 3.0f)
    };
}
