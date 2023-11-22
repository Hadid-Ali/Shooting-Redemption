using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDataHandler : MonobehaviourSingleton<GunDataHandler>
{
    public List<GunData> GunDatas;

    public GunData GetGunData(GunName gunName) => GunDatas.Find(x => x.gun == gunName);
}


