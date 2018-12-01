using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArrayManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text txt;
    public Transform prefab;
    void Start()
    {
        string path = System.AppDomain.CurrentDomain.BaseDirectory;
        txt.text =System.AppDomain.CurrentDomain.BaseDirectory;
        VArray array = new VArray(path,prefab,new Color32(255,255,255,0));
        //array.Apply(prefab, Color.black);
        // VArray array = new VArray("asd");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
