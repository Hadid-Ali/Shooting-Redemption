using System;
using UnityEditor;
using UnityEngine;

public partial class GameAdEvents
{
    public static class MenuEvents
    {
        public static GameEvent<MenuName> MenuStateSwitched = new();
        public static GameEvent MenuControllerInit = new();

        public static GameEvent LevelWin = new();
        public static GameEvent LevelFail = new();
    }
}
