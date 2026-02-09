using Mirror;
using UnityEngine;
using TMPro;

public class GameTimerScript : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnTimeChanged))]
    public float RemainingTime = 5f;
    
    public TMP_Text TimerText;

    public override void OnStartServer()
    {
        RemainingTime = 5f;
    }

    void Update()
    {
        if (!isServer) return;
        
        RemainingTime -= Time.deltaTime;

        if (RemainingTime <= 0f)
        {
            PlayerControllerScript localPlayer = NetworkClient.localPlayer.GetComponent<PlayerControllerScript>();
            localPlayer.EndTurn();

            RemainingTime = 5f;
        }
    }

    void OnTimeChanged(float oldTime, float newTime)
    {
        TimerText.text = "Timer: " + Mathf.Ceil(newTime).ToString();
    }
}