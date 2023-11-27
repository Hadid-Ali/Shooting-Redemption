using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameEvents : MonoBehaviour
{
    public partial class GamestateEvents
    {
        public static GameEvent GameLost  = new ();
        
    }
    
    public partial class GamePlayEvents
    {
        public static GameEvent<Action> OnPlayerSpawned = new();
    }
}
