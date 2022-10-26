using Luna.Unity;
using UnityEngine;

public class GameManager_R1_1 : MonoBehaviour
{
    #region Singleton
    public static GameManager_R1_1 Ins;
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
    public GameObject v_tut;

    public GameObject h_vfx_red, v_vfx_red;

    public GameObject h_endCardPanel, v_endCardPanel;

    //-----------Variables-------------
    [HideInInspector] public bool canInteract = false;
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool endGame = false;
    [HideInInspector] public bool isWin = false;

    private bool rotateScreen = false;

    //-----------Singleton Instance-------------
    private Player_R1_1 fighter;
    private EnemyManager_R1_1 enemyManager;
    private EndCard_R1_1 endCardManager;

    void Start()
    {
        h_startPanel.SetActive(true);
        v_startPanel.SetActive(true);
        h_tut.SetActive(false);
        v_tut.SetActive(false);
        h_endCardPanel.SetActive(false);
        v_endCardPanel.SetActive(false);


        fighter = Player_R1_1.Ins;
        enemyManager = EnemyManager_R1_1.Ins;
        endCardManager = EndCard_R1_1.Ins;

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

        //if (enemyManager.isRound2 && !isWin && enemyManager.GetDiedEnemy() == enemyManager.enemies_num)
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
