using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIID
{
    Menu = 0,
    Win = 1,
    Lose = 2,
    ChooseShip = 3,
    Alert = 4,
}


public class Ply_UIManager : Ply_Singleton<Ply_UIManager>
{
    [Header("---Thu tu UI phai dung voi UI ID---")]
    public GameObject[] horizonPrefabs;
    public GameObject[] vertiPrefabs;

    private GameObject[] uiActive = new GameObject[10];

    public Transform CanvasParentTF;

    bool isVertical = false;

    private void Start()
    {
        Debug.Log(Screen.height +"_"+ Screen.width);
        isVertical = Screen.height > Screen.width; 
    }

    public bool IsOpenedUI(UIID ID)
    {
        return IsLoaded(ID) && uiActive[(int)ID].activeInHierarchy;
    }

    public GameObject GetUI(UIID ID)
    {
        if (!IsLoaded(ID))
        {
            uiActive[(int)ID] = Instantiate(isVertical ? vertiPrefabs[(int)ID] : horizonPrefabs[(int)ID], CanvasParentTF);
        }

        return uiActive[(int)ID];
    }

    public GameObject OpenUI(UIID ID)
    {
        GameObject canvas = GetUI(ID);
        canvas.SetActive(true);
        return canvas;
    }

    public bool IsLoaded(UIID ID)
    {
        return uiActive[(int)ID] != null;
    }

    public void CloseUI(UIID ID)
    {
        if (IsLoaded(ID))
        {
            GetUI(ID).SetActive(false);
        }
    }

}
