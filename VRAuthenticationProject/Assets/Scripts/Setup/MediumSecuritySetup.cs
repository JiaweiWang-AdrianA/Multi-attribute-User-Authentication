using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumSecuritySetup : MonoBehaviour
{
    public static SetupFunctions setup = new SetupFunctions();
    public static string securityLevel = "Medium";

    public GameObject authObjPrefab;

    private const int authTypeNum = AuthSetting.MediumSecuritySetting.AuthTypeNum;
    private static int[] authValuesLen = AuthSetting.MediumSecuritySetting.AuthValuesLen;
    private const int passTypeNum = AuthSetting.MediumSecuritySetting.PassTypeNum;
    private static int[] passValuesLen = AuthSetting.MediumSecuritySetting.PassValuesLen;
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
