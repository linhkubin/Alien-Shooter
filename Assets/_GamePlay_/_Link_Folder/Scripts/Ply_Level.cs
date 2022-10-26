using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply_Level : Ply_Singleton<Ply_Level>
{
    public Ply_Ship ship;
    public Ply_Stage[] stages;
    int indexStage = 0;

    public GameObject bgHorizontal, bgVertical;

    private void Start()
    {
        GameObject bg = Screen.height > Screen.width ? bgVertical : bgHorizontal;
        Instantiate(bg);

        Ply_UIManager.Ins.OpenUI(UIID.ChooseShip);
    }

    public void Play()
    {
        stages[indexStage].OnInit();

    }

    public void NextStage()
    {
        stages[indexStage].gameObject.SetActive(false);
        indexStage++;

        if (indexStage < stages.Length)
        {
            stages[indexStage].OnInit();
        }
    }
}
