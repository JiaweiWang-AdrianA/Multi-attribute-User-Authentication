using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerMoveUp : MonoBehaviour
{
    public VRTK.VRTK_ControllerEvents vrtkCrtlEvents;

    //move up
    private void playerMoveUpEvent()
    {
        GameObject player;
        Vector3 playerUp;
        //player = GameObject.Find("[CameraRig]");
        try
        {
            player = GameObject.Find("[CameraRig]");
            GameObject player_eye = GameObject.Find("Camera (eye)");
            playerUp = player_eye.transform.up;
        }
        catch
        {
            player = GameObject.Find("[VRSimulator_CameraRig]");
            playerUp = player.transform.up;
            //print("player is VRSimulator" );
        }

        player.transform.Translate(playerUp * Time.deltaTime * 4f, Space.Self);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (vrtkCrtlEvents.gripPressed)
            playerMoveUpEvent();
        //GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(gripPressed_MovementEvent);

    }

}
