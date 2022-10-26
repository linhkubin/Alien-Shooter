using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_1 : Ply_Stage
{
    public Transform shipPoint;

    public Transform startPoint;
    public Transform[] rectangePoints;
    public Transform centerPoint;

    private List<Ply_Enemy> es = new List<Ply_Enemy>();

    int indexRect = 0;

    int booster = 2;

    public override void OnInit()
    {
        //enemy
        for (int i = 0; i < rectangePoints.Length; i++)
        {
            Ply_Enemy enemy = Ply_Pool.Ins.Spawn<Ply_Enemy>(PoolType.Enemy, startPoint.position, Quaternion.identity);
            enemy.OnInit(this, i, EnemyType.Enemy_1, 7);
            Vector3[] path = { startPoint.position, rectangePoints[i].position };
            enemy.StartMove(path, 1f, DG.Tweening.PathType.Linear);
            es.Add(enemy);
        }

        //boss
        Ply_Enemy enemyCenter = Ply_Pool.Ins.Spawn<Ply_Enemy>(PoolType.Enemy, startPoint.position, Quaternion.identity);
        enemyCenter.tf.localScale = Vector3.one * 2.7f;
        enemyCenter.OnInit(this, 0, EnemyType.Enemy_2, 50);
        Vector3[] pathCenter = { startPoint.position, centerPoint.position };
        enemyCenter.StartMove(pathCenter, 1f, DG.Tweening.PathType.Linear);

        //loop
        Invoke(nameof(Loop), 1f);
        Invoke(nameof(FireBullet), 1.5f);

        //di chuyen ve giua man hinh
        Ply_Level.Ins.ship.Moving(shipPoint.position, .7f,
            () =>
            {
                Ply_UIManager.Ins.OpenUI(UIID.Menu);
            });
    }

    public override void Add(Ply_Enemy enemy)
    {
        base.Add(enemy);
    }

    public override void Remove(Ply_Enemy enemy)
    {
        base.Remove(enemy);
        enemy.tf.localScale = Vector3.one;

        //tao booster
        if (booster > 0 && Random.Range(0, 100) > 80)
        {
            booster--;
            Ply_Pool.Ins.Spawn(PoolType.Booster, enemy.tf.position, Quaternion.identity);
        }
    }

    public void Loop()
    {
        MoveEnemyRect(++indexRect);
    }

    public void FireBullet()
    {
        if (enemieAlives.Count > 0)
        {
            int rand = Random.Range(0, enemieAlives.Count / 4);

            for (int i = 0; i < rand; i++)
            {
                int index = Random.Range(0, enemieAlives.Count);
                Ply_Pool.Ins.Spawn(PoolType.Enemy_Bullet, enemieAlives[index].tf.position, Quaternion.identity);
            }

            Invoke(nameof(FireBullet), Random.Range(1.5f, 3.5f));
        }

    }

    private void MoveEnemyRect(int index)
    {
        if (enemieAlives.Count > 0)
        {
            for (int i = 0; i < es.Count; i++)
            {
                if (es[i].gameObject.activeInHierarchy)
                {
                    int indexTarget = (index + i) % rectangePoints.Length;
                    Ply_Enemy e = es[i];
                    e.MoveToTarget(rectangePoints[indexTarget].position, 0.75f, 
                    () =>
                    {
                        e.MoveUpDown(1f);
                    });
                }
            }

            Invoke(nameof(Loop), 1.75f);
        }
    }

}
