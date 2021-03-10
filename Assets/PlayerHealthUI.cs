using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    PlayerHealth playerHealth;

    public GameObject TRPiece;
    public GameObject TLPiece;
    public GameObject BRPiece;
    public GameObject BLPiece;

    private void Update()
    {
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
            }
        }

        if (playerHealth == null)
        {
            DisableAllPieces();
        } else
        {
            if (playerHealth.GetHealth() == 0)
            {
                DisableAllPieces();
            } else
            {
                EnableAllPieces();

                if (playerHealth.GetHealth() <= 3)
                {
                    BRPiece.SetActive(false);
                }
                
                if(playerHealth.GetHealth() <= 2)
                {
                    BLPiece.SetActive(false);
                }


                if (playerHealth.GetHealth() <= 1)
                {
                    TRPiece.SetActive(false);
                }

            }
        }
    }

    void EnableAllPieces()
    {
        TRPiece.SetActive(true);
        TLPiece.SetActive(true);
        BRPiece.SetActive(true);
        BLPiece.SetActive(true);
    }

    void DisableAllPieces()
    {
        TRPiece.SetActive(false);
        TLPiece.SetActive(false);
        BRPiece.SetActive(false);
        BLPiece.SetActive(false);
    }
}
