using Mirror;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TurnManagerScript : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnTurnChanged))]
    public int CurrentTurnIndex = 0;
    [SyncVar]
    public uint CurrentTurnNetId;

    public List<NetworkIdentity> Players = new List<NetworkIdentity>();

    [SyncVar(hook = nameof(OnWinnerChanged))]
    public string WinnerResult;
    public TMP_Text WinnerText;

    public void RegisterPlayer(NetworkIdentity player)
    {
        if (!isServer) return;

        Players.Add(player);

        // int newId = Players.Count;
        player.GetComponent<PlayerAttemptScript>().PlayerId = Players.Count;

            
        // if(Players[0].GetComponent<PlayerAttemptScript>().PlayerId != newId)
        // {
        //     player.GetComponent<PlayerAttemptScript>().PlayerId = newId;
        // }
        // else
        // {
        //     player.GetComponent<PlayerAttemptScript>().PlayerId = 1;
        // }

        if (Players.Count == 1)
        {
            CurrentTurnNetId = player.netId;
        }
    }
    [Server]
    public void RemovePlayer(NetworkIdentity player)
    {
        if (Players.Contains(player))
            Players.Remove(player);

        if (Players.Count == 0)
            return;

        if (CurrentTurnIndex >= Players.Count)
            CurrentTurnIndex = 0;

        CurrentTurnNetId = Players[CurrentTurnIndex].netId;
    }

    [Command(requiresAuthority = false)]
    public void CmdEndTurn()
    {
        if (Players.Count == 0)
        {
            return;
        }

        CurrentTurnIndex = (CurrentTurnIndex + 1) % Players.Count;

        CurrentTurnNetId = Players[CurrentTurnIndex].netId;
    }

    // [Server]
    [Command(requiresAuthority=false)]
    public void CheckWinner()
    {
        if(Players.Count < 2)
        {
            return;
        }
        PlayerAttemptScript player1 = Players[0].GetComponent<PlayerAttemptScript>();
        PlayerAttemptScript player2 = Players[1].GetComponent<PlayerAttemptScript>();

        if (player1.CurrentAttempt < 5 || player2.CurrentAttempt < 5)
        {
            return;
        }
        
        if (player1.Wins > player2.Wins)
        {
            WinnerResult = "Player 1 Wins";
        }
        else if (player2.Wins > player1.Wins)
        {
            WinnerResult = "Player 2 Wins";
        }
        else
        {
            WinnerResult = "Draw";
        }
    }

    // [Server]
    [Command(requiresAuthority=false)]
    public void Reset()
    {
        WinnerResult = " ";

        foreach (var player in Players)
        {
            PlayerAttemptScript pas = player.GetComponent<PlayerAttemptScript>();
            pas.Reset();
        }
    }

    void OnTurnChanged(int oldTurn, int newTurn)
    {
        Debug.Log("Turn changed to player index: " + newTurn);
    }
    void OnWinnerChanged(string oldVal, string newVal)
    {
        WinnerText.text = newVal;
    }
}
