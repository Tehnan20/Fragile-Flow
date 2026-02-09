using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class AttemptUIScript : MonoBehaviour
{
    public int UIPlayerId;
    public PlayerAttemptScript Player;
    public Image[] Boxes; // size 5

    void Update()
    {
        if (Player == null)
        {
            FindPlayer();
            return;
        }
        
        for (int i = 0; i < Boxes.Length; i++)
        {
            if (Player.Results[i] == 1)
            {
                Boxes[i].color = Color.green;
            }
            else if (Player.Results[i] == 2)
            {
                Boxes[i].color = Color.red;
            }
            else
            {
                Boxes[i].color = Color.gray;
            }
        }
    }

    void FindPlayer()
    {
        var players = FindObjectsOfType<PlayerAttemptScript>();
        foreach (var p in players)
        {
            if (p.PlayerId == UIPlayerId)
            {
                Player = p;
                break;
            }
        }
    }
}
