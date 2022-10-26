using UnityEngine;
using DG.Tweening;
public class EnemyManager : MonoBehaviour
{
    public AudioSource explosion_Sound;

    public GameObject boosterPrefabs;

    public Transform leftPos, rightPos;

    public bool spawnedBooster = false;

    public SpaceShip[] listHaveBooster;
    public EnemyBlue[] enemies;
    public int enemyAlive;


    #region Singleton
    public static EnemyManager Ins;
    void Awake()
    {
        Ins = this;
    }
	#endregion

	private void Start()
	{
        enemyAlive = enemies.Length;	
	}

    public void PlayExplosionSound()
	{
        explosion_Sound.PlayOneShot(explosion_Sound.clip);
	}

	public void EnemyStart()
	{
        foreach (EnemyBlue e in enemies)
		{
            e.canMove = true;
		}
	}

    public void WaitToWin()
	{
        Invoke(nameof(EnemyWinGame), 1f);
	}

    void EnemyWinGame()
    {
        for (int i = 0; i<enemies.Length; i++)
		{
            if (enemies[i] != null)
			{
                enemies[i].MoveToInitPos();
			}
		}
    }

    public int GetAliveHaveBooster()
	{
        int numAlive = 0;
        for (int i = 0; i < listHaveBooster.Length; i++)
		{
            if (listHaveBooster[i] != null)
			{
                numAlive++;
			}
		}
        return numAlive;
	}

    public int GetAliveEnemy()
    {
        int numAlive = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                numAlive++;
            }
        }
        return numAlive;
    }
}
