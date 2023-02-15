using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerMove : MonoBehaviour
{
    public VRTK.VRTK_ControllerEvents vrtkCrtlEvents;

    //move forward
    private void playerMoveEvent()
    {
        GameObject player;
        Vector3 playerForword;
        //player = GameObject.Find("[CameraRig]");
        try
        {
            player = GameObject.Find("[CameraRig]");
            GameObject player_eye = GameObject.Find("Camera (eye)");
            playerForword = player_eye.transform.forward;
        }
        catch
        {
            player = GameObject.Find("[VRSimulator_CameraRig]");
            playerForword = player.transform.forward;
            //print("player is VRSimulator" );
        }

        player.transform.Translate(playerForword * Time.deltaTime * 4f, Space.Self);
    }

    //move to origin
    public static void playerMoveToOrigin()
    {
        GameObject player = GameObject.Find("[VRSimulator_CameraRig]");
        //player = GameObject.Find("[CameraRig]");
        try
        {
            player = GameObject.Find("[CameraRig]");
            player.transform.position = new Vector3(0f, 1.25f, 0f);
        }
        catch
        {
            player = GameObject.Find("[VRSimulator_CameraRig]");
            player.transform.position = new Vector3(0f, 0f, 0f);
            //print("player is VRSimulator" );
        }

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (vrtkCrtlEvents.gripPressed)
            playerMoveEvent();
        //GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(gripPressed_MovementEvent);

    }

}
