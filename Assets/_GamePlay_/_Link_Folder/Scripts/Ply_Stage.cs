using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply_Stage : MonoBehaviour
{
    protected List<Ply_Enemy> enemieAlives = new List<Ply_Enemy>();

    public virtual void OnInit()
    {

    }

    public virtual void Add(Ply_Enemy enemy)
    {
        enemieAlives.Add(enemy);
    }

    public virtual void Remove(Ply_Enemy enemy)
    {
        enemieAlives.Remove(enemy);

        if (enemieAlives.Count <= 0)
        {
            Ply_Level.Ins.NextStage();
        }
    }
}
