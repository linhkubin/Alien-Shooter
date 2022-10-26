using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luna.Unity;

public class GameManagerR1BLH : MonoBehaviour
{
    #region Singleton
    public static GameManagerR1BLH Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public GameObject hor, ver;
    public GameObject bg_hor, bg_ver;
    private bool isHor;

    public GameObject h_startPanel, v_startPanel;
    public GameObject h_tut, v_tut;

    //-----------Variables-------------
    [HideInInspector] public bool canInteract = false;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool endGame = false;
    [HideInInspector] public bool isWin = false;
    public bool isLose = false;

    public bool bossIsDie = false;
    private bool rotateScreen = false;

    //-----------Singleton Instance-------------
    private PlayerR1BLH fighter;
    private BossR1BulletHell boss;
    //private EndCard_R1 endCardManager;
    public AudioSource bossDie;
    public GameObject BossR1BLH;
    public float timingShowEndCard;
    public float maxTimingShowEndCard;
    public GameObject endCard1;
    public GameObject endCard2;
    public GameObject SpineBullet;
    public bool isClick;
    public float timeCounter;
    public bool canPlay;
    public AudioSource homingLauncherSound;

    public GameObject CanvasHor;

    public GameObject CanvasVer;

    public bool canMove;

    public GameObject winPanelHor;

    public GameObject winPanelVer;
    void Start()
    {
        h_startPanel.SetActive(true);
        v_startPanel.SetActive(true);
        h_tut.SetActive(false);
        v_tut.SetActive(false);

        fighter = PlayerR1BLH.Ins;
        boss = BossR1BulletHell.Ins;
        //endCardManager = EndCard_R1.Ins;

        LifeCycle.OnMute += MuteAudio;
        LifeCycle.OnUnmute += UnmuteAudio;
    }

    void Update()
    {
        if(canPlay == true)
        {
            if (canInteract && Input.GetMouseButtonDown(0) && !startGame)
            {
                StartGame();
                isClick = true;
            }
        }

        if (isClick == true && timeCounter >= 0)
        {
            timeCounter += Time.deltaTime;

            if (timeCounter >= 0.5f)
            {
                timeCounter = -1;

                SpineBullet.SetActive(true);
            }
        }

        if (bossIsDie == true)
        {
            bossDie.enabled = true;

            WaitToTurnOnWinPanel(2.3f);
        }
        if (fighter.health <= 0)
        {
            isLose = true;

            timingShowEndCard += Time.deltaTime;
            if(timingShowEndCard >= maxTimingShowEndCard)
            {
                endCard1.SetActive(true);
                endCard2.SetActive(true);
            }
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

            CanvasHor.SetActive(true);
            CanvasVer.SetActive(false);
        }
        else
        {
            isHor = true;
            if (!rotateScreen)
            {
                rotateScreen = true;
                fighter.movement.SetupMouseLimit(Constant.VERTICAL_MOUSE_X_LIMIT, Constant.VERTICAL_MOUSE_Y_LIMIT);
            }

            CanvasHor.SetActive(false);
            CanvasVer.SetActive(true);
        }

        hor.SetActive(!isHor);
        ver.SetActive(isHor);
        bg_hor.SetActive(!isHor);
        bg_ver.SetActive(isHor);

        //if (!isWin)
        //{
        //    isWin = true;
        //    fighter.TurnOffBullet();
        //}
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
    }

    public void StartGame()
    {
        h_startPanel.SetActive(false);
        v_startPanel.SetActive(false);

        fighter.StartShooting();
        boss.StartSkill();

        startGame = true;
    }

    void MuteAudio()
    {
        AudioListener.volume = 0;
    }

    void UnmuteAudio()
    {
        AudioListener.volume = 1;
    }

    public void WaitToTurnOnWinPanel(float time)
    {
        Invoke(nameof(checkWinScreenPanel), time);
    }

    public void checkWinScreenPanel()
    {
        if (Screen.width < Screen.height)
        {
            winPanelHor.SetActive(true);
            winPanelVer.SetActive(false);
        }
        else
        {
            winPanelHor.SetActive(false);
            winPanelVer.SetActive(true);
        }
    }
}
