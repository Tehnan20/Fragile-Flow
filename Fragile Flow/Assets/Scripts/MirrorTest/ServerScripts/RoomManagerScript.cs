using Mirror;
using UnityEngine;
using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading;

public class Room
{
    public List<NetworkIdentity> Players = new List<NetworkIdentity>();
    public TurnManagerScript TurnManager;
    public Guid MatchId;
    [ReadOnly]
    public int ChallengeAmount;
}

public class RoomManagerScript : MonoBehaviour
{
    public int RoomId = 0;
    List<Room> ActiveRooms = new List<Room>();
    public TurnManagerScript TurnManagerPrefab;
    public PlayerControllerScript PlayerPrefab;
    public Dictionary<int, List<NetworkIdentity>> WaitingByAmount = new Dictionary<int, List<NetworkIdentity>>();
    TimerScript BotTimer = new TimerScript(15f);
    int BotChallengeAmount;
    // public int MaxPlayers = 2;

    // public override void OnStartServer()
    // {
    //     // if (WaitingPlayers.Count > 0)
    //     // {
    //     //     WaitingPlayers.Clear();
    //     // }
    //     if (ActiveRooms.Count > 0)
    //     {
    //         ActiveRooms.Clear();
    //     }
    // }

    [Server]
    public void AddPlayer(NetworkIdentity player, int challengeAmount)
    {
        if (!WaitingByAmount.ContainsKey(challengeAmount))
        {
            WaitingByAmount[challengeAmount] = new List<NetworkIdentity>();
        }

        var list = WaitingByAmount[challengeAmount];

        // player.transform.GetComponent<PlayerAttemptScript>().PlayerNameText.text = "list.Count: " + list.Count;
        if (list.Contains(player))
        {
            return;
        }

        list.Add(player);

        if (list.Count == 2)
        {
            BotTimer.Stop();
            Debug.Log("TIMER STOPPED");

            var p1 = list[0];
            var p2 = list[1];

            list.Clear();

            CreateRoom(p1, p2, challengeAmount);
        }
        else
        if(list.Count == 1)
        {
            BotChallengeAmount = challengeAmount;
            BotTimer.Start();
            Debug.Log("TIMER STARTED");
        }

        // if (WaitingPlayers.Contains(player))
        // {
        //     return;
        // }

        // WaitingPlayers.Add(player);

        // if (WaitingPlayers.Count == MaxPlayers)
        // {
        //     // BotTimer.Stop();
        //     // Debug.Log("TIMER STOPPED");
        //     CreateRoom(WaitingPlayers[0], WaitingPlayers[1]);
        // }
        // else
        // if(WaitingPlayers.Count == 1)
        // {
        //     // Debug.Log("TIMER STARTED");
        //     // BotTimer.Start();
        // }
    }

    void Update()
    {
        BotTimer.Run(Time.deltaTime);
        if(BotTimer.GetCurrentSeconds() <= 0)
        {
            Debug.Log("TIMER STOPPED");
            SpawnBot(BotChallengeAmount);
            BotTimer.Reset();
        }
    }

    [Server]
    void CreateRoom(NetworkIdentity p1, NetworkIdentity p2, int challengeAmount)
    {
        // if (!p1 || !p2) return;
        Room room = new Room();
        room.MatchId = Guid.NewGuid();
        room.ChallengeAmount = challengeAmount;

        p1.GetComponent<NetworkMatch>().matchId = room.MatchId;
        p2.GetComponent<NetworkMatch>().matchId = room.MatchId;

        GameObject parent = new GameObject("Room: " + RoomId);
        RoomId++;

        TurnManagerScript turnManager = Instantiate(TurnManagerPrefab, parent.transform);
        turnManager.GetComponent<NetworkMatch>().matchId = room.MatchId;
        NetworkServer.Spawn(turnManager.gameObject);

        room.TurnManager = turnManager;
        room.TurnManager.RoomManager = this;

        p1.transform.SetParent(parent.transform);
        p2.transform.SetParent(parent.transform);

        p1.GetComponent<PlayerControllerScript>().TurnManagerIdentity = room.TurnManager.netIdentity;
        p2.GetComponent<PlayerControllerScript>().TurnManagerIdentity = room.TurnManager.netIdentity;

        p1.transform.position += new Vector3(Random.value * 3, p1.transform.position.y, Random.value * 3);
        p2.transform.position += new Vector3(Random.value * -3, p1.transform.position.y, Random.value * -3);

        room.Players.Add(p1);
        room.Players.Add(p2);

        room.TurnManager.RegisterPlayer(p1);
        room.TurnManager.RegisterPlayer(p2);

        room.TurnManager.InitializeMatch();

        ActiveRooms.Add(room);
        // WaitingPlayers.Clear();
    }

    [Server]
    public void RemovePlayer(NetworkIdentity player)
    {
        foreach (var room in ActiveRooms)
        {
            if (room.Players.Contains(player))
            {
                room.Players.Remove(player);
                room.TurnManager.RemovePlayer(player);

                if (room.Players.Count == 0)
                {
                    // GameObject.Destroy(room.TurnManager.transform.parent);
                    // TurnManagers.Remove(room.TurnManager);
                    NetworkServer.Destroy(room.TurnManager.gameObject);
                }

                return;
            }
        }
    }

    [Server]
    void SpawnBot(int botChallengeAmount)
    {
        PlayerAttemptScript bot = Instantiate(PlayerPrefab.GetComponent<PlayerAttemptScript>());
        bot.IsBot = true;
        NetworkServer.Spawn(bot.gameObject);

        AddPlayer(bot.netIdentity, botChallengeAmount);
    }
}