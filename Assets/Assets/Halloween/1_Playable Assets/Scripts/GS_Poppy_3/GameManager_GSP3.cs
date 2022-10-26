using Luna.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_GSP3 : MonoBehaviour
{
    #region Singleton
    public static GameManager_GSP3 Ins;
    void Awake()
    {
        Ins = this;
        cam = Camera.main;
        originalCameraSize = cam.orthographicSize;
    }


    #endregion
    public AudioSource clickSound;

    public Camera cam;
    public float originalCameraSize;
    [LunaPlaygroundFieldStep(0.1f)]
    [LunaPlaygroundField("Fixed Xs Camera Size", 1, "Game Settings")]
    [Range(13, 15)]
    public float FixedXsCameraSize = 13.5f;

    public GameObject hor, ver;
    public GameObject bg_hor, bg_ver;
    private bool isHor;

    public GameObject h_introPanel, v_introPanel;
    public GameObject h_pickWings, v_pickWings;
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

    private bool isWing1;
    private int bulletIdx;

    //-----------Singleton Instance-------------
    private Player_GSP3 fighter;
    private Boss_GSP3 boss;

    void Start()
    {
        h_introPanel.SetActive(true);
        v_introPanel.SetActive(true);
        h_pickWings.SetActive(true);
        v_pickWings.SetActive(true);
        h_pickBullet.SetActive(false);
        v_pickBullet.SetActive(false);
        h_startPanel.SetActive(false);
        v_startPanel.SetActive(false);
        h_tut.SetActive(false);
        v_tut.SetActive(false);

        fighter = Player_GSP3.Ins;
        boss = Boss_GSP3.Ins;

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

    public void PickWing(bool wings1)
    {
        isWing1 = wings1;
        h_pickWings.SetActive(false);
        v_pickWings.SetActive(false);
        h_pickBullet.SetActive(true);
        v_pickBullet.SetActive(true);

        clickSound.Play();

        Analytics.LogEvent("Choose Wings", 0);
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

        boss.ShowBoss(true);
        fighter.SetBulletWings(bulletIdx, isWing1);
        fighter.MoveStartGame();

        Analytics.LogEvent("Choose Bullet", 0);
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

        Analytics.LogEvent(Analytics.EventType.LevelStart);
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
