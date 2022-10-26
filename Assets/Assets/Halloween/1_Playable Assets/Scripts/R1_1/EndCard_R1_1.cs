using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCard_R1_1 : MonoBehaviour
{
    #region Singleton
    public static EndCard_R1_1 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public Transform bossEndCard;
    public Transform playerEndCard;

    public EnemyEndCard_R1_1[] enemies_EndCard;
    public Transform[] pos;

    private GameManager_R1_Remake_2 gameManager;

    public GameObject h_endCardPanel, v_endCardPanel;
    void Start()
    {
        gameManager = GameManager_R1_Remake_2.Ins;
    }

    public void SetupEndCard()
    {
        bossEndCard.transform.DOMoveY(7.5f, 0.75f);

        playerEndCard.gameObject.SetActive(true);
        playerEndCard.DOMoveY(-7f, 0.5f);

        playerEndCard.DOScale(Vector3.one * 1.5f, 0.75f).OnComplete(() => {
            playerEndCard.DOScale(Vector3.one, 0.75f);
        });

		for (int i = 0; i < enemies_EndCard.Length; i++)
		{
			enemies_EndCard[i].gameObject.SetActive(true);
			enemies_EndCard[i].curMove = i;
			enemies_EndCard[i].MoveToPos(enemies_EndCard[i].initPos);
		}

		Invoke(nameof(ShowEndCard), 1f);
        Invoke(nameof(StartShooting), 1f);
    }

    public void ShowEndCard()
    {
        h_endCardPanel.SetActive(true);
        v_endCardPanel.SetActive(true);
        StartCoroutine(IE_MoveEnemyBorder());
        Debug.Log(gameManager.h_endCardPanel.name);
    }

    public void StartShooting()
    {
        foreach (EnemyEndCard_R1_1 e in enemies_EndCard)
        {
            e.StartShooting();
        }
    }

    public void MoveEnemyEndCard()
    {
        for (int i = 0; i < enemies_EndCard.Length; i++)
        {
            enemies_EndCard[i].curMove += 1;
            int nextMove = enemies_EndCard[i].curMove % enemies_EndCard.Length;

            enemies_EndCard[i].MoveToPos(pos[nextMove]);
        }
    }
    IEnumerator IE_MoveEnemyBorder()
    {
        yield return new WaitForSeconds(0.5f);
        MoveEnemyEndCard();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(IE_MoveEnemyBorder());
    }
}
