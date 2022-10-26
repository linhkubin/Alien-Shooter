using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCard_R2_3 : MonoBehaviour
{
    #region Singleton
    public static EndCard_R2_3 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public Transform bossEndCard;

    public EnemyEndCard_R2_3[] leftEnemies, rightEnemies;
    public Transform leftPos, rightPos;

    private GameManager_R2_3 gameManager;

    void Start()
    {
        gameManager = GameManager_R2_3.Ins;
    }

    public void ShowEndCard()
	{
        SetupEnemies(leftEnemies);
        SetupEnemies(rightEnemies);
    }

    private void SetupEnemies(EnemyEndCard_R2_3[] enemies)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            StartCoroutine(IE_SetupEnemy(enemies[i], i / 10f));
        }

        Invoke(nameof(MoveBoss), 2.5f);
        Invoke(nameof(StartShooting), 3f);
    }

    public void StartShooting()
    {
        foreach (EnemyEndCard_R2_3 e in leftEnemies)
        {
            e.StartShooting();
        }

        foreach (EnemyEndCard_R2_3 e in rightEnemies)
        {
            e.StartShooting();
        }
    }

    IEnumerator IE_SetupEnemy(EnemyEndCard_R2_3 e, float idx)
    {
        yield return new WaitForSeconds(0.5f + idx);
        e.MoveToPos();
    }

    private void MoveBoss()
	{
        bossEndCard.transform.DOMoveY(6.5f, 1f);
    }
}
