using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameConfig : MonoBehaviour
{

    [Header("WARNING")]
    public static string CANT_CRAFT_WARNING = "CAN'T CRAFT!";
    public static string INVENTORY_FULL_WARNING = "INVENTORY FULL!";

    public static string NOT_ENOUGH_ITEM_WARNING = "NOT ENOUGH ITEM!";
    public static string HORIZONTAL = "Horizontal";

    public static string VERTICAL = "Vertical";
    public static String PLAYER_TAG0 = "Player";
    public static String DESTROYABLE_OBJECT_TAG = "DestroyObject";

    public static String ENEMY_TAG = "Enemy";

    public static string GAMEMANAGER_TAG = "GameManager";

    public static String HITBOX_ENEMY = "HitBox_Enemy";

    public static string SPEED_PARAMETER = "Speed";

    public static string HITBOX_PUNCH = "HitBox_Punch";

    public static string ITEM_MASK = "Item";

    public static string OBJECT_MASK = "ObjectLayer";

    public static string PUNCH_TRIGGER = "Punch";

    public static string LOOT_TRIGGER = "Loot";
    public static string MOVEX_FLOAT = "MoveX";
    public static string MOVEY_FLOAT = "MoveY";
    public static string DIRX_FLOAT = "DirX";
    public static string DIRY_FLOAT = "DirY";

    public static string USINGWEAPON_BOOL = "UsingWeapon";

    public static string COLORWHITERELOAD = "#FFFFFFFF";
    public static string COLORREDRELOAD = "#FF5A5AFF";
    public static string COLORYELLOWRELOAD = "#FFC14DFF";
    
    public static string COLOR_TABNAME_ACTIVE = "#F0E6FFFF";
    public static string COLOR_TABNAME_INACTIVE = "#C0B8CCFF";

    

   

    
}
