using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class IntwithObj : MonoBehaviour
{
    public bool showHoverState = false;
    public VRTK.VRTK_ControllerEvents vrtkCrtlEvents;

    private string sceneName;


    // select the authObj
    public void selectAuthObj(DestinationMarkerEventArgs e)
    {
        //print(e.target.name);
        int objnum = int.Parse(e.target.name.Replace("AuthObj", ""));
        bool isObjSelected = false;
        switch (sceneName){
            case "TrainingSessionAuthentication":
                TrainingSessionAuthentication.securityAuth.IsAuthObjSelected[objnum] = TrainingSessionAuthentication.securityAuth.IsAuthObjSelected[objnum] ? false : true;
                isObjSelected = TrainingSessionAuthentication.securityAuth.IsAuthObjSelected[objnum];
                break;
            case "LowSecurityAuthentication":
                LowSecurityAuthentication.securityAuth.IsAuthObjSelected[objnum] = LowSecurityAuthentication.securityAuth.IsAuthObjSelected[objnum] ? false : true;
                isObjSelected = LowSecurityAuthentication.securityAuth.IsAuthObjSelected[objnum];
                break;
            case "MediumSecurityAuthentication":
                MediumSecurityAuthentication.securityAuth.IsAuthObjSelected[objnum] = MediumSecurityAuthentication.securityAuth.IsAuthObjSelected[objnum] ? false : true;
                isObjSelected = MediumSecurityAuthentication.securityAuth.IsAuthObjSelected[objnum];
                break;
            case "HighSecurityAuthentication":
                HighSecurityAuthentication.securityAuth.IsAuthObjSelected[objnum] = HighSecurityAuthentication.securityAuth.IsAuthObjSelected[objnum] ? false : true;
                isObjSelected = HighSecurityAuthentication.securityAuth.IsAuthObjSelected[objnum];
                break;
            default:
                break;
        }
        
        foreach (Transform child in e.target.transform)
            if (child.tag == "SelectedMark")
                if (isObjSelected)
                    child.gameObject.SetActive(true);
                else
                    child.gameObject.SetActive(false);
    }

    // select the [Login] button
    public void selectLogin(DestinationMarkerEventArgs e)
    {
        //print(sceneName);
        bool isAuthenticationSuccess = false;
        string promptText = "";
        switch (sceneName)
        {
            case "TrainingSessionAuthentication":
                isAuthenticationSuccess = TrainingSessionAuthentication.securityAuth.isAuthenticationSuccess();
                promptText = TrainingSessionAuthentication.securityAuth.getAuthPrompt();
                break;

            case "LowSecurityAuthentication":
                isAuthenticationSuccess = LowSecurityAuthentication.securityAuth.isAuthenticationSuccess();
                promptText = LowSecurityAuthentication.securityAuth.getAuthPrompt();
                break;
            case "MediumSecurityAuthentication":
                isAuthenticationSuccess = MediumSecurityAuthentication.securityAuth.isAuthenticationSuccess();
                promptText = MediumSecurityAuthentication.securityAuth.getAuthPrompt();
                break;
            case "HighSecurityAuthentication":
                isAuthenticationSuccess = HighSecurityAuthentication.securityAuth.isAuthenticationSuccess();
                promptText = HighSecurityAuthentication.securityAuth.getAuthPrompt();
                break;
            default:
                break;
        }
        GameObject.Find("Prompt").GetComponent<TextMesh>().text = promptText;
    }

    // select [Setup] buttons of different security levels
    public void selectSetup(DestinationMarkerEventArgs e)
    {
        string setupSceneName = "";

        switch (e.target.name)
        {
            case "Button_LowSecuritySetup":
                setupSceneName = "LowSecuritySetup";
                break;
            case "Button_MediumSecuritySetup":
                setupSceneName = "MediumSecuritySetup";
                break;
            case "Button_HighSecuritySetup":
                setupSceneName = "HighSecuritySetup";
                break;
            default:
                break;
        }
        SceneManager.LoadScene(setupSceneName);
    }

    //select [Authentication] buttons of different security levels
    public void selectAuthentication(DestinationMarkerEventArgs e)
    {
        string authenticationSceneName = "";

        switch (e.target.name)
        {
            case "Button_LowSecurityAuth":
                if (AuthSetting.isSettingNull(AuthSetting.LowSecuritySetting.IsPasswordTypes))
                {
                    if (!GameObject.Find("Prompt_Auth2").GetComponent<TextMesh>().text.Contains("[Low]"))
                        GameObject.Find("Prompt_Auth2").GetComponent<TextMesh>().text += "\nPlease set the password of [Low] security level first.";
                    return;
                }
                authenticationSceneName = "LowSecurityAuthentication";
                //print(authenticationSceneName);
                break;

            case "Button_MediumSecurityAuth":
                if (AuthSetting.isSettingNull(AuthSetting.MediumSecuritySetting.IsPasswordTypes))
                {
                    if (!GameObject.Find("Prompt_Auth2").GetComponent<TextMesh>().text.Contains("[Medium]"))
                        GameObject.Find("Prompt_Auth2").GetComponent<TextMesh>().text += "\nPlease set the password of [Medium] security level first.";
                    return;
                }
                authenticationSceneName = "MediumSecurityAuthentication";
                //print(authenticationSceneName);
                break;

            case "Button_HighSecurityAuth":
                if (AuthSetting.isSettingNull(AuthSetting.HighSecuritySetting.IsPasswordTypes))
                {
                    if (!GameObject.Find("Prompt_Auth2").GetComponent<TextMesh>().text.Contains("[High]"))
                        GameObject.Find("Prompt_Auth2").GetComponent<TextMesh>().text += "\nPlease set the password of [High] security level first.";
                    return;
                }
                authenticationSceneName = "HighSecurityAuthentication";
                //print(authenticationSceneName);
                break;

            default:
                break;
        }
        SceneManager.LoadScene(authenticationSceneName);
    }

    //select [SetupPassType] buttons to set the password types of different security levels
    public void selectSetupPassType(DestinationMarkerEventArgs e)
    {
        //print(e.target.name);
        int typeInx = int.Parse(e.target.name.Replace("Button_SetupType", ""))-1;
        switch (sceneName)
        {
            case "LowSecuritySetup":
                LowSecuritySetup.setup.authValueButtonGen(typeInx);
                break;
            case "MediumSecuritySetup":
                MediumSecuritySetup.setup.authValueButtonGen(typeInx);
                break;
            case "HighSecuritySetup":
                HighSecuritySetup.setup.authValueButtonGen(typeInx);
                break;
            default:
                break;
        }
    }

    // select the [valueObj] to set the password values of different security levels
    public void selectSetupPassValue(DestinationMarkerEventArgs e)
    {
        
        int objnum = int.Parse(e.target.name.Replace("ValueObj", ""));
        int typeInx = int.Parse(e.target.parent.gameObject.name.Replace("SetupValueOfType", "")) - 1;
        bool isObjSelected = false;
        string promptText = "";
        switch (sceneName)
        {
            case "LowSecuritySetup":
                LowSecuritySetup.setup.IsPasswordValues[typeInx][objnum] = LowSecuritySetup.setup.IsPasswordValues[typeInx][objnum] ? false : true;
                isObjSelected = LowSecuritySetup.setup.IsPasswordValues[typeInx][objnum];
                promptText = LowSecuritySetup.setup.getPassValuePrompt(typeInx);
                break;
            case "MediumSecuritySetup":
                MediumSecuritySetup.setup.IsPasswordValues[typeInx][objnum] = MediumSecuritySetup.setup.IsPasswordValues[typeInx][objnum] ? false : true;
                isObjSelected = MediumSecuritySetup.setup.IsPasswordValues[typeInx][objnum];
                promptText = MediumSecuritySetup.setup.getPassValuePrompt(typeInx);
                break;
            case "HighSecuritySetup":
                HighSecuritySetup.setup.IsPasswordValues[typeInx][objnum] = HighSecuritySetup.setup.IsPasswordValues[typeInx][objnum] ? false : true;
                isObjSelected = HighSecuritySetup.setup.IsPasswordValues[typeInx][objnum];
                promptText = HighSecuritySetup.setup.getPassValuePrompt(typeInx);
                break;
            default:
                break;
        }

        foreach (Transform child in e.target.transform)
            if (child.tag == "SelectedMark")
                if (isObjSelected)
                    child.gameObject.SetActive(true);
                else
                    child.gameObject.SetActive(false);
        GameObject.Find("Prompt_PassValueSave2").GetComponent<TextMesh>().text = promptText;
    }


    public void selectSaveButton(DestinationMarkerEventArgs e)
    {
        // save the password
        if (e.target.name == "Button_SavePassword")
        {
            bool isPasswordFollowedRules = false;
            switch (sceneName)
            {
                case "LowSecuritySetup":
                    isPasswordFollowedRules = LowSecuritySetup.setup.isPasswordFollowedRules();
                    LowSecuritySetup.setup.savePassword();                    
                    break;
                case "MediumSecuritySetup":
                    isPasswordFollowedRules = MediumSecuritySetup.setup.isPasswordFollowedRules();
                    MediumSecuritySetup.setup.savePassword();
                    break;
                case "HighSecuritySetup":
                    isPasswordFollowedRules = HighSecuritySetup.setup.isPasswordFollowedRules();
                    HighSecuritySetup.setup.savePassword();
                    break;
                default:
                    break;
            }
            if (!isPasswordFollowedRules)
                if (!GameObject.Find("Prompt_PassSave2").GetComponent<TextMesh>().text.Contains("requirements"))
                    GameObject.Find("Prompt_PassSave2").GetComponent<TextMesh>().text += "\nDoes not meet the password setting requirements.";

        }

        // save the password value of type[typeInx] 
        if (e.target.name == "Button_SavePassValue")
        {
            Transform SetupValueOfType = GameObject.Find("ValueObj0").transform.parent;
            int typeInx = int.Parse(SetupValueOfType.name.Replace("SetupValueOfType", "")) - 1;
            switch (sceneName)
            {
                case "LowSecuritySetup":
                    LowSecuritySetup.setup.savePassValue(typeInx);
                    GameObject.Find("Prompt_PassSave2").GetComponent<TextMesh>().text = LowSecuritySetup.setup.getPasswordPrompt();
                    break;
                case "MediumSecuritySetup":
                    MediumSecuritySetup.setup.savePassValue(typeInx);
                    GameObject.Find("Prompt_PassSave2").GetComponent<TextMesh>().text = MediumSecuritySetup.setup.getPasswordPrompt();
                    break;
                case "HighSecuritySetup":
                    HighSecuritySetup.setup.savePassValue(typeInx);
                    GameObject.Find("Prompt_PassSave2").GetComponent<TextMesh>().text = HighSecuritySetup.setup.getPasswordPrompt();
                    break;
                default:
                    break;
            }
        }
    }

    private void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;

        if (GetComponent<VRTK_DestinationMarker>() == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerPointerEvents_ListenerExample", "VRTK_DestinationMarker", "the Controller Alias"));
            return;
        }

        //Setup controller event listeners
        GetComponent<VRTK_DestinationMarker>().DestinationMarkerEnter += new DestinationMarkerEventHandler(DoPointerIn);
        GetComponent<VRTK_DestinationMarker>().DestinationMarkerHover += new DestinationMarkerEventHandler(DoPointerHover);
        GetComponent<VRTK_DestinationMarker>().DestinationMarkerExit += new DestinationMarkerEventHandler(DoPointerOut);
        GetComponent<VRTK_DestinationMarker>().DestinationMarkerSet += new DestinationMarkerEventHandler(DoPointerDestinationSet);
    }


    private void DoPointerIn(object sender, DestinationMarkerEventArgs e)
    {
        
    }

    private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
    {

    }

    private void DoPointerHover(object sender, DestinationMarkerEventArgs e)
    {
        
    }

    private void DoPointerDestinationSet(object sender, DestinationMarkerEventArgs e)
    {
        //string output = "select" + e.target.name;
        //print(output);
        switch (e.target.tag)
        {
            case "AuthObj":
                selectAuthObj(e);
                break;
            case "Login":
                selectLogin(e);
                break;
            case "SetupButton":
                selectSetup(e);
                break;
            case "AuthenticationButton":
                selectAuthentication(e);
                break;
            case "SetupPassTypeButton":
                selectSetupPassType(e);
                PlayerMove.playerMoveToOrigin();
                break;
            case "SetupPassValueButton":
                selectSetupPassValue(e);
                break;
            case "SaveButton":
                selectSaveButton(e);
                PlayerMove.playerMoveToOrigin();
                break;
            case "Back":
                //return to the Index Page
                SceneManager.LoadScene("Index");
                PlayerMove.playerMoveToOrigin();
                break;
            default:
                break;
        }
            
        

    }

}
