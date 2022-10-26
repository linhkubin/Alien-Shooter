using UnityEngine;
using Luna.Unity;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public AudioSource clickSound;

    public GameObject hor, ver;
    public GameObject bg_hor, bg_ver;
    private bool isHor;

    //-----------UI-------------
    public GameObject h_startPanel;
    public GameObject v_startPanel;

    public GameObject h_handTut, v_handTut;
    public GameObject[] h_selecting_Border;
    public GameObject[] v_selecting_Border;

    public GameObject h_gameOverPanel;
    public GameObject v_gameOverPanel;
    public Text h_winTxt;
    public Text v_winTxt;

    public GameObject h_endCardPanel;
    public GameObject v_endCardPanel;

    //-----------Variables-------------
    [HideInInspector] public bool startGame = false;
    [HideInInspector] public bool endGame = false;
    [HideInInspector] public bool isWin = false;

    private bool rotateScreen = false;

    //-----------Singleton Instance-------------
    private Fighter fighter;
    private EnemyManager enemyManager;
    private EndCardManager endCardManager;

    void Start()
    {
        h_startPanel.SetActive(true);
        v_startPanel.SetActive(true);
        h_gameOverPanel.SetActive(false);
        v_gameOverPanel.SetActive(false);
        h_endCardPanel.SetActive(false);
        v_endCardPanel.SetActive(false);

        fighter = Fighter.Ins;
        enemyManager = EnemyManager.Ins;
        endCardManager = EndCardManager.Ins;

        LifeCycle.OnMute += MuteAudio;
        LifeCycle.OnUnmute += UnmuteAudio;
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

        if (!isWin && EnemyManager.Ins.GetAliveEnemy() == 0 )
        {
            isWin = true;
            Fighter.Ins.WaitToWin();
        }
    }

    public void StartGame()
	{
        h_startPanel.SetActive(false);
        v_startPanel.SetActive(false);

        fighter.StartShooting();
        enemyManager.EnemyStart();

        startGame = true;
	}

    public void EndGame()
	{
        if (!isWin) enemyManager.WaitToWin();

        StartCoroutine(IE_Endgame());
	}

    public void SelectButton(int idx)
	{
        clickSound.Play();
        h_handTut.SetActive(false);
        v_handTut.SetActive(false);

        for (int i = 0; i < fighter.ships.Length; i++)
		{
            if (i == idx)
            {
                //h_selecting_Border[i].SetActive(true);
                //v_selecting_Border[i].SetActive(true);
                fighter.ships[i].SetActive(true);
                Animator shipAnim = fighter.ships[i].GetComponent<Animator>();
                shipAnim.SetBool("Show", true);
                StartCoroutine(IE_DeactiveAnim(shipAnim, 0.1f));
                fighter.shipIdx = i;
            }
            else 
            {
                //h_selecting_Border[i].SetActive(false);
                //v_selecting_Border[i].SetActive(false);
                fighter.ships[i].SetActive(false); 
            }
        }

        StartGame();
    }

    public void GoToStore()
    {
        LifeCycle.GameEnded();
        Playable.InstallFullGame();
    }

    private void ShowEndCard()
	{
        h_endCardPanel.SetActive(true);
        v_endCardPanel.SetActive(true);
        endCardManager.SetupEndcard();
	}

    public void WaitToShowEndCard(float time)
	{
        Invoke(nameof(ShowEndCard), time);
	}

    IEnumerator IE_Endgame()
    {
        yield return new WaitForSeconds(2f);

  //      if (isWin)
		//{
  //          h_winTxt.text = "VICTORY";
  //          v_winTxt.text = "VICTORY";
		//}
  //      else
		//{
  //          h_winTxt.text = "DEFEAT";
  //          v_winTxt.text = "DEFEAT";
  //      }

        h_gameOverPanel.SetActive(true);
        v_gameOverPanel.SetActive(true);

        yield return new WaitForSeconds(2f);
        h_gameOverPanel.SetActive(false);
        v_gameOverPanel.SetActive(false);
        ShowEndCard();
    }

    private IEnumerator IE_DeactiveAnim(Animator a, float time)
    {
        yield return new WaitForSeconds(time);
        a.SetBool("Show", false);
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
