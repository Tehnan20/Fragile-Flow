using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerControllerScript : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnTurnManagerChanged))]
    public NetworkIdentity TurnManagerIdentity;
    public TurnManagerScript TurnManager;
    public RoomManagerScript RoomManager;
    [SyncVar]
    public int ChallengeAmount;

    // public float Speed = 5f;

    Renderer PlayerRenderer;

    void Start()
    {
        PlayerRenderer = GetComponent<Renderer>();
    }
    public override void OnStartLocalPlayer()
    {
        
    }

    public override void OnStartServer()
    {
        RoomManager = FindFirstObjectByType<RoomManagerScript>();
        // TurnManager = FindFirstObjectByType<TurnManagerScript>();
    }
    public override void OnStartClient()
    {
        RoomManager = FindFirstObjectByType<RoomManagerScript>();
        // TurnManager = FindFirstObjectByType<TurnManagerScript>();
    }
    public override void OnStopServer()
    {
        if(RoomManager != null)
        {
            RoomManager.RemovePlayer(netIdentity);
        }
        // if (TurnManager != null)
        // {
        //     TurnManager.RemovePlayer(netIdentity);
        // }
    }

    void Update()
    {
        if (TurnManager == null)
        {
            return;
        }
        if (IsMyTurn())
        {
            if(PlayerRenderer.material.color == Color.white)
            {
                PlayerRenderer.material.color = Random.ColorHSV();
            }
        }
        else
        {
            if(PlayerRenderer.material.color != Color.white)
            {
                PlayerRenderer.material.color = Color.white;
            }
            return;
        }
        if (!isLocalPlayer)
        {
            return;
        }

        // float h = Input.GetAxis("Horizontal");
        // float v = Input.GetAxis("Vertical");

        // if (h != 0 || v != 0)
        // {
        //     CmdMove(h, v);
        // }
        // CmdRotate(Input.GetAxis("Mouse X"));
    }

    [Command]
    public void CmdAddPlayer(int challengeAmount)
    {
        ChallengeAmount = challengeAmount;
        RoomManager.AddPlayer(netIdentity, challengeAmount);
    }

    public void PlayerTurnBtn()
    {
        if (TurnManager == null || !IsMyTurn())
        {
            return;
        }
        GetComponent<PlayerAttemptScript>().CmdTakeAttempt();
        // TurnManager.EndTurn();
        // TurnManager.CheckWinner();
    }

    public bool IsMyTurn()
    {
        return netIdentity.netId == TurnManager.CurrentTurnNetId;
    }

    [Command]
    public void EndTurn()
    {
        TurnManager.EndTurn();
    }

    void OnTurnManagerChanged(NetworkIdentity oldVal, NetworkIdentity newVal)
    {
        if (newVal != null)
        {
            TurnManager = newVal.GetComponent<TurnManagerScript>();
        }
    }

    // Command runs from client to server
    // [Command]
    // public void CmdMove(float h, float v)
    // {
    //     Vector3 move = new Vector3(h, 0, v);
    //     transform.Translate(move * Speed * Time.deltaTime);
    // }
    // [Command]
    // void CmdRotate(float mouseX)
    // {
    //     transform.Rotate(Vector3.up * mouseX * 300f * Time.deltaTime);
    // }
}
