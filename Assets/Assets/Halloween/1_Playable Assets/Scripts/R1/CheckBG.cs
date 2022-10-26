using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBG : MonoBehaviour
{
    public GameObject bgH;
    public GameObject bgV;
    public CheckBG checkBG;
    public GameObject Joker;

    public GameObject Gameplay;

    public bool nextAction = false;

    public float timeGoGameplay;
    public float maxTimeGoGameplay;

    public GameObject PurpleShip;
    public GameObject BlueShip;
    public GameObject Action1;

    public GameObject SelectH;
    public GameObject SelectV;
    // Update is called once per frame
    void Update()
    {
        if(Screen.height > Screen.width)
        {
            bgH.SetActive(true);
            bgV.SetActive(false);
            SelectH.SetActive(true);
            SelectV.SetActive(false);
        }
        else if(Screen.width > Screen.height)
        {
            bgH.SetActive(false);
            bgV.SetActive(true);
            SelectH.SetActive(false);
            SelectV.SetActive(true);
        }

        //timeGoGameplay += Time.deltaTime;

        //if(timeGoGameplay >= maxTimeGoGameplay && nextAction == false)
        //{
        //    TurnOffJoker();
        //}
    }

    public void ChoosePurpleShip()
    {
        MoveToGameplay();
        PurpleShip.SetActive(true);
        BlueShip.SetActive(false);
    }
    public void ChooseBlueShip()
    {
        MoveToGameplay();
        PurpleShip.SetActive(false);
        BlueShip.SetActive(true);
    }

    public void MoveToGameplay()
    {
        checkBG.enabled = false;
        Gameplay.SetActive(true);
        nextAction = true;
        Action1.SetActive(false);
    }
}
