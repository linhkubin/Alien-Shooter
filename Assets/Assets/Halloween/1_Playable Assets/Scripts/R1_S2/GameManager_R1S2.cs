using Luna.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_R1S2 : MonoBehaviour
{
    #region Singleton
    public static GameManager_R1S2 Ins;
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

    private bool rotateScreen = false;

    //-----------Singleton Instance-------------
    private Player_R1S2 fighter;

    public GameObject player;
    private EnemyManager_R1S2 enemyManager;

    public GameObject Spine;
    public GameObject BigSpine;

    public float timingSpine;
    public float maxTimingSpine;
    public float maxTimingSpine2;
    public float maxTimingSpine3;

    public float timingLose;
    public float maxTimingLose;

    public bool isLose;

    public bool isClick;

    public GameObject LoseCard;

    public GameObject Barrier;

    public bool canStart = false;

    public GameObject Decor;

    public GameObject LogoGameV;
    public GameObject LogoGameH;

    public GameObject LoseCardV;
    public GameObject LoseCardH;

    public bool TurnOffBullet = false;

    public bool canMoving = false;

    

    //private EndCard_V5 endCardManager;

    void Start()
    {
        canInteract = true;

        h_startPanel.SetActive(true);
        v_startPanel.SetActive(true);
        h_warning.SetActive(false);
        v_warning.SetActive(false);
        h_tut.SetActive(false);
        v_tut.SetActive(false);
        h_endCardPanel.SetActive(false);
        v_endCardPanel.SetActive(false);


        //fighter = Player_R1S2.Ins;
        enemyManager = EnemyManager_R1S2.Ins;
        //endCardManager = EndCard_V5.Ins;

        LifeCycle.OnMute += MuteAudio;
        LifeCycle.OnUnmute += UnmuteAudio;
    }

    void Update()
    {
        if (canInteract && Input.GetMouseButtonDown(0) && !startGame)
        {
            StartGame();

            isClick = true;
        }

        //if(isClick == true)
        //{
        //    timingSpine += Time.deltaTime;

        //    if (timingSpine >= 1.5f)
        //    {
        //        Spine.SetActive(true);
        //    }

        //    if (timingSpine >= maxTimingSpine)
        //    {
        //        Spine.SetActive(false);
        //    }

        //    if (timingSpine >= maxTimingSpine2)
        //    {
        //        Spine.SetActive(true);
        //    }

        //    if (timingSpine >= maxTimingSpine3)
        //    {
        //        Spine.SetActive(false);

        //        BigSpine.SetActive(false);
        //    }
        //}

        if(isLose == true)
        {
            timingLose += Time.deltaTime;

            if (timingLose >= maxTimingLose)
            {
                LoseCard.SetActive(true);
                
            }
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
                //fighter.movement.SetupMouseLimit(Constant.HORIZONTAL_MOUSE_X_LIMIT, Constant.HORIZONTAL_MOUSE_Y_LIMIT);
            }

            //LoseCardV.SetActive(true);
            //LoseCardH.SetActive(false);
        }
        else
        {
            //LoseCardV.SetActive(false);
            //LoseCardH.SetActive(true);

            cam.orthographicSize = originalCameraSize;

            isHor = true;
            if (!rotateScreen)
            {
                rotateScreen = true;
                //fighter.movement.SetupMouseLimit(Constant.VERTICAL_MOUSE_X_LIMIT, Constant.VERTICAL_MOUSE_Y_LIMIT);
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
        h_tut.SetActive(false);
        v_tut.SetActive(false);

        //fighter.StartShooting();

        canMoving = true;

        startGame = true;

        Barrier.SetActive(true);

        

        Decor.SetActive(false);
        
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
        h_startPanel.SetActive(false);
        v_startPanel.SetActive(false);
        h_endCardPanel.SetActive(true);
        v_endCardPanel.SetActive(true);
        LogoGame();
    }
    public void LogoGame()
    {
        StartCoroutine(IE_LogoGame());
    }
    IEnumerator IE_LogoGame()
    {
        yield return new WaitForSeconds(1f);

        LogoGameH.SetActive(true);
        LogoGameV.SetActive(true);
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
