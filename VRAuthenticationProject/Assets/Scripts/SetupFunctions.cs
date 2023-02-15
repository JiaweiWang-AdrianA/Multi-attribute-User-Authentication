using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SetupFunctions : MonoBehaviour
{
    private string SecurityLevel = "";

    public GameObject AuthObjPrefab;

    private int AuthTypeNum;
    private int[] AuthValuesLen;
    private int PassTypeNum;
    private int[] PassValuesLen;
    public bool[] IsPasswordTypes;
    public bool[][] IsPasswordValues;
    private float[] ValueObjScale = { 2f, 1f, 2f, 2f, 2f };


    public void initSetup(string securityLevel, GameObject authObjPrefab, int authTypeNum, int[] authValuesLen, int passTypeNum, int[] passValuesLen)
    {
        this.SecurityLevel = securityLevel;
        this.AuthObjPrefab = authObjPrefab;
        this.AuthTypeNum = authTypeNum;
        this.AuthValuesLen = authValuesLen;
        this.PassTypeNum = passTypeNum;
        this.PassValuesLen = passValuesLen;
        // set the initial password to be false
        this.IsPasswordTypes = new bool[AuthTypeNum];
        this.IsPasswordValues = new bool[AuthTypeNum][];
        for(int i=0;i< AuthTypeNum; i++)
        {
            this.IsPasswordTypes[i] = false;
            bool[] valuesTemp =new bool[AuthValuesLen[i]];
            for (int j=0;j< AuthValuesLen[i]; j++) 
                valuesTemp[j] = false;
            this.IsPasswordValues[i] = valuesTemp;
        }
        initSetupValueButtons();
    }

    // generate the setupPassValueButton
    public void initSetupValueButtons()
    {
        Transform SetupPassValue = GameObject.Find("SetupPassValue").transform;
        for (int typeInx = 0; typeInx < AuthTypeNum; typeInx++)
        {
            // generate the ValueButton
            string setupValueButtonName = "SetupValueOfType" + (typeInx + 1).ToString();
            GameObject setupValueButton = new GameObject(setupValueButtonName);
            setupValueButton.tag = "SetupPassValueButton";
            setupValueButton.transform.parent = SetupPassValue;
            // add the rotate component
            setupValueButton.AddComponent<RotateObjs>();
            
            // generate the ValueObjs (360 degree surround)
            int valuesLen = AuthValuesLen[typeInx];
            float angle = 360f / valuesLen;
            float radius = 2.8f;
            for (int i = 0; i < valuesLen; i++)
            {
                GameObject valueObj = GameObject.Instantiate(AuthObjPrefab, setupValueButton.transform);

                //set position 位置公式：  x = 原点x + 半径 * 邻边除以斜边的比例,   邻边除以斜边的比例 = Sin(弧度) , 弧度 = 角度 *3.14f / 180f;（Y同理弧度Cos（弧度））  
                float x = valueObj.transform.localPosition.x + radius * Mathf.Sin((angle * i) * (Mathf.PI / 180f));
                float y = valueObj.transform.localPosition.y - 0.2f;
                float z = valueObj.transform.localPosition.z + radius * Mathf.Cos((angle * i) * (Mathf.PI / 180f));
                valueObj.transform.localPosition = new Vector3(x, y, z);

                //set rotation
                float rx = valueObj.transform.localEulerAngles.x;
                float ry = valueObj.transform.localEulerAngles.y + angle * i + 90f;
                float rz = valueObj.transform.localEulerAngles.z;
                valueObj.transform.localEulerAngles = new Vector3(rx, ry, rz);
                valueObj.SetActive(true);

                //set name and tag
                valueObj.name = "ValueObj" + i.ToString();
                valueObj.tag = "SetupPassValueButton";

                // active the object's Value[typeInx][i] and change the object's scale based on typeInx
                GameObject authTypeObj = valueObj.transform.GetChild(typeInx).gameObject;
                authTypeObj.SetActive(true);
                authTypeObj.transform.GetChild(i).gameObject.SetActive(true);
                authTypeObj.transform.GetChild(i).gameObject.transform.localScale *= ValueObjScale[typeInx];
            }
            setupValueButton.SetActive(false);
        }
    }

    // generate the buttons of password types setting
    public void authTypeButtonGen()
    {
        // hide the ValueButton Objects and the [SavePassValue] button
        GameObject SetupPassValue = GameObject.Find("SetupPassValue");
        foreach (Transform child in SetupPassValue.transform)
            if (child.tag == "SetupPassValueButton" || child.name == "SavePassValue")
                child.gameObject.SetActive(false);

        // active the TypeButton Objects and the [SavePassword] button
        GameObject authSetting = GameObject.Find("SetupPassType");
        foreach (Transform child in authSetting.transform)
            if (child.tag == "SetupPassTypeButton" || child.name == "SavePassword")
                child.gameObject.SetActive(true);
    }

    public void authValueButtonGen(int typeInx)
    {
        // hide the TypeButton Objects and the [SavePassword] button
        GameObject SetupPassType = GameObject.Find("SetupPassType");
        foreach (Transform child in SetupPassType.transform)
            if (child.tag == "SetupPassTypeButton" || child.name == "SavePassword")
                child.gameObject.SetActive(false);

        //activethe ValueButton[typeInx] Object and the [SavePassValue] button
        string valueButtonName = "SetupValueOfType" + (typeInx + 1).ToString();
        GameObject SetupPassValue = GameObject.Find("SetupPassValue");
        foreach (Transform child in SetupPassValue.transform)
            if (child.name == valueButtonName || child.name == "SavePassValue")
                child.gameObject.SetActive(true);
    }

    //save the password value of type[typeInx]
    public void savePassValue(int typeInx)
    {
        //judge weather the type is setted to a password type or not
        IsPasswordTypes[typeInx] = false;
        for (int i = 0; i < IsPasswordValues[typeInx].Length; i++)
            if (IsPasswordValues[typeInx][i])
            {
                IsPasswordTypes[typeInx] = true;
                break;
            }

        //return to the password type setting
        authTypeButtonGen();
    }

    // compare (int[]) arr1 and arr2 (regardless of order, if arr1 and arr2 have same elements: return true)
    public bool compareArray(int[] arr1, int[] arr2)
    {
        if (arr1.Length != arr2.Length)
            return false;
        Array.Sort(arr1);
        Array.Sort(arr2);
        for (int i = 0; i < arr1.Length; i++)
            if (arr1[i] != arr2[i])
                return false;
        return true;
    }

    //if password is not followed the setting rules: return true
    public bool isPasswordFollowedRules()
    {
        int passTypeNumTemp = 0;
        for (int i = 0; i < IsPasswordTypes.Length; i++)
            if (IsPasswordTypes[i])
                passTypeNumTemp += 1;
        int[] passValuesLenTemp = new int[passTypeNumTemp];
        int k = 0;
        for (int i = 0; i < IsPasswordValues.Length; i++)
        {
            if (!IsPasswordTypes[i]) continue;
            int valueNumCount = 0;
            for (int j = 0; j < IsPasswordValues[i].Length; j++)
                if (IsPasswordValues[i][j])
                    valueNumCount += 1;
            passValuesLenTemp[k++] = valueNumCount;
        }
        if (passTypeNumTemp == PassTypeNum && compareArray(passValuesLenTemp, PassValuesLen))
            return true;
        return false;
    }

    // save the password setting to File
    public void savePassToFile()
    {
        string[] authTypeName = new string[AuthTypeNum];
        string[][] authValueName = new string[AuthTypeNum][];
        switch (SecurityLevel)
        {
            case "Low":
                authTypeName = AuthSetting.LowSecuritySetting.AuthTypeName;
                authValueName = AuthSetting.LowSecuritySetting.AuthValueName;
                break;
            case "Medium":
                authTypeName = AuthSetting.MediumSecuritySetting.AuthTypeName;
                authValueName = AuthSetting.MediumSecuritySetting.AuthValueName;
                break;
            case "High":
                authTypeName = AuthSetting.HighSecuritySetting.AuthTypeName;
                authValueName = AuthSetting.HighSecuritySetting.AuthValueName;
                break;
            default:
                break;
        }

        string passwordStr = "Password : { ";
        for (int i = 0; i < IsPasswordValues.Length; i++)
        {
            if (!IsPasswordTypes[i])
                continue;
            //promptPassText += "\n";
            //promptPassText += authTypeName[i] + " : ";
            passwordStr += authTypeName[i] + " : ";
            for (int j = 0; j < IsPasswordValues[i].Length; j++)
                if (IsPasswordValues[i][j])
                    passwordStr += authValueName[i][j] + ", ";
            passwordStr = passwordStr.Remove(passwordStr.Length - 2, 2) + "; ";
        }

        string outputText = "[ Setup ]\nSecurityLevel : " + SecurityLevel + ", ";
        outputText += passwordStr + " }\n";

        print(outputText);
        string filePath = AuthSetting.getFilePath();
        StreamWriter file = new StreamWriter(filePath, true);
        file.Write(outputText); //file.Write()直接追加文件末尾，不换行
        file.Close();
    }

    //save the password to AuthSetting
    public void savePassword()
    {
        //if password is not followed the setting rules
        if (!isPasswordFollowedRules())
        {
            //print("!!!");
            return;
        }
        switch (SecurityLevel)
        {
            case "Low":
                AuthSetting.LowSecuritySetting.IsPasswordTypes = this.IsPasswordTypes;
                AuthSetting.LowSecuritySetting.IsPasswordValues = this.IsPasswordValues;
                AuthSetting.LowSecuritySetting.RecordedAuthObjs = AuthSetting.initRecordedAuthObjs(AuthSetting.LowSecuritySetting.RecordAuthNum, AuthSetting.LowSecuritySetting.AuthTypeNum);
                break;
            case "Medium":
                AuthSetting.MediumSecuritySetting.IsPasswordTypes = this.IsPasswordTypes;
                AuthSetting.MediumSecuritySetting.IsPasswordValues = this.IsPasswordValues;
                AuthSetting.MediumSecuritySetting.RecordedAuthObjs = AuthSetting.initRecordedAuthObjs(AuthSetting.MediumSecuritySetting.RecordAuthNum, AuthSetting.MediumSecuritySetting.AuthTypeNum);
                break;
            case "High":
                AuthSetting.HighSecuritySetting.IsPasswordTypes = this.IsPasswordTypes;
                AuthSetting.HighSecuritySetting.IsPasswordValues = this.IsPasswordValues;
                AuthSetting.HighSecuritySetting.RecordedAuthObjs = AuthSetting.initRecordedAuthObjs(AuthSetting.HighSecuritySetting.RecordAuthNum, AuthSetting.HighSecuritySetting.AuthTypeNum);
                break;
            default:
                break;
        }

        // save the password to File and return to the Index Page 
        savePassToFile();
        SceneManager.LoadScene("Index");        
    }

    // return the prompt about password setting
    public string getPasswordPrompt()
    {
        string[] authTypeName = new string[AuthTypeNum];
        string[][] authValueName = new string[AuthTypeNum][];
        switch (SecurityLevel)
        {
            case "Low":
                authTypeName = AuthSetting.LowSecuritySetting.AuthTypeName;
                authValueName = AuthSetting.LowSecuritySetting.AuthValueName;
                break;
            case "Medium":
                authTypeName = AuthSetting.MediumSecuritySetting.AuthTypeName;
                authValueName = AuthSetting.MediumSecuritySetting.AuthValueName;
                break;
            case "High":
                authTypeName = AuthSetting.HighSecuritySetting.AuthTypeName;
                authValueName = AuthSetting.HighSecuritySetting.AuthValueName;
                break;
            default:
                break;
        }
        string promptPassText = "[ Password ]";
        for (int i = 0; i < IsPasswordValues.Length; i++)
        {
            if (!IsPasswordTypes[i])
                continue;
            promptPassText += "\n";
            promptPassText += authTypeName[i] + " : ";
            for (int j = 0; j < IsPasswordValues[i].Length; j++)
                if (IsPasswordValues[i][j])
                    promptPassText += authValueName[i][j] + " / ";            
        }
        return promptPassText;
    }

    // return the prompt about password value of type[typyInx] setting
    public string getPassValuePrompt(int typeInx)
    {
        string authTypeName = "";
        string[] authValueName = new string[AuthValuesLen[typeInx]];
        switch (SecurityLevel)
        {
            case "Low":
                authTypeName = AuthSetting.LowSecuritySetting.AuthTypeName[typeInx];
                authValueName = AuthSetting.LowSecuritySetting.AuthValueName[typeInx];
                break;
            case "Medium":
                authTypeName = AuthSetting.MediumSecuritySetting.AuthTypeName[typeInx];
                authValueName = AuthSetting.MediumSecuritySetting.AuthValueName[typeInx];
                break;
            case "High":
                authTypeName = AuthSetting.HighSecuritySetting.AuthTypeName[typeInx];
                authValueName = AuthSetting.HighSecuritySetting.AuthValueName[typeInx];
                break;
            default:
                break;
        }

        string promptPassText = "";
        bool isPassType = false;
        for (int j = 0; j < IsPasswordValues[typeInx].Length; j++)
            if (IsPasswordValues[typeInx][j])
            {
                isPassType = true;
                promptPassText += authValueName[j] + " / ";
            }
        if (isPassType)
            promptPassText = "[ Password Value ] of type [ " + authTypeName + " ]\n" + promptPassText;
        else
            promptPassText += "No password value selected of type [ " + authTypeName + " ]\n" + promptPassText;

        return promptPassText;
    }
}
