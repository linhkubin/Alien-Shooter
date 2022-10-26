using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EnemyType { Enemy_1, Enemy_2}

public class Ply_Enemy : Ply_GameUnit
{
    public Ply_Stage stage;
    public GameObject[] enemyTypes;
    private int currentType = -1;

    [HideInInspector] public int id;
    public float hp;

    Vector3 targetPoint;

    public Transform skin;
    bool immortal = false;


    public void OnInit(Ply_Stage stage, int id, EnemyType enemyType, float hp)
    {
        this.stage = stage;
        this.id = id;
        this.hp = hp;

        stage.Add(this);

        if (currentType != (int)enemyType)
        {
            if (currentType >= 0)
            {
                enemyTypes[currentType].SetActive(false);
            }
            currentType = (int)enemyType;
        }

        enemyTypes[currentType].SetActive(true);

    }

    public void Fire()
    {
        Ply_Pool.Ins.Spawn(PoolType.Enemy_Bullet, tf.position, Quaternion.identity);
    }

    public void TakeDamage(int damage)
    {
        if (immortal)
        {
            return;
        }

        hp -= damage;

        if (hp < 0)
        {
            //EnemyManager_R1_Remake_2.Ins.PlayExplosionSound();
            Ply_Pool.Ins.Spawn( PoolType.VFX_Explore, tf.position, Quaternion.identity);
            Ply_Pool.Ins.Despawn(PoolType.Enemy, this);
            Ply_SoundManager.Ins.PlayFx(FxType.EnemyDie);

            stage.Remove(this);
        }
    }

    public void MoveToTarget(Vector3 targetPoint, float time, UnityAction onCompleteCallback)
    {
        transform.DOMove(targetPoint, time).SetEase(Ease.Linear).OnComplete(() => { onCompleteCallback?.Invoke(); });
    }

    public void StartMove(Vector3[] path, float time, PathType pathType)
    {
        immortal = true;

        tf.DOPath(path, time, pathType).SetEase(Ease.Linear).OnComplete(() => {
            immortal = false;
        });
    }

    public void MoveUpDown(float time = 1f)
    {
        float endValue = 0.25f * ((id % 2) - 0.5f) * 2;
        float dt = time / 3;

        skin.DOLocalMoveY(endValue, dt).OnComplete(() =>
        {
            skin.DOLocalMoveY(-endValue, dt).OnComplete(() =>
            {
                skin.DOLocalMoveY(0, dt);
            });
        });
    }
}
