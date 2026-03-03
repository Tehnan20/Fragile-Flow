using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class AttemptUIScript : MonoBehaviour
{
    // TurnManagerScript TurnManager;
    public int UIPlayerId;
    public PlayerAttemptScript Player;
    public Image[] Boxes; // size 5
    public TMP_Text DebuTxt;

    void Update()
    {
        if (Player == null)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                FindPlayer();
            }
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
        var local = NetworkClient.localPlayer.GetComponent<PlayerControllerScript>();
        TurnManagerScript tm = local.TurnManager;
        
        if (tm == null) return;

        if (tm.PlayerNetIds.Count <= UIPlayerId)
            return;

        uint targetNetId = tm.PlayerNetIds[UIPlayerId];

        foreach (var p in FindObjectsOfType<PlayerAttemptScript>())
        {
            if (p.netIdentity.netId == targetNetId)
            {
                Player = p;
                break;
            }
        }
    }
}
