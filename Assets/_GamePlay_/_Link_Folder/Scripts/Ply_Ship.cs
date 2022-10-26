using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ShipType { Ship_1, Ship_2 }
public enum ShipState { Alive, Reviving, Revived}

public class Ply_Ship : MonoBehaviour
{
    public Transform tf;

    public BulletPoint[] bulletPos;

    
    private GameObject currentShip;

    private float speed = 300f;
    Vector3 mousePoint;

    Vector2 mouseLimit = new Vector2(6, 10);

    private int bulletIndex = 0;

    CounterTime counterTime = new CounterTime();

    bool canControl = false;

    ShipState shipState = ShipState.Alive;

    [SerializeField] GameObject holdEffect;

    //list cac thang tau
    public GameObject[] shipSkin;
    [SerializeField] Animator shipAnim;
    public Transform skin;

    private void Start()
    {
        counterTime.CounterStart(null, Fire, 0.2f);
        shipState = ShipState.Alive;
    }

    public void Update()
    {
        if (canControl && Ply_GameManager.gameState == GameState.GamePlay)
        {
            counterTime.CounterExecute();

            if (Input.GetMouseButton(0))
            {
                mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePoint.x = Mathf.Clamp(mousePoint.x, -mouseLimit.x, mouseLimit.x);
                mousePoint.y = Mathf.Clamp(mousePoint.y, -mouseLimit.y, mouseLimit.y);
                mousePoint.z = 0;

                tf.position = Vector3.Lerp(tf.position, mousePoint, Time.deltaTime * 20f);
            }
        }
    }

    internal void LevelUp()
    {
        if (bulletIndex + 1 < bulletPos.Length )
        {
            bulletIndex++;
            Ply_SoundManager.Ins.PlayFx(FxType.PowerUp);
        }
    }

    public void ChangeShip(ShipType shipType)
    {
        if (currentShip != null)
        {
            currentShip.SetActive(false);
        }

        currentShip = shipSkin[(int)shipType];

        currentShip.SetActive(true);
    }

    private void Fire()
    {
        for (int i = 0; i < bulletPos[bulletIndex].bulletPoints.Length; i++)
        {
            Ply_Pool.Ins.Spawn(PoolType.Bullet, bulletPos[bulletIndex].bulletPoints[i].position, bulletPos[bulletIndex].bulletPoints[i].rotation);
        }

        counterTime.CounterStart(null, Fire, bulletPos[bulletIndex].rateFire);
        Ply_SoundManager.Ins.PlayFx(FxType.Shoot);
    }

    public void Moving(Vector3 targetPoint, float duration, UnityAction callBack, float delay = 0)
    {
        canControl = false;
        tf.DOMove(targetPoint, duration).OnComplete(
            () =>
            {
                callBack?.Invoke();
                canControl = true;
            }).SetDelay(delay);
    }

    public void HoldEffect(bool active)
    {
        holdEffect.SetActive(active);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Ply_GameManager.gameState == GameState.GamePlay)
        {
            //chet lan dau tien -> hoi sinh
            switch (shipState)
            {
                case ShipState.Alive:
                    Ply_Pool.Ins.Spawn(PoolType.VFX_Explore, tf.position, Quaternion.identity).transform.localScale = Vector3.one * 3;
                    shipState = ShipState.Reviving;
                    skin.localPosition = Vector3.up * -10;
                    Ply_UIManager.Ins.OpenUI(UIID.Alert);

                    skin.DOLocalMove(Vector3.zero, .7f).OnComplete(() =>
                    {
                        shipAnim.SetBool("Fade", true);

                        StartCoroutine(IEDelayAction(
                            () =>
                            {
                                shipAnim.SetBool("Fade", false);
                                shipState = ShipState.Revived;
                                Ply_UIManager.Ins.CloseUI(UIID.Alert);
                            }, 2f));
                    });
                    
                    break;

             //dang chet
                case ShipState.Reviving:
                    break;

             //chet len thu 2 la lose luon
                case ShipState.Revived:

                    canControl = false;
                    gameObject.SetActive(false);
                    Ply_UIManager.Ins.OpenUI(UIID.Lose);
                    Ply_Pool.Ins.Spawn(PoolType.VFX_Explore, tf.position, Quaternion.identity).transform.localScale = Vector3.one * 3;

                    break;
            }
        }
    }

    IEnumerator IEDelayAction(UnityAction callBack, float delay)
    {
        yield return new WaitForSeconds(delay);
        callBack?.Invoke();
    }

}

[System.Serializable]
public class BulletPoint
{
    public float rateFire;
    public Transform[] bulletPoints;
}