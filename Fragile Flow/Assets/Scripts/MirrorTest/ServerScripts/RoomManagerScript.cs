using Mirror;
using UnityEngine;
using Guid = System.Guid;
using System.Collections.Generic;

public class Room
{
    public List<NetworkIdentity> Players = new List<NetworkIdentity>();
    public TurnManagerScript TurnManager;
    public Guid MatchId;
}

public class RoomManagerScript : NetworkBehaviour
{
    List<Room> ActiveRooms = new List<Room>();
    public TurnManagerScript TurnManagerPrefab;
    public List<NetworkIdentity> WaitingPlayers = new List<NetworkIdentity>();
    public int MaxPlayers = 2;

    public override void OnStartServer()
    {
        if(WaitingPlayers.Count > 0)
        {
            WaitingPlayers.Clear();
        }
        if(ActiveRooms.Count > 0)
        {
            ActiveRooms.Clear();
        }
    }

    [Server]
    public void AddPlayer(NetworkIdentity player)
    {
        if (WaitingPlayers.Contains(player))
        {
            return;
        }

        WaitingPlayers.Add(player);

        if (WaitingPlayers.Count == MaxPlayers)
        {
            WaitingPlayers.Clear();
            CreateRoom(WaitingPlayers[0], WaitingPlayers[1]);
        }
    }

    [Server]
    void CreateRoom(NetworkIdentity p1, NetworkIdentity p2)
    {
        Room room = new Room();
        room.MatchId = Guid.NewGuid();

        TurnManagerScript turnManager = Instantiate(TurnManagerPrefab);
        NetworkServer.Spawn(turnManager.gameObject);

        room.TurnManager = turnManager;

        room.Players.Add(p1);
        room.Players.Add(p2);

        room.TurnManager.RegisterPlayer(p1);
        room.TurnManager.RegisterPlayer(p2);

        p1.GetComponent<NetworkMatch>().matchId = room.MatchId;
        p2.GetComponent<NetworkMatch>().matchId = room.MatchId;
        room.TurnManager.GetComponent<NetworkMatch>().matchId = room.MatchId;

        ActiveRooms.Add(room);
    }

    [Server]
    public void RemovePlayer(NetworkIdentity player)
    {
        foreach (var room in ActiveRooms)
        {
            if (room.Players.Contains(player))
            {
                room.Players.Remove(player);

                if (room.Players.Count == 0)
                {
                    NetworkServer.Destroy(room.TurnManager.gameObject);
                }

                return;
            }
        }
    }
}