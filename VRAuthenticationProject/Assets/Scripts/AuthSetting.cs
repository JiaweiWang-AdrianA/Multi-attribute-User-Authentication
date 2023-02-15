using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthSetting : MonoBehaviour
{
    /*
     [ Setting ]
     */

    public const int authTypeNum = 5; // number of one authentication object's attribute types (N in paper)
    public const int authObjNum = 12; // number of authentication objects in one authentication (M in paper)
    public const int miniCoObjNum = 3; // minimum number of password-matching objects in one authentication (K_min in paper)
    public const int recordAuthNum = 30;  // number of authentication objects records
    public static int[,] recordedAuthObjs = new int[recordAuthNum, authTypeNum]; // the authentication objects records

    // Authentication object attribute types
    public static string[] authTypes = { "Food", "Plate", "Drink", "Tablewave", "Number" };
    // Authentication object attribute values
    public static string[] auth1Values = { "Burger", "Cheese", "Chips", "Croissant", "Egg", "Fries", "Hotdog", "Plzza", "Rice", "Sushi", "Tacos", "Toast" };
    public static string[] auth2Values = { "PlateWhite", "PlatePink", "PlateBlack", "PlateWood", "PlateBlue1", "PlateBlue2", "PlateGreen1", "PlateGreen2", "PlateRed1", "PlateRed2", "PlateYellow1", "PlateYellow2" };
    public static string[] auth3Values = { "Cola", "Pepsi", "Redbull", "Sprite", "Sunkist", "Wangzai", "Wanglaoji", "Milk", "Wine", "DrinkBlue", "DrinkGreen", "DrinkRed" };
    public static string[] auth4Values = { "KnifeBlack", "KnifeBlue", "KnifeGreen", "KnifeRed", "KnifeWood", "KnifeYellow", "SpoonBlack", "SpoonBlue", "SpoonGreen", "SpoonRed", "SpoonWood", "SpoonYellow" };
    public static string[] auth5Values = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

    // user's password setting: attribute type and corresponding values
    public static bool[] isPasswordTypes = new bool[] { false, false, false, false, false };
    public static bool[][] isPasswordValues = new bool[authTypeNum][]{
        new bool[]{ false, false, false, false, false, false, false, false, false, false, false, false },
        new bool[]{ false, false, false, false, false, false, false, false, false, false, false, false },
        new bool[]{ false, false, false, false, false, false, false, false, false, false, false, false },
        new bool[]{ false, false, false, false, false, false, false, false, false, false, false, false },
        new bool[]{ false, false, false, false, false, false, false, false, false, false, false, false }
    };

    public const int maxAttemptNum = 3; // max number of the user authentication attempts
    //public static float usingTime = 0; // time from the user to enter the authentication interface to the successful authentication

    public static int UserID = 39;
    //public static string fileFolderPath = "E:\\result\\";
    public static string fileFolderPath = "C:\\Users\\admin\\Desktop\\result\\";

    public static string getFilePath()
    {
        string filePath = fileFolderPath + "User" + AuthSetting.UserID.ToString() + ".txt";
        return filePath;
    }


    public static string[] stringArrayCopy(string[] oriArray, int copyLength)
    {
        string[] resultArray = new string[copyLength];
        for (int i = 0; i < copyLength; i++)
            resultArray[i] = oriArray[i];
        return resultArray;
    }

    public static bool[] boolArrayCopy(bool[] oriArray, int copyLength)
    {
        bool[] resultArray = new bool[copyLength];
        for (int i = 0; i < copyLength; i++)
            resultArray[i] = oriArray[i];
        return resultArray;
    }

    //training session setting
    public class TrainingSessionSetting
    {
        public const int AuthTypeNum = 5;
        public static int[] AuthValuesLen = { 12, 12, 12, 12, 12 };
        public static string[] AuthTypeName = { authTypes[0], authTypes[1], authTypes[2], authTypes[3], authTypes[4] };
        public static string[][] AuthValueName = new string[AuthTypeNum][]
        {
            stringArrayCopy(auth1Values, AuthValuesLen[0]),
            stringArrayCopy(auth2Values, AuthValuesLen[1]),
            stringArrayCopy(auth3Values, AuthValuesLen[2]),
            stringArrayCopy(auth4Values, AuthValuesLen[3]),
            stringArrayCopy(auth5Values, AuthValuesLen[4])
        };
        public const int AuthObjNum = 6;
        public const int MiniCoObjNum = 2;
        public const int RecordAuthNum = 30;
        public static int[,] RecordedAuthObjs = initRecordedAuthObjs(RecordAuthNum, AuthTypeNum);
        public const int PassTypeNum = 3;
        public static int[] PassValuesLen = { 2, 2, 2 };

        // initate the password
        public static bool[] IsPasswordTypes = { true, false, true, false, true }; // set the password types are "Food", "Drink" and "Number"
        public static bool[][] IsPasswordValues = new bool[AuthTypeNum][] {
            new bool[]{ true, false, false, false, false, true, false, false, false, false, false, false }, //set the password value of "Food" is {"Burger","Fries"}
            boolArrayCopy(isPasswordValues[1], AuthValuesLen[1]),
            new bool[]{ true, true, false, false, false, false, false, false, false, false, false, false }, //set the password value of "Drink" is {"Cola","Pepsi"}
            boolArrayCopy(isPasswordValues[3], AuthValuesLen[3]),
            new bool[]{ true, true, false, false, false, false, false, false, false, false, false, false } //set the password value of "Number" is {"1","2"}
        };
    }


    //low security setting
    public class LowSecuritySetting
    {
        public const int AuthTypeNum = 4;
        public static int[] AuthValuesLen = { 10, 10, 10, 10 };
        public static string[] AuthTypeName = { authTypes[0], authTypes[1], authTypes[2], authTypes[3] };
        public static string[][] AuthValueName = new string[AuthTypeNum][]
        {
            stringArrayCopy(auth1Values, AuthValuesLen[0]),
            stringArrayCopy(auth2Values, AuthValuesLen[1]),
            stringArrayCopy(auth3Values, AuthValuesLen[2]),
            stringArrayCopy(auth4Values, AuthValuesLen[3])
        };
        //public static string[] Auth1Values = ArrayCopy(auth1Values, AuthValuesLen[0]);
        //public static string[] Auth2Values = ArrayCopy(auth2Values, AuthValuesLen[1]);
        //public static string[] Auth3Values = ArrayCopy(auth3Values, AuthValuesLen[2]);
        public const int AuthObjNum = 8;
        public const int MiniCoObjNum = 2;
        public const int RecordAuthNum = 24;
        public static int[,] RecordedAuthObjs = initRecordedAuthObjs(RecordAuthNum, AuthTypeNum);
        public const int PassTypeNum = 1;
        public static int[] PassValuesLen = { 3 };

        // initate the password
        public static bool[] IsPasswordTypes = { false, false, false, false };
        public static bool[][] IsPasswordValues = new bool[AuthTypeNum][] {
            boolArrayCopy(isPasswordValues[0], AuthValuesLen[0]),
            boolArrayCopy(isPasswordValues[1], AuthValuesLen[1]),
            boolArrayCopy(isPasswordValues[2], AuthValuesLen[2]),
            boolArrayCopy(isPasswordValues[3], AuthValuesLen[3])
        };
    }


    //medium security setting
    public class MediumSecuritySetting
    {
        public const int AuthTypeNum = 5;
        public static int[] AuthValuesLen = { 10, 10, 10, 10, 10 };
        public static string[] AuthTypeName = { authTypes[0], authTypes[1], authTypes[2], authTypes[3], authTypes[4] };
        public static string[][] AuthValueName = new string[AuthTypeNum][]
        {
            stringArrayCopy(auth1Values, AuthValuesLen[0]),
            stringArrayCopy(auth2Values, AuthValuesLen[1]),
            stringArrayCopy(auth3Values, AuthValuesLen[2]),
            stringArrayCopy(auth4Values, AuthValuesLen[3]),
            stringArrayCopy(auth5Values, AuthValuesLen[4])
        };
        //public static string[] Auth1Values = ArrayCopy(auth1Values, AuthValuesLen[0]);
        //public static string[] Auth2Values = ArrayCopy(auth2Values, AuthValuesLen[1]);
        //public static string[] Auth3Values = ArrayCopy(auth3Values, AuthValuesLen[2]);
        //public static string[] Auth4Values = ArrayCopy(auth4Values, AuthValuesLen[3]);
        public const int AuthObjNum = 10;
        public const int MiniCoObjNum = 2;
        public const int RecordAuthNum = 30;
        public static int[,] RecordedAuthObjs = initRecordedAuthObjs(RecordAuthNum, AuthTypeNum);
        public const int PassTypeNum = 2;
        public static int[] PassValuesLen = { 2, 2 };

        // initate the password
        public static bool[] IsPasswordTypes = { false, false, false, false, false };
        public static bool[][] IsPasswordValues = new bool[AuthTypeNum][] {
            boolArrayCopy(isPasswordValues[0], AuthValuesLen[0]),
            boolArrayCopy(isPasswordValues[1], AuthValuesLen[1]),
            boolArrayCopy(isPasswordValues[2], AuthValuesLen[2]),
            boolArrayCopy(isPasswordValues[3], AuthValuesLen[3]),
            boolArrayCopy(isPasswordValues[4], AuthValuesLen[4])
        };
    }


    //high security setting
    public class HighSecuritySetting
    {
        public const int AuthTypeNum = 5;
        public static int[] AuthValuesLen = { 12, 12, 12, 12, 12 };
        public static string[] AuthTypeName = { authTypes[0], authTypes[1], authTypes[2], authTypes[3], authTypes[4] };
        public static string[][] AuthValueName = new string[AuthTypeNum][]
        {
            stringArrayCopy(auth1Values, AuthValuesLen[0]),
            stringArrayCopy(auth2Values, AuthValuesLen[1]),
            stringArrayCopy(auth3Values, AuthValuesLen[2]),
            stringArrayCopy(auth4Values, AuthValuesLen[3]),
            stringArrayCopy(auth5Values, AuthValuesLen[4])
        };
        //public static string[] Auth1Values = ArrayCopy(auth1Values, AuthValuesLen[0]);
        //public static string[] Auth2Values = ArrayCopy(auth2Values, AuthValuesLen[1]);
        //public static string[] Auth3Values = ArrayCopy(auth3Values, AuthValuesLen[2]);
        //public static string[] Auth4Values = ArrayCopy(auth4Values, AuthValuesLen[3]);
        public const int AuthObjNum = 12;
        public const int MiniCoObjNum = 3;
        public const int RecordAuthNum = 36;
        public static int[,] RecordedAuthObjs = initRecordedAuthObjs(RecordAuthNum, AuthTypeNum);
        public const int PassTypeNum = 2;
        public static int[] PassValuesLen = { 3, 3 };

        // initate the password
        public static bool[] IsPasswordTypes = { false, false, false, false, false };
        public static bool[][] IsPasswordValues = new bool[AuthTypeNum][] {
            boolArrayCopy(isPasswordValues[0], AuthValuesLen[0]),
            boolArrayCopy(isPasswordValues[1], AuthValuesLen[1]),
            boolArrayCopy(isPasswordValues[2], AuthValuesLen[2]),
            boolArrayCopy(isPasswordValues[3], AuthValuesLen[3]),
            boolArrayCopy(isPasswordValues[4], AuthValuesLen[4])
        };
    }

    public static int[,] initRecordedAuthObjs(int RecordAuthNum, int AuthTypeNum)
    {
        int[,] recordedAuthObjs = new int[RecordAuthNum, AuthTypeNum];
        for (int i = 0; i < RecordAuthNum; i++)
            for (int j = 0; j < AuthTypeNum; j++)
                recordedAuthObjs[i, j] = -1;
        return recordedAuthObjs;
    }

    public static int getRecordAuthNum(string securityLevel)
    {
        int recordAuthNum = AuthSetting.recordAuthNum;
        switch (securityLevel)
        {
            case "TrainingSession":
                recordAuthNum = TrainingSessionSetting.RecordAuthNum;
                break;
            case "Low":
                recordAuthNum = LowSecuritySetting.RecordAuthNum;
                break;
            case "Medium":
                recordAuthNum = MediumSecuritySetting.RecordAuthNum;
                break;
            case "High":
                recordAuthNum = HighSecuritySetting.RecordAuthNum;
                break;
            default:
                break;
        }
        return recordAuthNum;
    }

    public static int[,] getRecordedAuthObjs(string securityLevel)
    {
        int[,] recordedAuthObjs = AuthSetting.recordedAuthObjs;
        switch (securityLevel)
        {
            case "TrainingSession":
                recordedAuthObjs = TrainingSessionSetting.RecordedAuthObjs;
                break;
            case "Low":
                recordedAuthObjs = LowSecuritySetting.RecordedAuthObjs;
                break;
            case "Medium":
                recordedAuthObjs = MediumSecuritySetting.RecordedAuthObjs;
                break;
            case "High":
                recordedAuthObjs = HighSecuritySetting.RecordedAuthObjs;
                break;
            default:
                break;
        }
        return recordedAuthObjs;
    }

    public static void setRecordedAuthObjs(string securityLevel, int[,] recordedAuthObjs)
    {
        switch (securityLevel)
        {
            case "TrainingSession":
                TrainingSessionSetting.RecordedAuthObjs = recordedAuthObjs;
                break;
            case "Low":
                LowSecuritySetting.RecordedAuthObjs = recordedAuthObjs;
                break;
            case "Medium":
                MediumSecuritySetting.RecordedAuthObjs = recordedAuthObjs;
                break;
            case "High":
                HighSecuritySetting.RecordedAuthObjs = recordedAuthObjs;
                break;
            default:
                break;
        }
    }

    public static int[,] addRecordedAuthObj(int[] obj, int[,] recordedAuthObjs)
    {
        int[,] resultObjs = recordedAuthObjs;
        //find the index of blank record
        int recordInx = 0;
        int recordAuthNum = recordedAuthObjs.GetLength(0);
        while (recordInx < recordAuthNum && resultObjs[recordInx, 0] != -1)
            recordInx ++;
        // j is the index of authType
        for (int j = 0; j < obj.Length; j++)
            resultObjs[(recordInx) % recordAuthNum, j] = obj[j];
        return resultObjs;
    }


    public static void printRecordedAuthObjs(int[,] recordedAuthObjs)
    {
        //print the recordAuthObjs
        string outputAuthObjs = "";
        for (int i = 0; i < recordedAuthObjs.GetLength(0); i++)
        {
            if (recordedAuthObjs[i, 0] == -1) break;
            outputAuthObjs += ("Obj" + i.ToString() + " : ");
            for (int j = 0; j < recordedAuthObjs.GetLength(1); j++)
                outputAuthObjs += (recordedAuthObjs[i, j].ToString() + ",");
            outputAuthObjs += ";";
        }
        print(outputAuthObjs);
    }

    //reset the password
    public static void resetPassword()
    {
        // reset the passwordTypes
        for (int i = 0; i < isPasswordTypes.Length; i++)
            isPasswordTypes[i] = false;
        // reset the passwordValues
        for (int i = 0; i < isPasswordValues.Length; i++)
            for (int j = 0; j < isPasswordValues[i].Length; j++)
                isPasswordValues[i][j] = false;
    }

    //if no setting password, return true
    public static bool isSettingNull(bool[] isPasswordTypes)
    {
        for (int i = 0; i < isPasswordTypes.Length; i++)
            if (isPasswordTypes[i])
                return false;
        return true;
    }

    // get the passwordValuesInxs
    public static int[][] getPasswordValuesInxs(string securityLevel)
    {
        bool[][] isPasswordValues = AuthSetting.isPasswordValues;
        switch (securityLevel)
        {
            case "TrainingSession":
                isPasswordValues = TrainingSessionSetting.IsPasswordValues;
                break;
            case "Low":
                isPasswordValues = LowSecuritySetting.IsPasswordValues;
                break;
            case "Medium":
                isPasswordValues = MediumSecuritySetting.IsPasswordValues;
                break;
            case "High":
                isPasswordValues = HighSecuritySetting.IsPasswordValues;
                break;
            default:
                break;
        }
        
        int[][] passwordVaulesInxs = new int[authTypeNum][];
        for (int i = 0; i < isPasswordValues.Length; i++)
        {
            List<int> inxListTemp = new List<int>();
            for (int j = 0; j < isPasswordValues[i].Length; j++)
                if (isPasswordValues[i][j])
                    inxListTemp.Add(j);
            passwordVaulesInxs[i]= inxListTemp.ToArray();
        }

        ////print the passwordValuesInxs
        //string outputResult = "";
        //for (int i = 0; i < passwordVaulesInxs.Length; i++)
        //{
        //    outputResult += ("Type" + i.ToString() + ": ");
        //    for (int j = 0; j < passwordVaulesInxs[i].Length; j++)
        //        outputResult += (passwordVaulesInxs[i][j].ToString() + ",");
        //    outputResult += " ; ";
        //}
        //print(outputResult);
        return passwordVaulesInxs;
    }
    



    //the last one scene name
    public static string LastSceneName = "Authentication";
    public static void setLastSceneName(string name)
    {
        LastSceneName = name;
    }

    public static string getLastSceneName()
    {
        return LastSceneName;
    }

    /*
    //set the password of training session
    public static void setTrainingSessionPassword()
    {
        resetPassword();
        isPasswordTypes[0] = true;  // set the password type is "Food"
        isPasswordValues[0][0] = true; //set the password value of "Food" is "Burger"
        isPasswordValues[0][5] = true; //set the password value of "Food" is "Fries"
        isPasswordTypes[4] = true; // set the password type is "Number"
        isPasswordValues[4][0] = true;// set the password value of "Food" is "1"
        isPasswordValues[4][1] = true;// set the password value of "Food" is "2"
        //isPasswordValues[4][2] = true;// set the password value of "Food" is "3"
    }*/



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


