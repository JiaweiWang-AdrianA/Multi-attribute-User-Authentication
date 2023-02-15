using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.IO;


public class AuthenticationFunctions : MonoBehaviour
{
    private string SecurityLevel = "";

    public  GameObject AuthObjPrefab;

    private  int AuthTypeNum;
    private  int[] AuthValuesLen;//int[authTypeNum];
    private  int AuthObjNum;
    private  int MiniCoObjNum;

    public int FailNum = 0;
    public float SpendTime = 0f;
    public bool IsAuthenticationSuccess = false;

    private  int[,] AuthObjGeno;
    public  bool[] IsAuthObjSelected;
    private  bool[] IsAuthObjMatched;

    public System.Random rand = new System.Random();

    public void initAuth(string securityLevel, GameObject authObjPrefab,int authTypeNum, int[] authValuesLen,int authObjNum, int miniCoObjNum)
    {
        this.SecurityLevel = securityLevel;
        this.AuthObjPrefab = authObjPrefab;
        this.AuthTypeNum = authTypeNum;
        this.AuthValuesLen = authValuesLen;
        this.AuthObjNum = authObjNum;
        this.MiniCoObjNum = miniCoObjNum;
        this.AuthObjGeno = new int[AuthObjNum, AuthTypeNum];
        this.IsAuthObjSelected = new bool[AuthObjNum];
        for (int i = 0; i < AuthObjNum; i++)
            this.IsAuthObjSelected[i] = false;
        this.IsAuthObjMatched = new bool[AuthObjNum];
        
        this.FailNum = 0;
        this.SpendTime = 0f;
        this.IsAuthenticationSuccess = false;
}


    public int[,] authObjGenFollowedRules()
    {
        // genarate authObjNum objects follow the rules
        int[,] authObjs = AuthObjGeno;
        int attemptCount = 0;
        int objCount = 0;
        int[][] passValuesInxs = AuthSetting.getPasswordValuesInxs(SecurityLevel);
        int[,] recordedAuthObjs = AuthSetting.getRecordedAuthObjs(SecurityLevel);
        int recordAuthNum = AuthSetting.getRecordAuthNum(SecurityLevel);
        while (objCount < AuthObjNum)
        {
            int[] obj = new int[AuthTypeNum];
            // At least miniCoObjNum(K_min in paper) objects are correct
            if (objCount < MiniCoObjNum)
            {
                for (int j = 0; j < AuthTypeNum; j++)
                {
                    int randomInx;
                    // if the type j is one of password types
                    if (passValuesInxs[j].Length > 0)
                        randomInx = passValuesInxs[j][rand.Next(0, passValuesInxs[j].Length)];
                    // if the type j does not belong to password types
                    else
                        randomInx = rand.Next(0, AuthValuesLen[j]);
                    obj[j] = randomInx;
                }
            }
            // genarate random attribute values of rest object
            else
            {
                for (int j = 0; j < AuthTypeNum; j++)
                    obj[j] = rand.Next(0, AuthValuesLen[j]);
            }
            // the genarated object has at least two different attributes comparing to every object genarated before
            // if this condition cannot be met after 20 consecutive attempts, authObjFalg is always True
            bool authObjFlag = true;
            for (int k = 0; k < recordAuthNum; k++)
            {
                //after 20 consecutive attempts, don't need to compute the objDiffCount (authObjFalg is always True)
                if (attemptCount >= 20) { print("!!!attemptCount>20"); break; }
                int objDiffCount = 0;
                for (int j = 0; j < AuthTypeNum; j++)
                    if (obj[j] != recordedAuthObjs[k, j])
                        objDiffCount++;
                if (objDiffCount <= 1)
                {
                    authObjFlag = false;
                    attemptCount++;
                }
            }
            if (authObjFlag)
            {
                for (int j = 0; j < AuthTypeNum; j++)
                    authObjs[objCount, j] = obj[j];
                recordedAuthObjs = AuthSetting.addRecordedAuthObj(obj, recordedAuthObjs);
                objCount++;
                if (attemptCount < 20) attemptCount = 0;
            }
        }
        // adjust the indexs of the password-matching objects
        for (int i = 0; i < MiniCoObjNum + 1; i++)
        {
            int randomInx = rand.Next(0, AuthObjNum);
            //print("[ "+i.ToString()+" ] : "+randomInx.ToString());
            //switch the objects with index i and randomInx
            for (int j = 0; j < AuthTypeNum; j++)
            {
                int tmp = authObjs[i, j];
                authObjs[i, j] = authObjs[randomInx, j];
                authObjs[randomInx, j] = tmp;
            }
        }
        AuthSetting.setRecordedAuthObjs(SecurityLevel, recordedAuthObjs);
        AuthSetting.printRecordedAuthObjs(recordedAuthObjs);
        return authObjs;
    }


    /// <summary>generate authCube to select </summary>
    public void AuthObjGen()
    {
        ////genanrate the authObjs array
        //for (int i = 0; i < authObjNum; i++)
        //    for (int j = 0; j < AuthSetting.authTypeNum; j++)
        //        authObjGeno[i, j] = rand.Next(0, authValuesLen[j] - 1);
        AuthObjGeno = authObjGenFollowedRules();
        //AuthSetting.addRecordedAuthObjs(authObjGeno);

        //generate authObj (360 degree surround)
        float angle = 360f / AuthObjNum;
        for (int i = 0; i < AuthObjNum; i++)
        {
            //GameObject authGenoCube = GameObject.Instantiate(authCube, Vector3.zero, Quaternion.identity);
            Transform LoginObjTrans = GameObject.Find("Authentication").transform;
            GameObject authObj = GameObject.Instantiate(AuthObjPrefab, LoginObjTrans);

            //authCube.transform.SetParent(authTypeTrans, false);

            //set position 位置公式：  x = 原点x + 半径 * 邻边除以斜边的比例,   邻边除以斜边的比例 = Sin(弧度) , 弧度 = 角度 *3.14f / 180f;（Y同理弧度Cos（弧度））  
            float x = authObj.transform.localPosition.x + 2.8f * Mathf.Sin((angle * i) * (Mathf.PI / 180f));
            float y = authObj.transform.localPosition.y - 0.2f;
            float z = authObj.transform.localPosition.z + 2.8f * Mathf.Cos((angle * i) * (Mathf.PI / 180f));
            authObj.transform.localPosition = new Vector3(x, y, z);

            //set rotation
            float rx = authObj.transform.localEulerAngles.x;
            float ry = authObj.transform.localEulerAngles.y + angle * i + 90f;
            float rz = authObj.transform.localEulerAngles.z;
            authObj.transform.localEulerAngles = new Vector3(rx, ry, rz);

            authObj.SetActive(true);

            authObj.name = "AuthObj" + i.ToString();
            authObj.tag = "AuthObj";
            for (int j = 0; j < AuthTypeNum; j++)
            {
                GameObject authTypeObj = authObj.transform.GetChild(j).gameObject;
                authTypeObj.SetActive(true);
                authTypeObj.transform.GetChild(AuthObjGeno[i, j]).gameObject.SetActive(true);
            }
        }
    }


    //get the password-matching authentication objects
    private void getAuthObjMatched()
    {
        bool[] isPassWordType = new bool[AuthTypeNum];
        bool[][] isPasswordValues = new bool[AuthTypeNum][];
        switch (SecurityLevel)
        {
            case "Low":
                isPassWordType = AuthSetting.LowSecuritySetting.IsPasswordTypes;
                isPasswordValues = AuthSetting.LowSecuritySetting.IsPasswordValues;
                break;
            case "Medium":
                isPassWordType = AuthSetting.MediumSecuritySetting.IsPasswordTypes;
                isPasswordValues = AuthSetting.MediumSecuritySetting.IsPasswordValues;
                break;
            case "High":
                isPassWordType = AuthSetting.HighSecuritySetting.IsPasswordTypes;
                isPasswordValues = AuthSetting.HighSecuritySetting.IsPasswordValues;
                break;
            case "TrainingSession":
                isPassWordType = AuthSetting.TrainingSessionSetting.IsPasswordTypes;
                isPasswordValues = AuthSetting.TrainingSessionSetting.IsPasswordValues;
                break;
            default:
                break;
        }

        for (int i = 0; i < AuthObjNum; i++)
        {
            IsAuthObjMatched[i] = true;
            for (int j = 0; j < isPassWordType.Length; j++)
                if (isPassWordType[j] && !isPasswordValues[j][AuthObjGeno[i, j]])
                {
                    IsAuthObjMatched[i] = false;
                    continue;
                }
        }        
    }


    //compare arr1 and arr2 (if arr1==arr2: return true)
    public static bool compareArr(bool[] arr1, bool[] arr2)
    {
        for (int i = 0; i < arr1.Length; i++)
            if (arr1[i] != arr2[i])
                return false;
        return true;
    }

    // save the result of authentication to File
    public void saveAuthResultToFile()
    {
        string outputText = "[ Authentication ]\nSecurityLevel : " + SecurityLevel + ", ";
        if (IsAuthenticationSuccess)
            outputText += "Result : Success, FailNum : " + FailNum.ToString() + ", ";
        else
            outputText += "Result : Fail, FailNum : " + FailNum.ToString() + ", ";
        outputText += "SpendTime : " + SpendTime.ToString("f2") + "\n";

        print(outputText);
        string filePath = AuthSetting.getFilePath();
        StreamWriter file = new StreamWriter(filePath, true);
        //StreamWriter file = new StreamWriter(@"D:\\ResultAuthentication.txt", true);
        file.Write(outputText); //file.Write()直接追加文件末尾，不换行
        file.Close();
    }


    //if the authentication is successfull,return true
    public bool isAuthenticationSuccess()
    {
        if (IsAuthenticationSuccess)
            return true;

        getAuthObjMatched();
        //print the password-matching objects
        string output = "";
        for (int i = 0; i < AuthObjNum; i++)
            output += (IsAuthObjMatched[i].ToString() + ", ");
        print(output);

        if (compareArr(IsAuthObjMatched, IsAuthObjSelected) && FailNum < AuthSetting.maxAttemptNum)
        {
            SpendTime = Time.timeSinceLevelLoad;
            IsAuthenticationSuccess = true;
            // if authentication is successful, save the result to file and disable the [Login] button
            saveAuthResultToFile();
            GameObject.Find("LoginButton").tag = "Untagged";
        }
        else
        {
            FailNum += 1;
            // if fail the authentication, save the result to file and disable the [Login] button
            if (FailNum == AuthSetting.maxAttemptNum)
            {
                SpendTime = Time.timeSinceLevelLoad;
                saveAuthResultToFile();
                GameObject.Find("LoginButton").tag = "Untagged";
            }
            IsAuthenticationSuccess = false;
        }


        return IsAuthenticationSuccess;

    }


    // return the prompt about authentication
    public string getAuthPrompt()
    {
        string outputText = "";
        if (IsAuthenticationSuccess)
            outputText = "[ Success ]\n Number of Attempts: " + (FailNum + 1) + "\n Spend Time: " + SpendTime.ToString("f2") + "s\n" + "Please trun arroud and select [Back] to return";
        
        else
        {
            outputText = "[ Fail ]\n Remaining chance: ";
            if (AuthSetting.maxAttemptNum - FailNum > 0)
                outputText += (AuthSetting.maxAttemptNum - FailNum).ToString() + "\n Please try again.";
            else
                outputText += "0\n Spend Time: " + SpendTime.ToString("f2") + "s\n" + "Please trun arroud and select [Back] to return"; 
        }
        return outputText;
    }


}
