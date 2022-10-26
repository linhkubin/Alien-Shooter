using System.Collections;
using UnityEngine;
using DG.Tweening;

public abstract class Spaceship_V5 : MonoBehaviour
{
    public float health;

    public virtual void MoveToPos(Transform pos, float moveDuration)
    {
        transform.DOMove(pos.position, moveDuration).SetEase(Ease.Linear);
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.BULLET_TAG) || collision.CompareTag(Constant.BULLET_GALA_TAG))
        {
            TakeDamage(Constant.BULLET_DAMAGE);
        }

        else if (collision.CompareTag(Constant.PLAYER_TAG))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            EnemyManager_R1_Remake_2.Ins.PlayExplosionSound();
            DestroyShip();
        }
    }

    public abstract void DestroyShip();
}
