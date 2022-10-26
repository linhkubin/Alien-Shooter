using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    #region Singleton
    public static SlotMachine Ins;
    private void Awake()
    {
        Ins = this;
    }
    #endregion

    public List<Animator> RollItem = new List<Animator>();
    public AudioSource MainAudio;
    public Button SpinButton;
    public GameObject TutPanel;
    public GameObject TutHandle;

    public void Roll_SlotMachine()
    {
        foreach(Animator anim in RollItem)
        {
            anim.Play(Constant.ROLL_SLOTMACHINE_TAG);
        }

        MainAudio.Play();
        SpinButton.interactable = false;
        TutPanel.gameObject.SetActive(false);
        TutHandle.gameObject.SetActive(false);
    }
}
