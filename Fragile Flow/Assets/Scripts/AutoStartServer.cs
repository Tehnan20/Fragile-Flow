using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class AutoStartServer : MonoBehaviour
{
    void Start()
    {
        if (Application.isBatchMode)
        {
            Debug.Log("Batchmode detected -> Starting Mirror server");
            NetworkManager.singleton.StartServer();
        }
    }
}