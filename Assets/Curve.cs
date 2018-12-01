using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Curve : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject tanPrefab;
    
    List<Dragger> draggers = new List<Dragger>();
    //List<Tangent> tangents = new List<Tangent>();
    [SerializeField]
    AnimationCurve curve;
    //public AnimationCurve helper;
   // public AnimationCurve helper2;
    public LineRenderer lineRenderer;
    public GameObject dragPrefab, tanVisual;

    public RectTransform coordinates;

    [Header("Values")]
    public float minHorizontalValue;
    public float maxHorizontalValue;
    public float minVerticalValue;
    public float maxVerticalValue;
    [HideInInspector]
    public float tanMultiplier, height, width;
    float matchMultiplierX, matchMultiplierY;
    [Header("Visual Settings")]
    public int points = 64;
    public float horizontalGridStep;
    public float verticalGridStep;

    private void Start()
    {
        Initialize();
    }
    public Dragger GetNearestDragger(Vector3 pos, int number)
    {
        float minMagniitude = 1e37f;
        int minIndex = 0;
        for (int i = 0; i < draggers.Count; i++)
        {
            if(draggers[i].number != number)
            if(Mathf.Abs(pos.x - draggers[i].transform.localPosition.x) < minMagniitude)
            {
                minMagniitude = Mathf.Abs(pos.x - draggers[i].transform.localPosition.x);
                minIndex = i;
            }
        }
       // Debug.Log(minMagniitude);
        return draggers[minIndex];
    }
    public void SetDragger(int keyNumber)
    {
         Keyframe[] ke = new Keyframe[curve.keys.Length];

        int draggerIndex = 0;
        for (int i = 0; i < draggers.Count; i++)
        {
            if (draggers[i].number == keyNumber)
            {
                draggerIndex = i;
                break;
            }
        }
        Vector2 curveValues = GetCurveValues(draggers[draggerIndex].transform.localPosition);
        // Debug.Log("before keyframes: " + curve.keys.Length);
        for (int i = 0; i < curve.keys.Length; i++)
         {
            //  int offset = 0;

            //  foreach (Keyframe k in ke)
            //     if (k.time < draggers[i].transform.localPosition.x + 0.01f)
            //        offset = 1;

            //tangemts
            // if (i != n)
            // {
            //     ke[i].inTangent = curve.keys[i].inTangent;
            //     ke[i].outTangent = curve.keys[i].outTangent;
            //  }

            //  ke[i].time = helper.Evaluate(draggers[i].transform.localPosition.x + offset);
            if (i == keyNumber)
            {
                ke[i].value = curveValues.y;
                ke[i].time = curveValues.x;

            }
            else
            {
                ke[i].value = curve.keys[i].value;
                ke[i].time = curve.keys[i].time;
            }

            ke[i].inTangent = curve.keys[i].inTangent;
            ke[i].outTangent = curve.keys[i].outTangent;
            //draggers[i].tangent.SetPosition(0, draggers[i].transform.localPosition + new Vector3(-1, -ke[i].inTangent / 8, 0).normalized * 30);
            //draggers[i].tangent.SetPosition(1, draggers[i].transform.localPosition + new Vector3(1, ke[i].outTangent / 8, 0).normalized * 30);

            //draggers[i].tangent.GetComponent<Tangent>().tan1.transform.localPosition = draggers[i].tangent.GetPosition(0);
            // draggers[i].tangent.GetComponent<Tangent>().tan2.transform.localPosition = draggers[i].tangent.GetPosition(1);

        }
         curve = new AnimationCurve(ke);
       /* int draggerIndex = 0;
        for (int i = 0; i < draggers.Count; i++)
        {
            if (draggers[i].number == keyNumber)
            {
                draggerIndex = i;
                break;
            }
        }

        Vector2 curveValues = GetCurveValues(draggers[draggerIndex].transform.localPosition);
        curve.MoveKey(keyNumber, new Keyframe(curveValues.x, curveValues.y));*/
        DrawCurve();
    }
    public void ApplyTangent(float inTan, int keyNumber)
    {
        Keyframe[] ke = new Keyframe[curve.keys.Length];

        // Debug.Log("before keyframes: " + curve.keys.Length);
        for (int i = 0; i < curve.keys.Length; i++)
        {
            //tangemts
            if (i != keyNumber)
            {
                ke[i].inTangent = curve.keys[i].inTangent;
                ke[i].outTangent = curve.keys[i].outTangent;
            }
            else
            {
                ke[keyNumber].inTangent = ke[keyNumber].outTangent = inTan;
            }

            //  ke[i].time = helper.Evaluate(draggers[i].transform.localPosition.x + offset);
            ke[i].value = curve.keys[i].value;
            ke[i].time = curve.keys[i].time;

        }
        for (int draggerIndex = 0; draggerIndex < draggers.Count; draggerIndex++)
        {
            if (draggers[draggerIndex].number == keyNumber)
            {
                draggers[draggerIndex].tangent.SetPosition(0, draggers[draggerIndex].transform.localPosition + new Vector3(-1, -ke[keyNumber].inTangent / tanMultiplier, 0).normalized * 30);
                draggers[draggerIndex].tangent.SetPosition(1, draggers[draggerIndex].transform.localPosition + new Vector3(1, ke[keyNumber].outTangent / tanMultiplier, 0).normalized * 30);

                draggers[draggerIndex].tangent.GetComponent<Tangent>().tan1.transform.localPosition = draggers[draggerIndex].tangent.GetPosition(0);
                draggers[draggerIndex].tangent.GetComponent<Tangent>().tan2.transform.localPosition = draggers[draggerIndex].tangent.GetPosition(1);
                break;
            }
        }
   
        curve = new AnimationCurve(ke);
        DrawCurve();
    }
    Vector2 GetCurveValues(Vector3 pos)
    {
        matchMultiplierX = (coordinates.rect.width / (maxHorizontalValue - minHorizontalValue));
        matchMultiplierY = (coordinates.rect.height / (maxVerticalValue - minVerticalValue));
        return new Vector2((pos.x) /matchMultiplierX + minHorizontalValue, (pos.y ) /matchMultiplierY + minVerticalValue);
    }
    Vector3 GetKeyPosition(Keyframe key)
    {
        matchMultiplierX = (coordinates.rect.width / (maxHorizontalValue - minHorizontalValue));
        matchMultiplierY = (coordinates.rect.height / (maxVerticalValue - minVerticalValue));
        return new Vector3((key.time - minHorizontalValue) * matchMultiplierX,( Mathf.Clamp(curve.Evaluate(key.time), minVerticalValue, maxVerticalValue) - minVerticalValue) * matchMultiplierY, 0);
    }
    Vector3 GetKeyPosition(float time)
    {
        matchMultiplierX = (coordinates.rect.width / (maxHorizontalValue - minHorizontalValue));
        matchMultiplierY = (coordinates.rect.height / (maxVerticalValue - minVerticalValue));
        return new Vector3( (time-minHorizontalValue) * matchMultiplierX , (Mathf.Clamp(curve.Evaluate(time), minVerticalValue, maxVerticalValue)-minVerticalValue) * matchMultiplierY, 0);
    }
    void DrawHorizontalLine(float posy,float value)
    {
        GameObject p = Instantiate(new GameObject());
        p.transform.parent = lineRenderer.transform;
        p.transform.localPosition = new Vector3(0, posy, 0);
        p.transform.localRotation = Quaternion.identity;

        p.AddComponent<RectTransform>();
        p.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        p.GetComponent<RectTransform>().anchorMax = Vector2.zero;
        p.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        p.transform.localScale = Vector3.one;
        p.AddComponent<Image>();
        p.GetComponent<RectTransform>().sizeDelta = new Vector2(coordinates.rect.width, 2.5f);
        p.GetComponent<Image>().color = new Color(1, 1, 1, 0.05f);
        p.GetComponent<Image>().raycastTarget = false;

        GameObject text = Instantiate(new GameObject());
        text.transform.parent = lineRenderer.transform;
        text.transform.localPosition = new Vector3(-30, posy, 0);
        text.transform.localRotation = Quaternion.identity;
        text.AddComponent<RectTransform>();
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        text.GetComponent<RectTransform>().anchorMax = Vector2.zero;
        text.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        text.transform.localScale = Vector3.one;
        text.AddComponent<Text>();
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 20f);
        text.GetComponent<Text>().resizeTextForBestFit = true;
        text.GetComponent<Text>().resizeTextMinSize = 1;
        text.GetComponent<Text>().text = (value).ToString();
        text.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
    }
    void DrawVerticalLine(float posx, float value)
    {
        GameObject p = Instantiate(new GameObject());
        p.transform.parent = lineRenderer.transform;
        p.transform.localPosition = new Vector3(posx, 0, 0);
        p.transform.localRotation = Quaternion.identity;

        p.AddComponent<RectTransform>();
        p.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        p.GetComponent<RectTransform>().anchorMax = Vector2.zero;
        p.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        p.transform.localScale = Vector3.one;
        p.AddComponent<Image>();
        p.GetComponent<RectTransform>().sizeDelta = new Vector2(2.5f, coordinates.rect.height);
        p.GetComponent<Image>().color = new Color(1, 1, 1, 0.05f);
        p.GetComponent<Image>().raycastTarget = false;

        GameObject text = Instantiate(new GameObject());
        text.transform.parent = lineRenderer.transform;
        text.transform.localPosition = new Vector3(posx, -20, 0);
        text.transform.localRotation = Quaternion.identity;
        text.AddComponent<RectTransform>();
        text.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        text.GetComponent<RectTransform>().anchorMax = Vector2.zero;
        text.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        text.transform.localScale = Vector3.one;
        text.AddComponent<Text>();
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20f);
        text.GetComponent<Text>().resizeTextForBestFit = true;
        text.GetComponent<Text>().resizeTextMinSize = 1;
        text.GetComponent<Text>().text = (value).ToString();
        text.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.GetComponent<Text>().alignment = TextAnchor.LowerLeft;
    }
    void DrawGrid()
    {
         matchMultiplierX = (coordinates.rect.width / (maxHorizontalValue-minHorizontalValue));
         matchMultiplierY = (coordinates.rect.height / (maxVerticalValue-minVerticalValue));

        int vCount;
        int hCount;

        vCount = (int)((maxVerticalValue - minVerticalValue) / verticalGridStep) + 1;
        hCount = (int)((maxHorizontalValue - minHorizontalValue) / horizontalGridStep) + 1;

        //draw horizontal steps (vertical lines)
        for (int i = 0; i < hCount; i++)
        {
            if (Math.Abs(minHorizontalValue + i * horizontalGridStep - maxHorizontalValue) > 1e-10)
            {
               // float newStep = (maxHorizontalValue - minHorizontalValue - ((maxHorizontalValue - minHorizontalValue)% horizontalGridStep)) / horizontalGridStep;
                //Debug.Log(i);
                float position = i * horizontalGridStep * matchMultiplierX;
                DrawVerticalLine(position, (minHorizontalValue + i * horizontalGridStep));
            }

            //create text

        }
        DrawVerticalLine((maxHorizontalValue-minHorizontalValue) * matchMultiplierX,  maxHorizontalValue);
        //draw vertical steps (horizontal lines)
        for (int i = 0; i < vCount; i++)
        {
            //create image
            if (Math.Abs(minVerticalValue + i * verticalGridStep - maxVerticalValue) > 1e-10)
            {
                //float newStep = (maxVerticalValue - minVerticalValue)/ (vCount+3);

                float position = i * verticalGridStep * matchMultiplierY;
                DrawHorizontalLine(position, (minVerticalValue + i * verticalGridStep));
            }

            //create text

        }
        DrawHorizontalLine((maxVerticalValue-minVerticalValue) * matchMultiplierY,  maxVerticalValue);
    }
    void DrawCurve()
    {
        lineRenderer.positionCount = points+1;
        Vector3[] pos = new Vector3[points+1];
        int c = 0;
        double step = (maxHorizontalValue - minHorizontalValue) / points;
        //  Debug.Log($"float i = {minHorizontalValue}; i < {maxHorizontalValue}; i += {(maxHorizontalValue-minHorizontalValue+1.0)/points}");
        for (double i = minHorizontalValue; i < maxHorizontalValue + step; i += step)
        {
             //Debug.Log("what");
             pos[c] = GetKeyPosition((float)i);
            c++;
        }

        lineRenderer.SetPositions(pos);
    }
    void Initialize()
    {
        tanMultiplier =  ((maxVerticalValue-minVerticalValue)*coordinates.rect.width) / ((maxHorizontalValue-minHorizontalValue)*coordinates.rect.height);
        curve = new AnimationCurve();
        curve.AddKey(minHorizontalValue, minVerticalValue);
        curve.AddKey(maxHorizontalValue, maxVerticalValue);

        // Debug.Log("Width? " + coordinates.rect.width);
        // Debug.Log("Height? " + coordinates.rect.height);
        // Debug.Log("Y? " + matchMultiplierY);
        // Debug.Log("X? " + matchMultiplierX);
        // for (int i = 1; i < lineRenderer.transform.childCount; i++)
        //  {
        // Destroy(lineRenderer.transform.GetChild(i).gameObject);
        // }
        DrawGrid();
        DrawCurve();

        int c = 0;
        foreach (Keyframe key in curve.keys)
        {
            CreateUIKey(c, key);
            c++;
        }
    }
    public Vector3 GetMouseLocalPos()
    {
        Vector3 mouse = Input.mousePosition + new Vector3(0, 0, 1);
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        Vector3 local = coordinates.InverseTransformPoint(mouse);
        return local;
    }
    void CreateUIKey(int number, Keyframe key)
    {
        GameObject sprite = Instantiate(dragPrefab, coordinates);
        GameObject lr = Instantiate(tanVisual, coordinates);

        sprite.transform.localPosition = GetKeyPosition(key);
        sprite.GetComponent<Dragger>().tangent = lr.GetComponent<LineRenderer>();
        sprite.GetComponent<Dragger>().number = number;
        sprite.GetComponent<Dragger>().controller = this;
        //lr.transform.localPosition = new Vector3(0, 0, 0);
        lr.GetComponent<LineRenderer>().useWorldSpace = false;
        lr.GetComponent<LineRenderer>().positionCount = 2;
        lr.GetComponent<LineRenderer>().SetPosition(0, sprite.transform.localPosition + new Vector3(-1, -key.inTangent / tanMultiplier, 0).normalized * 30);
        lr.GetComponent<LineRenderer>().SetPosition(1, sprite.transform.localPosition + new Vector3(1, key.outTangent / tanMultiplier, 0).normalized * 30);

        GameObject tan1 = Instantiate(tanPrefab, lineRenderer.transform);
        GameObject tan2 = Instantiate(tanPrefab, lineRenderer.transform);
        
        tan1.transform.localPosition = lr.GetComponent<LineRenderer>().GetPosition(0);
        tan2.transform.localPosition = lr.GetComponent<LineRenderer>().GetPosition(1);

        lr.GetComponent<Tangent>().tan1 = tan1;
        lr.GetComponent<Tangent>().tan2 = tan2;
        lr.GetComponent<Tangent>().controller = this;
        lr.GetComponent<Tangent>().dragger = sprite.GetComponent<Dragger>();
        lr.GetComponent<Tangent>().Init();

        draggers.Add(sprite.GetComponent<Dragger>());
    }
    public void AddKey(Vector3 localPos)
    {
        Vector2 data = GetCurveValues(localPos);
        Keyframe key = new Keyframe(data.x, data.y);
        curve.AddKey(key);
        int number = 0;
        //
        // find number
        draggers = draggers.OrderBy(i => i.number).ToList();
        bool found = false;
        for (int i = 0; i < draggers.Count; i++)
        {
            if(draggers[i].transform.localPosition.x > localPos.x)
            {
                number = draggers[i].number;
                found = true;
                break;
            }
        }
        if (found)
        {
            Debug.Log("we found dragger, which is after new one. New number is " + number);
            for (int i = 0; i < draggers.Count; i++)
            {
                if (draggers[i].number >= number)
                {
                    draggers[i].number++;
                }
            }
        }
        else
        {
            Debug.Log("New keyframe is last, only draggers behind");
            number = curve.keys.Length-1;
        }
        //
        //
        CreateUIKey(number, key);
        DrawCurve();
    }
    public void DeleteKey(int n)
    {
        List<Keyframe> keys = curve.keys.ToList();
        keys.RemoveAt(n);
        curve.keys = keys.ToArray();
        for (int i = 0; i < draggers.Count; i++)
        {
            if(draggers[i].number == n)
            {
                draggers[i].Destroy();
                draggers.RemoveAt(i);
                break;
            }
        }
        for (int i = 0; i < draggers.Count; i++)
        {
            if(draggers[i].number > n)
            {
                draggers[i].number--;
            }
        }
        DrawCurve();
    }
}
