using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Tangent : MonoBehaviour
{
    public Dragger dragger;
    public Curve controller;
    public GameObject tan1, tan2;
    public void Init()
    {
        tan1.GetComponent<TanDragger>().tangent = this;
        tan2.GetComponent<TanDragger>().tangent = this;
        tan2.GetComponent<TanDragger>().dragger = dragger;
        tan1.GetComponent<TanDragger>().dragger = dragger;
    }
    public void Destroy()
    {
        Destroy(tan1);
        Destroy(tan2);
        Destroy(this.gameObject);
    }
}
