using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAura : MonoBehaviour
{
    //public ParticleSystem[] m_Particles;

    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * 300f);
    }

    //public void UpdateColor(Color c)
    //{
    //    for (int i = 0; i < m_Particles.Length; i++)
    //    {
    //        var col = m_Particles[i].colorOverLifetime;
    //        col.enabled = true;
    //        Gradient grad = new Gradient();
    //        //grad.alphaKeys = col.color.gradient.alphaKeys;
    //        //grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(c, 0.05f) }, col.color.gradient.alphaKeys);
    //        col.color = grad;
    //    }
    //}
}
 