using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighSecuritySetup : MonoBehaviour
{
    public static SetupFunctions setup = new SetupFunctions();
    public static string securityLevel = "High";

    public GameObject authObjPrefab;

    private const int authTypeNum = AuthSetting.HighSecuritySetting.AuthTypeNum;
    private static int[] authValuesLen = AuthSetting.HighSecuritySetting.AuthValuesLen;
    private const int passTypeNum = AuthSetting.HighSecuritySetting.PassTypeNum;
    private static int[] passValuesLen = AuthSetting.HighSecuritySetting.PassValuesLen;
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
