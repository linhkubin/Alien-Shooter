using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luna.Unity;
using DG.Tweening;

public class Gamemanager1 : MonoBehaviour
{
    public GameObject BaseCanvas;

    /*----------------------------GameObject----------------------------------------------*/
    public GameObject Ship;

    public GameObject bossBody;

    public GameObject BossSpiderIMG;

    public GameObject BossScorpionIMG;

    public Animator spiderAnim;

    public Animator scorpionAnim;

    public AudioSource StartSound;

    private GameManagerR1BLH gameManager;

    public GameObject canvasGamePlay;

    public void GoToStore()
    {
        LifeCycle.GameEnded();

        Playable.InstallFullGame();

        Debug.Log("Store");
    }

    public void selectSpider()
    {
        BossR1BulletHell.Ins.anim = spiderAnim;

        BaseCanvas.SetActive(false);

        BossSpiderIMG.SetActive(true);

        //bossBody.transform.DOMoveY(5f, 1f).SetEase(Ease.Linear);

        GameManagerR1BLH.Ins.canMove = true;

        StartSound.enabled = true;

        canvasGamePlay.SetActive(true);
    }
    
    public void selectScorpion()
    {
        BossR1BulletHell.Ins.anim = scorpionAnim;

        BaseCanvas.SetActive(false);

        BossScorpionIMG.SetActive(true);

        //bossBody.transform.DOMoveY(5f, 1f).SetEase(Ease.Linear);

        GameManagerR1BLH.Ins.canMove = true;

        StartSound.enabled = true;
        canvasGamePlay.SetActive(true);
    }
}