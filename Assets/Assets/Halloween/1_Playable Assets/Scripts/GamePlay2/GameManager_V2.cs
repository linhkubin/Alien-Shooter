using Luna.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_V2 : MonoBehaviour
{
    #region Singleton
    public static GameManager_V2 Ins;
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


    public GameObject h_logo, v_logo;
    public Text h_CountDownTxt;
    public Text v_CountDownTxt;

    public GameObject h_endCardPanel, v_endCardPanel;

    //-----------Variables-------------
    [HideInInspector] public bool canInteract = false;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool endGame = false;
    [HideInInspector] public bool isWin = false;

    private bool rotateScreen = false;

    private float time3s = 4.0f;
    private bool countDown3s = false;

    //-----------Singleton Instance-------------
    private Player_V2 fighter;
    private EnemyManager_V2 enemyManager;
    private EndCard_V2 endCardManager;

    void Start()
    {
        h_startPanel.SetActive(true);
        v_startPanel.SetActive(true);
        h_tut.SetActive(false);
        v_tut.SetActive(false);
        h_tut_txt.SetActive(false);
        v_tut_txt.SetActive(false);
        h_CountDownTxt.gameObject.SetActive(false);
        v_CountDownTxt.gameObject.SetActive(false);
        h_endCardPanel.SetActive(false);
        v_endCardPanel.SetActive(false);

        //Invoke(nameof(ShowTut), 1.25f);

        fighter = Player_V2.Ins;
        enemyManager = EnemyManager_V2.Ins;
        endCardManager = EndCard_V2.Ins;

        LifeCycle.OnMute += MuteAudio;
        LifeCycle.OnUnmute += UnmuteAudio;
    }

    void Update()
    {
        if (canInteract && Input.GetMouseButtonDown(0) && !startGame && !countDown3s)
		{
            StartCountDown();
            countDown3s = true;
		}

        if (countDown3s && !startGame) CountDown3s();
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
        else { 
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

    public void StartGame()
    {
        h_startPanel.SetActive(false);
        v_startPanel.SetActive(false);
    }

    public void StartCountDown()
    {
        h_logo.SetActive(false);
        v_logo.SetActive(false);
        h_tut.SetActive(false);
        v_tut.SetActive(false);

        h_tut_txt.SetActive(false);
        v_tut_txt.SetActive(false);

        h_CountDownTxt.gameObject.SetActive(true);
        v_CountDownTxt.gameObject.SetActive(true);

        //fighter.vfx_ripple.SetActive(false);
        fighter.movement.canMove = true;
        enemyManager.StartMovingTeam();
        fighter.StartShooting();
    }

    public void ShowTut()
	{
        h_tut.SetActive(true);
        v_tut.SetActive(true);

        h_tut_txt.SetActive(true);
        v_tut_txt.SetActive(true);
    }

    public void EndGame()
	{
        endCardManager.WaitToEndCard();
        WaitToEndCard(2f);
    }

    public void WaitToEndCard(float time)
    {
        Invoke(nameof(ShowEndCard), time);
    }

    void ShowEndCard()
	{
        h_endCardPanel.SetActive(true);
        v_endCardPanel.SetActive(true);
    }

    public void GoToStore()
    {
        LifeCycle.GameEnded();
        Playable.InstallFullGame();
    }

    private void CountDown3s()
    {
        time3s -= Time.deltaTime;
        int time = (int)time3s;
        if (time == 0)
        {
            countDown3s = false;
            startGame = true;
            StartGame();
        }
        else
        {
            h_CountDownTxt.gameObject.SetActive(true);
            v_CountDownTxt.gameObject.SetActive(true);
        }
        h_CountDownTxt.text = time.ToString();
        v_CountDownTxt.text = time.ToString();
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
