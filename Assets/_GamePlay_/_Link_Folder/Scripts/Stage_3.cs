using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_3 : Ply_Stage
{
    public Transform shipPoint;
    public Transform startPoint;

    public Transform[] pathPoints;
    public Transform[] rowPoints;

    public EnemyType[] enemyType;


    public override void OnInit()
    {
        for (int i = 0; i < rowPoints.Length; i++)
        {
            List<Vector3> path = new List<Vector3>();
            path.Add(startPoint.position);
            for (int j = 0; j < pathPoints.Length; j++)
            {
                path.Add(pathPoints[j].position);
            }
            path.Add(rowPoints[i].position);


            Ply_Enemy enemy = Ply_Pool.Ins.Spawn<Ply_Enemy>(PoolType.Enemy, startPoint.position, Quaternion.identity);
            enemy.OnInit(this, i, enemyType[i], 6);

            enemy.StartMove(path.ToArray(), 2f, DG.Tweening.PathType.CatmullRom);
        }

        //di chuyen ve giua man hinh
        //ket thuc game
        Ply_GameManager.gameState = GameState.MainMenu;
        Ply_Level.Ins.ship.Moving(shipPoint.position, 0.7f, 
        () =>
        {
            Ply_UIManager.Ins.OpenUI(UIID.Win);
            Ply_Level.Ins.ship.HoldEffect(true);
        });

    }

    public override void Add(Ply_Enemy enemy)
    {
        base.Add(enemy);
    }

    public override void Remove(Ply_Enemy enemy)
    {
        base.Remove(enemy);
        enemy.tf.SetParent(null);
    }

    public void Loop()
    {
    }

}
