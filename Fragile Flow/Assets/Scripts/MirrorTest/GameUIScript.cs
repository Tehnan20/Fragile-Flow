using UnityEngine;
using Mirror;

public class GameUIScript : MonoBehaviour
{
    PlayerControllerScript localPlayer;
    public TurnManagerScript TurnManager;

    void Update()
    {
        if (localPlayer == null && NetworkClient.localPlayer != null)
            localPlayer = NetworkClient.localPlayer.GetComponent<PlayerControllerScript>();
    }

    public void OnTakeTurnClicked()
    {
        localPlayer?.PlayerTurnBtn();
    }

    public void OnResetClicked()
    {
        TurnManager.Reset();
    }
}
