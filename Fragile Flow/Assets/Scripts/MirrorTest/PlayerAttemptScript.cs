using Mirror;
using UnityEngine;
using TMPro;

public class PlayerAttemptScript : NetworkBehaviour
{
    [SyncVar]
    public int PlayerId;
    [SyncVar]
    public int CurrentAttempt = 0;
    [SyncVar]
    public int Wins = 0;
    [SyncVar]
    string PlayerName;
    public TMP_Text PlayerNameText;

    // 0 = none, 1 = win, 2 = lose
    public SyncList<int> Results = new SyncList<int>();

    public override void OnStartServer()
    {
        PlayerId = connectionToClient.connectionId + 1;

        // Debug.Log("Player ID: " + PlayerId);

        for (int i = 0; i < 5; i++)
        {
            Results.Add(0);
        }
    }
    public override void OnStartClient()
    {
        PlayerName = "Player " + PlayerId;
        PlayerNameText.text = PlayerName;
    }

    [Command]
    public void CmdTakeAttempt()
    {
        if (CurrentAttempt >= 5) return;

        bool win = Random.value > 0.5f;

        if (win)
        {
            Results[CurrentAttempt] = 1;
            Wins++;
        }
        else
        {
            Results[CurrentAttempt] = 2;
        }

        CurrentAttempt++;
    }

    [Server]
    public void Reset()
    {
        CurrentAttempt = 0;
        Wins = 0;
        for (int i = 0; i < Results.Count; i++)
        {
            Results[i] = 0;
        }
    }
}
