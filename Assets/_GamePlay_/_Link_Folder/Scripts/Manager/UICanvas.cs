using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public bool IsDestroyOnClose = true;

    private void Start()
    {
        Init();
    }

    protected void Init()
    {
        //m_RectTransform = GetComponent<RectTransform>();
        //m_Animator = GetComponent<Animator>();

        //float ratio = (float)Screen.height / (float)Screen.width;

        //// xu ly tai tho
        //if (ratio > 2.1f)
        //{
        //    Vector2 leftBottom = m_RectTransform.offsetMin;
        //    Vector2 rightTop = m_RectTransform.offsetMax;
        //    rightTop.y = -100f;
        //    m_RectTransform.offsetMax = rightTop;
        //    leftBottom.y = 0f;
        //    m_RectTransform.offsetMin = leftBottom;
        //    m_OffsetY = 100f;
        //}
        //m_IsInit = true;
    }

    public virtual void Setup()
    {
    }

    public virtual void BackKey()
    {

    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        //anim
    }

    public virtual void Close()
    {
        //anim
        gameObject.SetActive(false);
        if (IsDestroyOnClose)
        {
            Destroy(gameObject);
        }
        
    }


}
