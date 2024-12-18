using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0 )
        {
            return;
        }
        time = 1f;

        if (Input.GetKey(KeyCode.W)) 
        {

            Debug.Log("WWWW");
            ServerConnect.Instance.clientInfo[ServerConnect.Instance.myClientIndex].mTransform += new Vector3(0, 10 * Time.deltaTime, 0);

            ServerConnect.Instance.EnqueueSendData(ServerConnect.Instance.packetData[(int)ServerUtil.Header.HeaderType.INGAME].Serialzed(((int)ServerUtil.Header.ConnectionState.INGAME_MOVE).ToString()));
        }

    }
}
