using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameScript : MonoBehaviour
{
    PlayerPiece playerPiece;
    private CountDownTimer countDownTimer;
    PlayerTurnManager playerTurnManager;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        playerPiece = FindObjectOfType<PlayerPiece>();
        countDownTimer = FindObjectOfType<CountDownTimer>();
        playerTurnManager = FindObjectOfType<PlayerTurnManager>();
    }

    public void quitGame(int Player)
    {
        
        if (isPlayerTurn(Player))
        {
            playerTurnManager.quitChangePlayerTurn();
        }

        playerPiece.quitPlayer(Player);
        countDownTimer.displayBotPlayedCount();
    }

    int getIndex()
    {
        int index = GameManager.gm.playerTurn;
        if (SameMarker.Instance.getPlayerArray().Count == 2)
        {
            if (index == 3)
                index = 1;
            else
                index = index - 1;
        }
        else
        {
            index = index - 1;
        }

        return index;
    }

    bool isPlayerTurn(int Player)
    {
        if (SameMarker.Instance.getPlayerArray().Count == 2)
        {
            if (Player == 3)
                Player = 1;
            else
                Player = Player - 1;
        }
        else
        {
            Player = Player - 1;
        }

        return getIndex() == Player;
    }

}
