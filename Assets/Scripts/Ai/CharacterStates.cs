using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerCustomStates
{
     CutScene,
     InMovement,
     HoldingPosition,
     InActive,
     InZoom
}
public class CharacterStates : MonoBehaviour
{
    public static PlayerCustomStates playerState;

    private void Awake()
    {
        playerState = PlayerCustomStates.CutScene;
    }


    public void SetPlayerState(PlayerCustomStates state)
    {
        playerState = state;
    }
}
