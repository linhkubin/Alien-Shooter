using Luna.Unity;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_R1P : MonoBehaviour
{
    #region Singleton
    public static GameManager_R1P Ins;
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

    private bool rotateScreen = false;

    //-----------Singleton Instance-------------
    private Player_R1P fighter;
    private Boss boss;
    //private EndCard_R1 endCardManager;

    // Edit 26/04/2022
    public SpriteRenderer shipRender;
    public  Sprite redShip;
    public Sprite blueShip;
    public GameObject ship;

    public GameObject BossR1;

    public GameObject CanvasChooseShip;

    public GameObject CanvasGame;

    public bool canplay = false;

    public GameObject HorSelectShip, VerSelectShip;
    void Start()
    {
        h_startPanel.SetActive(true);
        v_startPanel.SetActive(true);
        h_tut.SetActive(false);
        v_tut.SetActive(false);

        fighter = Player_R1P.Ins;
        boss = Boss.Ins;
        //endCardManager = EndCard_R1.Ins;

        LifeCycle.OnMute += MuteAudio;
        LifeCycle.OnUnmute += UnmuteAudio;
    }

    void Update()
    {
        if (canInteract && Input.GetMouseButtonDown(0) && !startGame )
        {
            //if(canplay == true)
            //{
                StartGame();
            //}
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

            //HorSelectShip.SetActive(true);
            //VerSelectShip.SetActive(false);
        }
        else
        {
            isHor = true;
            if (!rotateScreen)
            {
                rotateScreen = true;
                fighter.movement.SetupMouseLimit(Constant.VERTICAL_MOUSE_X_LIMIT, Constant.VERTICAL_MOUSE_Y_LIMIT);
            }

            //HorSelectShip.SetActive(false);
            //VerSelectShip.SetActive(true);
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

    //public void selectRedShip()
    //{
    //    BossR1.transform.DOMoveY(6.5f, 1f);
    //    ship.transform.DOMoveY(-5, 1f).OnComplete(() => {
    //        canInteract = true;
    //        ShowTut();
    //    });

    //    CanvasGame.SetActive(true);

    //    CanvasChooseShip.SetActive(false);

    //    shipRender.sprite = redShip;

    //    canplay = true;
    //}

    //public void selectBlueShip()
    //{
    //    BossR1.transform.DOMoveY(6.5f, 1f);
    //    CanvasGame.SetActive(true);
    //    ship.transform.DOMoveY(-5, 1f).OnComplete(() => {
    //        canInteract = true;
    //        ShowTut();
    //    });
    //    shipRender.sprite = blueShip;
    //    CanvasChooseShip.SetActive(false);
    //    canplay = true;
    //}
}
