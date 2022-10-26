using Luna.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCard_GSP1 : MonoBehaviour
{
    #region Singleton
    public static EndCard_GSP1 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion
    public AudioSource explosionSound;

    public GameObject h_EndCardPanel, v_EndCardPanel;
    public GameObject h_winPanel, v_winPanel;

    public EnemyEndcard_GSP1[] enemies;
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
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].targetPos = endCardPos[i];
        }
    }

    public void WaitToShowEndCard(float time)
    {
        explosionSound.Play();
        Player_GSP2.Ins.WaitToTurnOffBullet(3f);

        StartEndCardWin();
        Invoke(nameof(ShowEndCardWin), time);
        
    }

    public void StartEndCardWin()
    {
        StartCoroutine(IE_StartEnemy());
    }

    public void ShowEndCardWin()
    {
        h_EndCardPanel.SetActive(true);
        v_EndCardPanel.SetActive(true);
        h_winPanel.SetActive(true);
        v_winPanel.SetActive(true);

        Analytics.LogEvent(Analytics.EventType.EndCardShown);
    }

    IEnumerator IE_StartEnemy()
    {
        foreach (EnemyEndcard_GSP1 e in enemies)
        {
            yield return new WaitForSeconds(0.1f);
            e.MoveToPos();
        }
    }
}
