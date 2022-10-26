using DG.Tweening;
using UnityEngine;

public class EndCardManager : MonoBehaviour
{
    #region Singleton
    public static EndCardManager Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public GameObject h_tutTxt;
    public GameObject v_tutTxt;

    public Transform boss;
    public Transform player;

    void Start()
	{
        h_tutTxt.SetActive(false);
        v_tutTxt.SetActive(false);
    }

    public void SetupEndcard ()
	{
        boss.gameObject.SetActive(true);
        player.gameObject.SetActive(true);

        player.DOMoveY(-7, 1f);
        boss.DOMoveY(6, 3f).OnComplete(()=> {
            h_tutTxt.SetActive(true);
            v_tutTxt.SetActive(true);
        });
	}
}
