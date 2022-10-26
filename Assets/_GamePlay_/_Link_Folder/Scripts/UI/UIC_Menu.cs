using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIC_Menu : MonoBehaviour
{
    private void OnEnable()
    {
        Ply_Level.Ins.ship.HoldEffect(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ply_GameManager.gameState = GameState.GamePlay;
            gameObject.SetActive(false);
            Ply_Level.Ins.ship.HoldEffect(false);
        }
    }
}
