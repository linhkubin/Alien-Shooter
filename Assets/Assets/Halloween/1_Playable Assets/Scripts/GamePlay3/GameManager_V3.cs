using Luna.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_V3 : MonoBehaviour
{
    #region Singleton
    public static GameManager_V3 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public GameObject hor, ver;
    public GameObject bg_hor, bg_ver;
    private bool isHor;

    public GameObject h_startPanel, v_startPanel;
    public GameObject h_tut;
    public GameObject h_tut_txt;
    public GameObject v_tut;
    public GameObject v_tut_txt;

    public GameObject h_endCardPanel, v_endCardPanel;

    //-----------Variables-------------
    [HideInInspector] public bool canInteract = false;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool endGame = false;
    [HideInInspector] public bool isWin = false;

    private bool rotateScreen = false;

    //-----------Singleton Instance-------------
    private Player_V3 fighter;
    private EnemyManager enemyManager;
    private EndCardManager endCardManager;

    void Start()
    {
        h_startPanel.SetActive(true);
        v_startPanel.SetActive(true);
        h_tut.SetActive(false);
        v_tut.SetActive(false);
        h_tut_txt.SetActive(false);
        v_tut_txt.SetActive(false);
        h_endCardPanel.SetActive(false);
        v_endCardPanel.SetActive(false);


        fighter = Player_V3.Ins;
        enemyManager = EnemyManager.Ins;
        endCardManager = EndCardManager.Ins;

        enemyManager.EnemyStart();

        LifeCycle.OnMute += MuteAudio;
        LifeCycle.OnUnmute += UnmuteAudio;
    }

    void Update()
    {
        if (canInteract && Input.GetMouseButtonDown(0) && !startGame)
        {
            StartGame();
        }
    }

    private void FixedUpdate()
    {
        if (Screen.width < Screen.height)
        {
            isHor = false;

            if (rotateScreen)
            {
                rotateScreen = false;
                fighter.movement.SetupMouseLimit(Constant.HORIZONTAL_MOUSE_X_LIMIT, Constant.HORIZONTAL_MOUSE_Y_LIMIT);
            }
        }
        else
        {
            isHor = true;
            if (!rotateScreen)
            {
                rotateScreen = true;
                fighter.movement.SetupMouseLimit(Constant.VERTICAL_MOUSE_X_LIMIT, Constant.VERTICAL_MOUSE_Y_LIMIT);
            }
        }

        hor.SetActive(!isHor);
        ver.SetActive(isHor);
        bg_hor.SetActive(!isHor);
        bg_ver.SetActive(isHor);

        if (!isWin && EnemyManager.Ins.GetAliveEnemy() == 0)
        {
            isWin = true;
            fighter.WaitToWin();
        }
    }

    public void GoToStore()
    {
        LifeCycle.GameEnded();
        Playable.InstallFullGame();
    }

    public void ShowTut()
    {
        h_tut.SetActive(true);
        v_tut.SetActive(true);

        h_tut_txt.SetActive(true);
        v_tut_txt.SetActive(true);
    }

    public void StartGame()
    {
        h_startPanel.SetActive(false);
        v_startPanel.SetActive(false);

        fighter.StartShooting();

        startGame = true;
    }

    private void ShowEndCard()
    {
        h_endCardPanel.SetActive(true);
        v_endCardPanel.SetActive(true);
        endCardManager.SetupEndcard();
    }

    public void WaitToShowEndCard(float time)
    {
        Invoke(nameof(ShowEndCard), time);
    }

    public void EndGame()
    {
        if (!isWin) enemyManager.WaitToWin();
        WaitToShowEndCard(2f);
    }

    void MuteAudio()
    {
        AudioListener.volume = 0;
    }

    void UnmuteAudio()
    {
        AudioListener.volume = 1;
    }
}
