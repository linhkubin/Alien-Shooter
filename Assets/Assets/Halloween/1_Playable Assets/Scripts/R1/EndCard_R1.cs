using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndCard_R1 : MonoBehaviour
{
    #region Singleton
    public static EndCard_R1 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public Transform bossEndCard;
    public Transform playerEndCard;

    public Enemy_Encard_R1[] enemies_EndCard;
    public Transform[] pos;

    private GameManager_R1_RemakeV1 gameManager;

   


    void Start()
    {
        gameManager = GameManager_R1_RemakeV1.Ins;
    }

    public void SetupEndCard()
    {
        bossEndCard.transform.DOMoveY(4.5f, 1f);

        playerEndCard.gameObject.SetActive(true);
        playerEndCard.DOMoveY(-7f, 0.5f);

        playerEndCard.DOScale(Vector3.one * 1.5f, 0.75f).OnComplete(() =>
        {
            playerEndCard.DOScale(Vector3.one, 0.75f);
        });

        for (int i = 0; i < enemies_EndCard.Length; i++)
        {
            enemies_EndCard[i].gameObject.SetActive(true);
            enemies_EndCard[i].curMove = i;
            enemies_EndCard[i].MoveToPos(enemies_EndCard[i].initPos);
        }

        Invoke(nameof(ShowEndCard), 0.5f);
    }

    

    public void ShowEndCard()
    {
        StartCoroutine(IE_MoveEnemyBorder());
        gameManager.h_endCardPanel.SetActive(true);
        gameManager.v_endCardPanel.SetActive(true);
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
        yield return new WaitForSeconds(1f);
        MoveEnemyEndCard();
        yield return new WaitForSeconds(1f);
        StartCoroutine(IE_MoveEnemyBorder());
    }
}
