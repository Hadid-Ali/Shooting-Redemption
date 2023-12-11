using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameAdEvents
{
    public static GameEvent<float> m_HealthUpdate=new();
    public static GameEvent<float> m_EnemyHealthUpdate=new();
    public static GameEvent<float,float> m_UIHealthUpdate=new();
    public static GameEvent levelComplete = new();
 //   public static GameEvent<EnemyType> levelCOmpleteCheck = new();
 //   public static GameEvent<UIMenuBase> SetPauseMenu = new();
    public static GameEvent DisablePlayer = new();
    public static GameEvent DieAnimation = new();
}
