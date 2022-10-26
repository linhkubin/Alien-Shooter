using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager_V2 : MonoBehaviour
{
    #region Singleton
    public static EnemyManager_V2 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public AudioSource explosion_Sound;
    public GameObject boosterPrefabs;

    public Transform[] targetPositions;

    public Transform[] path1, path2, path3, path4;

    public Enemy_V2[] team1, team2, team3, team4;

    public SpaceShip_V2[] listHaveBooster;

    public bool spawnedBooster = false;

    private float speedTeam1 = 5f, speedTeam2 = 7.5f;

    public void PlayExplosionSound()
    {
        explosion_Sound.PlayOneShot(explosion_Sound.clip);
    }

    void MoveEnemyTeam(Enemy_V2[] team, Transform target, float speed)
	{
        foreach(Enemy_V2 e in team)
		{
            e.targetPos = target;
            e.canMove = true;
            e.speed = speed;
		}
	}

    void MoveEnemyPath(Enemy_V2[] team, Transform[] path, float time, float speed)
    {
        for (int i = 0; i < team.Length;i++)
        {
            team[i].canMove = true;
            team[i].speed = speed;
            team[i].FollowPath(path, time + i/2f);
        }
    }

    public void StartMovingTeam()
	{
        StartCoroutine(IE_MoveEnemyTeam());
	}

    IEnumerator IE_MoveEnemyTeam()
	{
        yield return new WaitForSeconds(3f);
        if (Player_V2.Ins != null)
        {
            //MoveEnemyTeam(team1, targetPositions[0], speedTeam1);
            MoveEnemyPath(team1, path1, 8f ,speedTeam1);
        }
        else EndCard_V2.Ins.WaitToRevive();

        yield return new WaitForSeconds(0f);

        if (Player_V2.Ins != null)
        {
            //MoveEnemyTeam(team2, targetPositions[1], speedTeam1);
            MoveEnemyPath(team2, path2, 8f, speedTeam2);
        } else 
        {
            EndCard_V2.Ins.WaitToRevive();
        }

        yield return new WaitForSeconds(7f);

        if (Player_V2.Ins != null)
        {
            //MoveEnemyTeam(team3, targetPositions[2], speedTeam2);
            MoveEnemyPath(team3, path3, 3f, speedTeam2);
            //MoveEnemyTeam(team4, targetPositions[3], speedTeam2);
            MoveEnemyPath(team4, path4, 3f, speedTeam2);
        }
        else
		{
            EndCard_V2.Ins.WaitToRevive();
        }

        yield return new WaitForSeconds(15f);
        if (Player_V2.Ins != null)
        {
            Player_V2.Ins.WaitToWin();
        }
        else {
            EndCard_V2.Ins.WaitToRevive(); 
        }
    }
}
