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

    public static readonly int Idle = Animator.StringToHash("IsIdle");
    public static readonly int Move = Animator.StringToHash("IsMove");
    public static readonly int Attack = Animator.StringToHash("IsAttack");
    public static readonly int Dead = Animator.StringToHash("IsDead");
}
