using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestBullet : MonoBehaviour
{
    public int id;
    public Vector3[] path;
    public Transform initPos;
    public Transform targetPos;
 
    public void MoveToPosL()
    {
        //path = new Vector3[7];
        //path[0] = initPos.position;
        //path[6] = targetPos.position;
        //for (int i = 0; i < GMNGTest.Ins.pathL.Length; i++)
        //{
        //    path[i + 1] = GMNGTest.Ins.pathL[i].position;
        //}

        //transform.DOPath(path, 1f, PathType.CatmullRom).SetEase(Ease.Linear);
    }
    
    public void MoveToPosR()
    {
        //path = new Vector3[7];
        //path[0] = initPos.position;
        //path[6] = targetPos.position;
        //for (int i = 0; i < GMNGTest.Ins.pathR.Length; i++)
        //{
        //    path[i + 1] = GMNGTest.Ins.pathR[i].position;
        //}

        //transform.DOPath(path, 1f, PathType.CatmullRom).SetEase(Ease.Linear);
    }
}
