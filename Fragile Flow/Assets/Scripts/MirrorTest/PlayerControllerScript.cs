using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControllerScript : NetworkBehaviour
{
    public TurnManagerScript TurnManager;

    public float Speed = 5f;

    Renderer PlayerRenderer;

    void Start()
    {
        PlayerRenderer = GetComponent<Renderer>();
    }

    public override void OnStartServer()
    {
        TurnManager = FindFirstObjectByType<TurnManagerScript>();
        TurnManager.RegisterPlayer(netIdentity);
    }
    public override void OnStartClient()
    {
        TurnManager = FindFirstObjectByType<TurnManagerScript>();
    }

    void Update()
    {
        if (IsMyTurn())
        {
            PlayerRenderer.material.color = Color.green;
        }
        else
        {
            PlayerRenderer.material.color = Color.white;
            return;
        }
        if (!isLocalPlayer)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            CmdMove(h, v);
        }
        CmdRotate(Input.GetAxis("Mouse X"));
    }

    public void PlayerTurnBtn()
    {
        if (!IsMyTurn() || !isLocalPlayer)
        {
            return;
        }
        GetComponent<PlayerAttemptScript>().CmdTakeAttempt();
        TurnManager.CheckWinner();
        EndTurn();
    }

    public bool IsMyTurn()
    {
        return netIdentity.netId == TurnManager.CurrentTurnNetId;
    }

    // Command runs from client to server
    [Command]
    public void CmdMove(float h, float v)
    {
        Vector3 move = new Vector3(h, 0, v);
        transform.Translate(move * Speed * Time.deltaTime);
    }
    [Command]
    void CmdRotate(float mouseX)
    {
        transform.Rotate(Vector3.up * mouseX * 300f * Time.deltaTime);
    }

    public void EndTurn()
    {
        if (!isLocalPlayer) return;
        TurnManager.CmdEndTurn();
    }
}
