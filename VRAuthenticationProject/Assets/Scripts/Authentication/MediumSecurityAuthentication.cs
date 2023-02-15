using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumSecurityAuthentication : MonoBehaviour
{
    public static AuthenticationFunctions securityAuth = new AuthenticationFunctions();
    public static string securityLevel = "Medium";

    public GameObject authObjPrefab;

    private const int authTypeNum = AuthSetting.MediumSecuritySetting.AuthTypeNum;
    private static int[] authValuesLen = AuthSetting.MediumSecuritySetting.AuthValuesLen;
    private const int authObjNum = AuthSetting.MediumSecuritySetting.AuthObjNum;
    private const int miniCoObjNum = AuthSetting.MediumSecuritySetting.MiniCoObjNum;


    // Start is called before the first frame update
    void Start()
    {
        securityAuth.initAuth(securityLevel, authObjPrefab, authTypeNum, authValuesLen, authObjNum, miniCoObjNum);

        securityAuth.AuthObjGen();
        for (int i = 0; i < authObjNum; i++)
            securityAuth.IsAuthObjSelected[i] = false;
    }

    // Update is called once per frame
    void Update()
    {
        // rotate the objects
        transform.RotateAround(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f), 0.1f);
    }
}
