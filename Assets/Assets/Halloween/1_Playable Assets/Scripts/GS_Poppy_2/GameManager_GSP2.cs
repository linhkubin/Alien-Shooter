using Luna.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_GSP2 : MonoBehaviour
{
    #region Singleton
    public static GameManager_GSP2 Ins;
    void Awake()
    {
        Ins = this;
        cam = Camera.main;
        originalCameraSize = cam.orthographicSize;
    }
    #endregion

    public Camera cam;
    public float originalCameraSize;
    [LunaPlaygroundFieldStep(0.1f)]
    [LunaPlaygroundField("Fixed Xs Camera Size", 1, "Game Settings")]
    [Range(13, 15)]
    public float FixedXsCameraSize = 13.5f;

    public AudioSource clickSound;

    public GameObject hor, ver;
    public GameObject bg_hor, bg_ver;
    private bool isHor;

    public GameObject h_introPanel, v_introPanel;
    public GameObject h_pickBoss, v_pickBoss;
    public GameObject h_pickBullet, v_pickBullet;

    public GameObject h_startPanel, v_startPanel;
    public GameObject h_tut, v_tut;

    public GameObject h_vfx_red, v_vfx_red;

    //-----------Variables-------------
    [HideInInspector] public bool canInteract = false;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool endGame = false;
    [HideInInspector] public bool isWin = false;

    private bool rotateScreen = false;

    private bool isBossBlue;
    private int bulletIdx;

    //-----------Singleton Instance-------------
    private Player_GSP2 fighter;
    private Boss_GSP2 boss;

    void Start()
    {
        h_introPanel.SetActive(true);
        v_introPanel.SetActive(true);
        h_pickBoss.SetActive(true);
        v_pickBoss.SetActive(true);
        h_pickBullet.SetActive(false);
        v_pickBullet.SetActive(false);
        h_startPanel.SetActive(false);
        v_startPanel.SetActive(false);
        h_tut.SetActive(false);
        v_tut.SetActive(false);

        fighter = Player_GSP2.Ins;
        boss = Boss_GSP2.Ins;

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
            if (Math.Abs(Screen.width / (float)Screen.height - 1125f / 2436f) < 0.1)
            {
                cam.orthographicSize = FixedXsCameraSize;
            }
            else
            {
                cam.orthographicSize = originalCameraSize;
            }

            isHor = false;

            if (rotateScreen)
            {
                rotateScreen = false;
                fighter.movement.SetupMouseLimit(Constant.HORIZONTAL_MOUSE_X_LIMIT, Constant.HORIZONTAL_MOUSE_Y_LIMIT);
            }
        }
        else
        {
            cam.orthographicSize = originalCameraSize;
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
    }

    public void GoToStore()
    {
        LifeCycle.GameEnded();
        Playable.InstallFullGame();
    }

    public void PickBoss(bool isBlueBoss)
    {
        isBossBlue = isBlueBoss;
        h_pickBoss.SetActive(false);
        v_pickBoss.SetActive(false);
        h_pickBullet.SetActive(true);
        v_pickBullet.SetActive(true);

        clickSound.Play();
    }

    public void PickBullet(int idx)
    {
        clickSound.Play();

        bulletIdx = idx;
        h_pickBullet.SetActive(false);
        v_pickBullet.SetActive(false);

        h_introPanel.SetActive(false);
        v_introPanel.SetActive(false);

        h_startPanel.SetActive(true);
        v_startPanel.SetActive(true);

        boss.SetBoss(isBossBlue);
        fighter.SetBullet(bulletIdx);
        fighter.MoveStartGame();
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

    public void ShowRedVFX(bool canShow)
    {
        h_vfx_red.SetActive(canShow);
        v_vfx_red.SetActive(canShow);
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
