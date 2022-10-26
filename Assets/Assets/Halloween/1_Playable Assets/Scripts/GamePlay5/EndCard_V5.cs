using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCard_V5 : MonoBehaviour
{
    #region Singleton
    public static EndCard_V5 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public Enemy_EndCard_V5[] enemies_EndCard;

    private GameManager_V5 gameManager;

    void Start()
    {
        gameManager = GameManager_V5.Ins;
    }

    public void SetupEndCard()
    {
        foreach (Enemy_EndCard_V5 e in enemies_EndCard)
        {
            e.gameObject.SetActive(true);
            e.MoveToPos();
        }

        Invoke(nameof(MoveEnemyEndCard), 1f);
    }

    public void WaitToShowEndCard(float time)
    {
        Player_V5.Ins.MoveEndGame();
        Invoke(nameof(ShowEndCard), time);
    }

    public void ShowEndCard()
    {
        SetupEndCard();
        gameManager.h_endCardPanel.SetActive(true);
        gameManager.v_endCardPanel.SetActive(true);
    }

    void MoveEnemyEndCard()
    {
        for (int i = 1; i < enemies_EndCard.Length; i++)
        {
            StartCoroutine(IE_MoveEnemyEndcard(enemies_EndCard[i].transform));
        }
    }

    IEnumerator IE_MoveEnemyEndcard(Transform enemy)
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));
        float rand = Random.Range(0, 1f);
        float curY = enemy.localPosition.y;

        if (rand > 0.75f)
        {
            enemy.DOLocalMoveY(curY + 0.1f, 0.25f).OnComplete(() =>
            {
                enemy.DOLocalMoveY(curY - 0.2f, 0.5f).OnComplete(() =>
                {
                    enemy.DOLocalMoveY(curY, 0.25f);
                });
            });
        }
        else if (rand < 0.25f)
        {
            enemy.DOLocalMoveY(curY - 0.1f, 0.5f).OnComplete(() =>
            {
                enemy.DOLocalMoveY(curY + 0.2f, 0.25f).OnComplete(() =>
                {
                    enemy.DOLocalMoveY(curY, 0.25f);
                });
            });
        }

        StartCoroutine(IE_MoveEnemyEndcard(enemy));
    }
}
