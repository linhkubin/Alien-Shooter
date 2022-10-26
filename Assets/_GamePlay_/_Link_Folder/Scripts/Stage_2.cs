using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_2 : Ply_Stage
{
    public Transform[] startPoint;
    public Transform[] hearthPoints;

    public EnemyType[] enemyType;

    public Transform enemyContainer;

    public override void OnInit()
    {
        for (int i = 0; i < hearthPoints.Length; i++)
        {
            Ply_Enemy enemy = Ply_Pool.Ins.Spawn<Ply_Enemy>(PoolType.Enemy, startPoint[i % 2].position, Quaternion.identity);
            enemy.tf.SetParent(enemyContainer);
            enemy.OnInit(this, i, enemyType[i], 6);
            Vector3[] path = { startPoint[i % 2].position, hearthPoints[i].position };
            enemy.StartMove(path, 1f, DG.Tweening.PathType.CatmullRom);
        }

        //loop
        Invoke(nameof(Loop), 1f);
        Invoke(nameof(FireBullet), 1.5f);
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
        MoveEnemyRect();
    }

    private void MoveEnemyRect()
    {
        if (enemieAlives.Count > 0)
        {
            enemyContainer.DOMoveX(0.5f, 2.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                enemyContainer.DOMoveX(-0.5f, 2.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Loop();
                });
            });

        }
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

}
