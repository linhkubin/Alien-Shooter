using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EndCard_R1P : MonoBehaviour
{
    #region Singleton
    public static EndCard_R1P Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion
    public AudioSource explosionSound;

    public GameObject h_EndCardPanel, v_EndCardPanel;
    public GameObject h_winPanel, v_winPanel;
    public GameObject h_LosePanel, v_LosePanel;

    public EnemyEndCard_R1P[] enemies;
    public Transform[] endCardPos;

    public Transform[] movePath;

    [HideInInspector]
    public bool canStartEndcard = false;

    void Start()
    {
        h_EndCardPanel.SetActive(false);
        v_EndCardPanel.SetActive(false);

        SetupEnemyEndCard();
    }

    void SetupEnemyEndCard()
	{
        for (int i = 0; i<enemies.Length; i++)
		{
            enemies[i].targetPos = endCardPos[i];
        }
	}

    public void WaitToShowEndCard(float time, bool isWin)
	{
        explosionSound.Play();
        Player_R1P.Ins.WaitToTurnOffBullet(3f);

        if (isWin)
		{
            StartEndCardWin();
            Invoke(nameof(ShowEndCardWin), time);
		}
        else
		{
            Invoke(nameof(ShowEndCardLose), time);
        }
	}

    public void StartEndCardWin()
	{
        //transform.DOMoveY(0, 4f).SetEase(Ease.Linear);
        StartCoroutine(IE_StartEnemy());
	}

    public void ShowEndCardWin()
	{
        h_EndCardPanel.SetActive(true);
        v_EndCardPanel.SetActive(true);
        h_winPanel.SetActive(true);
        v_winPanel.SetActive(true);
        h_LosePanel.SetActive(false);
        v_LosePanel.SetActive(false);
    }

    public void ShowEndCardLose()
    {
        h_EndCardPanel.SetActive(true);
        v_EndCardPanel.SetActive(true);
        h_winPanel.SetActive(false);
        v_winPanel.SetActive(false);
        h_LosePanel.SetActive(true);
        v_LosePanel.SetActive(true);
    }

    IEnumerator IE_StartEnemy()
	{
        foreach (EnemyEndCard_R1P e in enemies)
		{
            yield return new WaitForSeconds(0.1f);
            e.MoveToPos();
		}
	}
}
