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

public enum GameStates
{
    InGame,
    GamePause,
    GameOver
}
public class CharacterStates : MonoBehaviour
{
    public CharacterType CharacterType;
    
    public static PlayerCustomStates playerState;
    public static GameStates gameState;

    private void Awake()
    {
        playerState = PlayerCustomStates.CutScene;
        gameState = GameStates.InGame;

    }

    public static void SetGameState(GameStates _gameState)
    {
        gameState = _gameState;
    }

    public static void SetPlayerState(PlayerCustomStates state)
    {
        playerState = state;
    }
}
