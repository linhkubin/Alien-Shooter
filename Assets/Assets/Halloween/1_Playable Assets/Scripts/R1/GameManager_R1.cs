using Luna.Unity;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager_R1 : MonoBehaviour
{
    #region Singleton
    public static GameManager_R1 Ins;
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

    public GameObject h_vfx_red, v_vfx_red;

    public GameObject h_warning, v_warning;

    public GameObject h_endCardPanel, v_endCardPanel;

    //-----------Variables-------------
    [HideInInspector] public bool canInteract = false;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool endGame = false;
    [HideInInspector] public bool isWin = false;
    [HideInInspector] public bool islose = false;

    private bool rotateScreen = false;

    //-----------Singleton Instance-------------
    private Player_R1 fighter;
    private EnemyManager_R1_AutoFire enemyManager;
    private EndCard_R1 endCardManager;
    public GameObject loseEndcard;
    public GameObject Enemy;
    public GameObject objectPool;

    public GameObject HorEndcardTut;
    public GameObject VerEndcardTut;

    public Player_R1 Player;

    public AudioSource MusicBg;

    public bool isTurnOnMusicBg;

    public GameObject winCard;
    public GameObject winCardH;
    public GameObject winCardV;

    public Text CoinCount;
    public Text CheckText;
    public Text CheckText1;
    public Text CheckText2;
    public int ii1 = 0;
    public int ii2 = 0;
    public int ii3 = 0;
    public GameObject CoinCountText;
    public int Coin;

    
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

        fighter = Player_R1.Ins;
        enemyManager = EnemyManager_R1_AutoFire.Ins;
        endCardManager = EndCard_R1.Ins;

        //LifeCycle.OnMute += MuteAudio;
        //LifeCycle.OnUnmute += UnmuteAudio;

        //Analytics.LogEvent(Analytics.EventType.TutorialStarted,0);
        //Analytics.LogEvent(Analytics.EventType.LevelStart,1);
    }

    // Update is called once per frame

    public int clickCount = 0;
    void Update()
    {
        if (canInteract && Input.GetMouseButtonDown(0) && !startGame)
        {
            StartGame();
        }

        CoinCount.text = Coin.ToString();

        if( Coin != 0 && Coin % 1000 == 0)
        {
            EnemyManager_R1_AutoFire.Ins.coinCollect.Play();
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
            HorEndcardTut.SetActive(true);
            VerEndcardTut.SetActive(false);
            winCardH.SetActive(true);
            winCardV.SetActive(false);

        }
        else
        {
            isHor = true;
            if (!rotateScreen)
            {
                rotateScreen = true;
                fighter.movement.SetupMouseLimit(Constant.VERTICAL_MOUSE_X_LIMIT, Constant.VERTICAL_MOUSE_Y_LIMIT);
              
               
            }
            HorEndcardTut.SetActive(false);
            VerEndcardTut.SetActive(true);
            winCardH.SetActive(false);
            winCardV.SetActive(true);

        }

        hor.SetActive(!isHor);
        ver.SetActive(isHor);
        bg_hor.SetActive(!isHor);
        bg_ver.SetActive(isHor);

        if (!isWin && EnemyManager_R1_AutoFire.Ins.GetDiedEnemy() == EnemyManager_R1_AutoFire.Ins.enemies_num)
        {
            isWin = true;
            
            fighter.TurnOffBullet();

            Invoke(nameof(DelayWinTurnOffMusic), 1f);

            CoinCountText.gameObject.SetActive(false);
        }

        //lose action
        if (islose == true)
        {
            Invoke(nameof(DelayEndcardLose), 1f);
            CoinCountText.gameObject.SetActive(false);

        }
    }


    public void ShowWarning(bool canShow)
    {
        h_warning.SetActive(canShow);
        v_warning.SetActive(canShow);
    }
    public void GoToStore()
    {
        //LifeCycle.GameEnded();
        Playable.InstallFullGame();
    }

    public void GoStore()
    {
        StartCoroutine(IE_GoStore());
        ii1++;
        CheckText.text = ii1.ToString();
    }

    IEnumerator IE_GoStore()
    {
        for(int i = 0; i < 100; i++)
        {
            LifeCycle.GameEnded();

        }

        ii2++;
        CheckText1.text = ii2.ToString();
        yield return new WaitForSeconds(0.15f);

        ii3++;
        CheckText2.text = ii3.ToString();
        for (int i = 0; i < 100; i++)
        {
            Playable.InstallFullGame();

        }
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
        if (isTurnOnMusicBg == false)
        {
            isTurnOnMusicBg = true;

            MusicBg.enabled = true;
        }

        h_startPanel.SetActive(false);
        v_startPanel.SetActive(false);

        fighter.StartShooting();
        enemyManager.canSetupEnemy = true;

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

    public void DelayEndcardLose()
    {
        StartCoroutine(IE_DelayEndCardLose(1f));
    }
    IEnumerator IE_DelayEndCardLose(float time)
    {
        yield return new WaitForSeconds(time);
        objectPool.SetActive(false);

        loseEndcard.SetActive(true);

        Enemy.SetActive(false);

        MusicBg.enabled = false;
    }
    public void DelayWinTurnOffMusic()
    {
        StartCoroutine(IE_DelayWinTurnOffMusic(1f));
    }
    IEnumerator IE_DelayWinTurnOffMusic(float time)
    {
        yield return new WaitForSeconds(time);
      
        MusicBg.enabled = false;
    }
}
