using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerCustomStates
{
     IdleStart,
     InMovement,
     HoldingPosition,
     InZoom
}
public class CharacterStates : MonoBehaviour
{
    public static PlayerCustomStates playerState;

    private void Awake()
    {
        playerState = PlayerCustomStates.IdleStart;
    }


    public void SetPlayerState(PlayerCustomStates state)
    {
        playerState = state;
    }
}
