using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public bool isDie = false;
    public bool haveBooster;
    public float health;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constant.BULLET_TAG)) 
        {
            TakeDamage(Constant.BULLET_DAMAGE);
        }
	}

	public virtual void TakeDamage(int damage)
	{
        health -= damage;
		
        if (health < 0)
		{
            EnemyManager.Ins.PlayExplosionSound();
            DestroyShip();
        }
	}

    public virtual void DestroyShip()
	{
        float rand = Random.Range(0, 1f);
        if (!EnemyManager.Ins.spawnedBooster && haveBooster)
		{
            if (EnemyManager.Ins.GetAliveHaveBooster() == 1)
			{
                Instantiate(EnemyManager.Ins.boosterPrefabs, transform.position, Quaternion.identity);
                EnemyManager.Ins.spawnedBooster = true;
            }
            else
			{
                if (rand < 0.8f)
                {
                    Instantiate(EnemyManager.Ins.boosterPrefabs, transform.position, Quaternion.identity);
                    EnemyManager.Ins.spawnedBooster = true;
                }
            }
		}
        else
		{
            if (rand < 0.1f)
            {
                ObjectPooling.Ins.SpawnFromPool(Constant.COIN_TAG, transform.position, Quaternion.identity);
            }
        }

        ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
           
        Destroy(gameObject);
	}
}
