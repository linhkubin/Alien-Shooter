using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSlotMachineVer : MonoBehaviour
{
    public static GameManagerSlotMachineVer Ins;

    public GameObject map1;
    public GameObject map2;

    public GameObject canvasFade;

    public GameObject slotMachineHorBG;
    public GameObject slotMachineVerBG;

    public Camera cam;

    public int mapCount;

    public GameManagerSlotMachineVer gmngSlotMachine;
    private void Awake()
    {
        Ins = this;
    }

    public bool canNextMap = false;
    private void Start()
    {
        mapCount = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if(mapCount == 1)
        {
            if (Screen.height > Screen.width)
            {
                cam.orthographicSize = 11;
                slotMachineHorBG.SetActive(false);
                slotMachineVerBG.SetActive(true);
            }
            else if (Screen.height < Screen.width)
            {
                cam.orthographicSize = 8;
                slotMachineHorBG.SetActive(true);
                slotMachineVerBG.SetActive(false);
            }
        }
        else if (mapCount == 2)
        {
            cam.orthographicSize = 11;
        }
   
        nextMap();
    }

    public void nextMap()
    {
        StartCoroutine(IE_nextMap());
    }

    public IEnumerator IE_nextMap()
    {
        yield return new WaitForSeconds(1f);

        if(canNextMap == true)
        {
            map1.SetActive(false);

            map2.SetActive(true);

            mapCount = 2;
        }
    }
}
