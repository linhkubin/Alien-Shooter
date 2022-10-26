using Luna.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_V5 : MonoBehaviour
{
    #region Singleton
    public static GameManager_V5 Ins;
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

    public GameObject hor, ver;
    public GameObject bg_hor, bg_ver;
    private bool isHor;

    public GameObject h_startPanel, v_startPanel;
    public GameObject h_tut;
    public GameObject v_tut;

    public GameObject h_endCardPanel, v_endCardPanel;

    //-----------Variables-------------
    [HideInInspector] public bool canInteract = false;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool endGame = false;
    [HideInInspector] public bool isWin = false;

    private bool rotateScreen = false;

    //-----------Singleton Instance-------------
    private Player_V5 fighter;
    private EnemyManager_V5 enemyManager;
    private EndCard_V5 endCardManager;

    void Start()
    {
        h_startPanel.SetActive(true);
        v_startPanel.SetActive(true);
        h_tut.SetActive(false);
        v_tut.SetActive(false);
        h_endCardPanel.SetActive(false);
        v_endCardPanel.SetActive(false);


        fighter = Player_V5.Ins;
        enemyManager = EnemyManager_V5.Ins;
        endCardManager = EndCard_V5.Ins;

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
            if (Mathf.Abs(Screen.width / (float)Screen.height - 1125f / 2436f) < 0.1)
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
}
