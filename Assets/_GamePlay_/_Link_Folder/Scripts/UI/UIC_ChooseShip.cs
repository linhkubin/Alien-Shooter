using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIC_ChooseShip : MonoBehaviour
{
    public void ChooseButton_Ship_1()
    {
        gameObject.SetActive(false);
        Ply_Level.Ins.ship.ChangeShip(ShipType.Ship_1); 
        Ply_Level.Ins.Play();
    }

    public void ChooseButton_Ship_2()
    {
        gameObject.SetActive(false);
        Ply_Level.Ins.ship.ChangeShip(ShipType.Ship_2);
        Ply_Level.Ins.Play();
    }
}
