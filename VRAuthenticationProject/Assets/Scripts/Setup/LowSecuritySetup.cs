using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowSecuritySetup : MonoBehaviour
{
    public static SetupFunctions setup = new SetupFunctions();
    public static string securityLevel = "Low";

    public GameObject authObjPrefab;

    private const int authTypeNum = AuthSetting.LowSecuritySetting.AuthTypeNum;
    private static int[] authValuesLen = AuthSetting.LowSecuritySetting.AuthValuesLen;
    private const int passTypeNum = AuthSetting.LowSecuritySetting.PassTypeNum;
    private static int[] passValuesLen = AuthSetting.LowSecuritySetting.PassValuesLen;
    // Start is called before the first frame update
    void Start()
    {
        setup.initSetup(securityLevel, authObjPrefab, authTypeNum, authValuesLen, passTypeNum, passValuesLen);
        //setup.authTypeButtonGen();

    }

    // Update is called once per frame
    void Update()
    {        

    }
}
