using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EndCard_V2 : MonoBehaviour
{
    #region Singleton
    public static EndCard_V2 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public GameObject enemy_Endcard;
    public GameObject enemyPos_Endcard;

    public Transform leftPos, rightPos;

    public Enemy_EndCard[] enemyEndCard;

    public Transform revivePlayer;

    void Start()
    {
        enemy_Endcard.SetActive(false);
        enemyPos_Endcard.SetActive(false);  
    }

    public void WaitToEndCard()
	{
        Invoke(nameof(EndCardSetup), 1f);
	}

    public void EndCardSetup()
	{
        enemy_Endcard.SetActive(true);
        enemyPos_Endcard.SetActive(true);

        foreach(Enemy_EndCard e in enemyEndCard)
		{
            e.canMove = true;
		}

        GameManager_V2.Ins.WaitToEndCard(2f);
    }

    public void WaitToRevive()
	{
        Invoke(nameof(RevivePlayer), 3f);
        Invoke(nameof(EndCardSetup), 3f);
    }

    void RevivePlayer()
	{
        revivePlayer.gameObject.SetActive(true);
        revivePlayer.DOMoveY(-7, 1);
	}
}
