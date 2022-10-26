using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNextMap : MonoBehaviour
{
    private void OnEnable()
    {
        WaitToNextMap();
    }

    public void WaitToNextMap()
    {
        StartCoroutine(IE_WaitToNextMap());
    }

    public IEnumerator IE_WaitToNextMap()
    {
        yield return new WaitForSeconds(3.8f);

        GameManagerSlotMachineVer.Ins.canvasFade.SetActive(true);

        yield return new WaitForSeconds(1.3f);

        GameManagerSlotMachineVer.Ins.canNextMap = true;

        
    }
}
