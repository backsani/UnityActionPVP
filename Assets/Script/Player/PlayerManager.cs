using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Transform Player1;
    [SerializeField] private Transform Player2;
    [SerializeField] private Camera mainCamera;
    private void Start()
    {
        Debug.Log(ServerConnect.Instance.myClientIndex);
        if (ServerConnect.Instance.myClientIndex == 0)
        {
            Player1.transform.AddComponent<PlayerController>();
            mainCamera.transform.SetParent(Player1);
            mainCamera.transform.localPosition = new Vector3(0,0,-10);
        }
        else if (ServerConnect.Instance.myClientIndex == 1)
        {
            Player2.transform.AddComponent<PlayerController>();
            mainCamera.transform.SetParent(Player2);
            mainCamera.transform.localPosition = new Vector3(0, 0, -10);
        }
    }

    private void Update()
    {
        Player1.transform.position = ServerConnect.Instance.clientInfo[0].mTransform;
        Player2.transform.position = ServerConnect.Instance.clientInfo[1].mTransform;
    }
}
