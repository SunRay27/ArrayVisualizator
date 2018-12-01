using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointCreator : MonoBehaviour, IPointerClickHandler
{
    public Curve curve;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Right)
        {
            // 
            Vector3 mouse = Input.mousePosition + new Vector3(0, 0, 1);
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            Vector3 local = transform.InverseTransformPoint(mouse);
            curve.AddKey(local);
            // 
        }

    }
    public Vector3 GetMouseLocalPos()
    {
        Vector3 mouse = Input.mousePosition + new Vector3(0, 0, 1);
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        Vector3 local = transform.InverseTransformPoint(mouse);
        return local;
    }
}
