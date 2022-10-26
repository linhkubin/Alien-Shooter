using Luna.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager_GS1 : MonoBehaviour
{
    #region Singleton
    public static GameManager_GS1 Ins;
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
    public GameObject h_warning, v_warning;
    public GameObject h_tut, v_tut;

    public GameObject h_vfx_red, v_vfx_red;

    public GameObject h_endCardPanel, v_endCardPanel;

    //-----------Variables-------------
    [HideInInspector] public bool canInteract = false;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool endGame = false;
    [HideInInspector] public bool isWin = false;
     public bool PauseGame = false;

    private bool rotateScreen = false;

    //-----------Singleton Instance-------------
    private Player_GS1 fighter;
    private EnemyManager_GS1 enemyManager;

    public bool isLose;
    public GameObject canvasLose;
    public GameObject HorLose;
    public GameObject VerLose;
    public GameObject EnemyM;
    public GameObject ObjectPool;

    //private EndCard_V5 endCardManager;

    void Start()
    {
        h_startPanel.SetActive(true);
        v_startPanel.SetActive(true);
        h_warning.SetActive(false);
        v_warning.SetActive(false);
        h_tut.SetActive(false);
        v_tut.SetActive(false);
        h_endCardPanel.SetActive(false);
        v_endCardPanel.SetActive(false);


        fighter = Player_GS1.Ins;
        enemyManager = EnemyManager_GS1.Ins;
        //endCardManager = EndCard_V5.Ins;

        LifeCycle.OnMute += MuteAudio;
        LifeCycle.OnUnmute += UnmuteAudio;

        Analytics.LogEvent(Analytics.EventType.TutorialStarted, 1);
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

            HorLose.SetActive(true);
            VerLose.SetActive(false);
        }
        else
        {
            HorLose.SetActive(false);
            VerLose.SetActive(true);

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

        if (isLose == true)
        {
            DelayLosePanel();
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
    }

    public void StartGame()
    {
        h_tut.SetActive(false);
        v_tut.SetActive(false);
        Analytics.LogEvent(Analytics.EventType.TutorialComplete, 1);
        fighter.StartShooting();
        startGame = true;
    }

    public void ShowWarning(bool canShow)
	{
        h_warning.SetActive(canShow);
        v_warning.SetActive(canShow);
    }

    public void ShowRedVFX(bool canShow)
    {
        h_vfx_red.SetActive(canShow);
        v_vfx_red.SetActive(canShow);
    }

    public void ShowEndCard()
	{
        StartCoroutine(IE_ShowEndCard());
    }
    public IEnumerator IE_ShowEndCard()
	{
        yield return new WaitForSeconds(1f);
        h_startPanel.SetActive(false);
        v_startPanel.SetActive(false);
        h_endCardPanel.SetActive(true);
        v_endCardPanel.SetActive(true);
    }

    void MuteAudio()
    {
        AudioListener.volume = 0;
    }

    void UnmuteAudio()
    {
        AudioListener.volume = 1;
    }

    public void DelayLosePanel()
    {
        StartCoroutine(IE_DelayLosePanel());
    }

    IEnumerator IE_DelayLosePanel()
    {
        yield return new WaitForSeconds(1.3f);
        canvasLose.SetActive(true);
        ObjectPool.SetActive(false);
        EnemyM.SetActive(false);
    }
}
