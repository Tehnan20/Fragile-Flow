using Mirror;
using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerAttemptScript : NetworkBehaviour
{
    // [SyncVar]
    // public int PlayerId;
    [SyncVar]
    public int CurrentAttempt = 0;
    [SyncVar]
    public int Wins = 0;
    // [SyncVar]
    // string PlayerName;
    public TMP_Text PlayerNameText;
    public TurnManagerScript TurnManager;
    public bool IsBot = false;
    TimerScript BotTurnTimer = new TimerScript(1f);

    // 0 = none, 1 = win, 2 = lose
    public SyncList<int> Results = new SyncList<int>();

    public override void OnStartServer()
    {
        for (int i = 0; i < 5; i++)
        {
            Results.Add(0);
        }
    }
    public override void OnStartClient()
    {
        // PlayerName = "Player " + PlayerId;
        // PlayerNameText.text = PlayerName;
    }

    void Update()
    {
        BotTurnTimer.Run(Time.deltaTime);
        if(BotTurnTimer.GetCurrentSeconds() <= 0)
        {
            BotTurnTimer.Reset();
            ServerTakeAttempt();
        }
        if(TurnManager != null)
        {
            if(netIdentity.netId == TurnManager.CurrentTurnNetId)
            {
                if(IsBot == true)
                {
                    // StartCoroutine(BotTakeTurn());
                    BotTurnTimer.Start();
                }
            }
        }
    }

    [Command]
    public void CmdTakeAttempt()
    {
        ServerTakeAttempt();
    }

    [Server]
    public void ServerTakeAttempt()
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

        TurnManager.EndTurn();
    }

    // IEnumerator BotTakeTurn()
    // {
    //     yield return new WaitForSeconds(1.5f);

    //     ServerTakeAttempt();
    //     TurnManager.EndTurn();
    // }

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
