using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class GameUIScript : MonoBehaviour
{
    public PlayerControllerScript localPlayer;
    public TurnManagerScript TurnManager;
    public TMP_Text WinnerText;
    public TMP_Dropdown ChallengeDropdown;
    public Button TurnButton;
    bool isProcessingTurn = false;
    int[] ChallengeValues = { 500, 1000, 2000 };

    void Update()
    {
        if (localPlayer == null && NetworkClient.localPlayer != null)
        {
            localPlayer = NetworkClient.localPlayer.GetComponent<PlayerControllerScript>();
        }

        if(localPlayer != null)
        {
            if(localPlayer.TurnManager != null && TurnManager == null)
            {
                TurnManager = localPlayer.TurnManager;
            }

            if(TurnManager != null)
            {
                bool myTurn = localPlayer.netIdentity.netId == TurnManager.CurrentTurnNetId;
                if (myTurn && !TurnButton.interactable)
                {
                    TurnButton.interactable = true;
                    isProcessingTurn = false;
                }
            }
        }
        

        if(TurnManager != null)
        {
            if(WinnerText.text != TurnManager.WinnerResult && TurnManager.WinnerResult != "")
            {
                WinnerText.text = TurnManager.WinnerResult;
            }
        }
    }

    public void OnJoinButton()
    {
        int bet = ChallengeValues[ChallengeDropdown.value];
        ChallengeDropdown.gameObject.SetActive(false);

        if(localPlayer != null)
        {
            localPlayer.CmdAddPlayer(bet);
        }
    }

    public void OnTakeTurnClicked()
    {
        if (isProcessingTurn)
        {
            return;
        }

        isProcessingTurn = true;
        TurnButton.interactable = false;
        localPlayer?.PlayerTurnBtn();
    }

    public void OnResetClicked()
    {
        if (TurnManager == null)
        {
            return;
        }
        TurnManager.Reset();
    }
}
