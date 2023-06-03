using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // WaitForSeconds 
    private readonly Dictionary<float, WaitForSeconds> dicWaitForSeconds = new Dictionary<float, WaitForSeconds>();

    protected override bool DontDestroyOnload => true;

    protected override void OnAwake()
    {
        // Static 변수 초기화 시점.
    }

    // 모든 시분할 메서드는 싱글턴 하나에서 관리.
    public WaitForSeconds Get_WaitForSeconds(float second)
    {
        if (!dicWaitForSeconds.ContainsKey(second))
            dicWaitForSeconds.Add(second, new WaitForSeconds(second));
        return dicWaitForSeconds[second];
    }

}
