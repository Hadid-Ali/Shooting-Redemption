using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SessionData : MonobehaviourSingleton<SessionData>
{
    public bool comingToMainMenuOnModeComplete;
    
    public int civiliansKilled;
    public SceneName sceneToLoad;
    
    public List<CharacterData> characterDatas;
    public CharacterData GetCharacterData(CharacterType characterType) => characterDatas.Find(x => x.character == characterType);
    
    public List<GunData> GunDatas;
    public GunData GetGunData(OverlayWeapons gunName) => GunDatas.Find(x => x.gun == gunName);

    public List<CharacterStates> PlayerFabs;
    public GameObject GetPlayerPrefab(CharacterType characterType) =>
        PlayerFabs.Find(x => x.CharacterType == characterType).transform.gameObject;
}
