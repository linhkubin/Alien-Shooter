using UnityEngine;
using System.Collections;
using DG.Tweening;

using UnityEngine.UI;

public class BossMinimove : MonoBehaviour {

    public Transform target;
	//public Text txtDebug;
    //public Transform startPoint;
    //public float speed;
    //public float coutMove;
    //float x1, x2;
    GameObject player;
    private Quaternion rote;

	[Tooltip("Enables lerp or not. If it's false and lerpSwitchThreshold is greater than 0, it will switch to lerp whenever target rotation is greater than the threshold.")]
	public bool useLerp = true;
	[Tooltip("Lerp speed")]
	public float rotSpeed = 3.5f;	
	[Tooltip("Non-lerp degrees per second")]
	public float nonLerpDegPerSec = 720;
	[Tooltip("When target rotation is greater than this value, it will switch to lerp")]
	public float lerpSwitchThreshold = 45;
	[Tooltip("Use this to dynamically adjust rotation speed")]
	public float speedMult = 1;
    private float keepStraightSpdMult = 10;

    private void Awake()
    {
        if (target == null) target = transform;
    }

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
		if (keepStraight)
		{
            if (target.rotation.z == 0)
            {
                return;
            }
			rote.z = 0;
            keepStraightSpdMult = 10;
		}
        else
        {
            rote = Quaternion.LookRotation(target.position - player.transform.position, Vector3.back);
            keepStraightSpdMult = 1;
        }

        rote.x = rote.y = 0;

        if (useLerp || (lerpSwitchThreshold > 0 && rote.eulerAngles.z > lerpSwitchThreshold && rote.eulerAngles.z < 360 - lerpSwitchThreshold))
		{
			target.rotation = Quaternion.Lerp(target.rotation, rote, Time.deltaTime * rotSpeed * speedMult * keepStraightSpdMult);
			//if (txtDebug != null) txtDebug.text = "LERP " + Mathf.Abs(rote.eulerAngles.z);
		}
		else
		{
			target.rotation = Quaternion.RotateTowards(target.rotation, rote, Time.deltaTime * nonLerpDegPerSec * speedMult * keepStraightSpdMult);
			//if (txtDebug != null) txtDebug.text = "TOWARD " + Mathf.Abs(rote.eulerAngles.z);
		}
    }
    //void Move()
    //{
    //    transform.DOMoveX(x1, 3).SetEase(Ease.Linear).OnComplete(Move2);
    //}
    //void Move2()
    //{
    //    transform.DOMoveX(x2, 6).SetEase(Ease.Linear).OnComplete(Move3);
    //}
    //void Move3()
    //{
    //    transform.DOMoveX(x1, 6).SetEase(Ease.Linear).OnComplete(Move2);
    //}

    public bool keepStraight;
    public void KeepStraightOn()
    {
        keepStraight = true;
    }
    public void KeepStraightOff()
    {
        keepStraight = false;
    }
}
