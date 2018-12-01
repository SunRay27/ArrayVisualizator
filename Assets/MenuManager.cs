using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider pointSlider;
    public Dropdown cameraModes;
    public PointCloud pointCloud;
    public Button exitButton, curveButton, generateButton;
    public LineRenderer graphLiner;
    public ColorPicker vertexPicker, bgPicker;
    public Material vertexMaterial;
    public InputField pathField;
    public static string path;
    bool open = true;
    bool locked = false;
    bool generated = false;
    public CanvasGroup cover,cover2;
    public CanvasGroup menu;
    void Start()
    {
        vertexPicker.CurrentColor = vertexMaterial.GetColor("_Color");
        bgPicker.CurrentColor = Camera.main.backgroundColor;

        bgPicker.onValueChanged.AddListener(color =>
        {
            Camera.main.backgroundColor = color;
        });
        vertexPicker.onValueChanged.AddListener(color =>
        {
            vertexMaterial.SetColor("_Color", color);
        });
        cameraModes.onValueChanged.AddListener(delegate { ChangeCameraMode(cameraModes); });

        vertexMaterial.SetFloat("_PointSize", pointSlider.value);
        exitButton.onClick.AddListener(Exit);
        curveButton.onClick.AddListener(ApplyCurve);
       // generateButton.onClick.AddListener(Generate);

        path = System.AppDomain.CurrentDomain.BaseDirectory + "\\ArrayData\\";
        pathField.text = path;

        pathField.onValueChanged.AddListener(ApplyPath);

        pointSlider.value = vertexMaterial.GetFloat("_PointSize");
        pointSlider.onValueChanged.AddListener(delegate { SetPointSize(pointSlider.value); });


    }
    void SetPointSize(float val)
    {
        vertexMaterial.SetFloat("_PointSize", val);
    }
    void ChangeCameraMode(Dropdown modes)
    {
        switch (modes.value)
        {
            case 0:
                Camera.main.orthographic = false;
                Camera.main.nearClipPlane = 0.01f;
                break;
            case 1:
                Camera.main.orthographic = true;
                Camera.main.nearClipPlane = -200;
                break;
            default:
                break;
        }
    }
    void ApplyPath(string newPath)
    {
        path = newPath;
    }
    void Exit()
    {
        Application.Quit();
    }
    void ApplyCurve()
    {
        if (!generated)
            return;
        StartCoroutine(ApplyCurveCoroutine());
        
    }
    public void Generate()
    {
        Debug.Log("Called twice?");
        if (Directory.Exists(path) && !generated)
        {
            if (Directory.GetFiles(path).Length < 1)
                return;
            //Fade everything
            StartCoroutine(GenerateCoroutine());
        }
    }
    IEnumerator GenerateCoroutine()
    {
        locked = true;
        yield return FadeCanvas(true,cover, 1,false);
        Coroutine c = StartCoroutine(pointCloud.GenerateMesh());
        yield return FadeCanvas(false, cover, 1, false);
        generated = true;
        locked = false;
    }
    IEnumerator ApplyCurveCoroutine()
    {
        locked = true;
        yield return FadeCanvas(true, cover2, 1, false);
        yield return FadeCanvas(false, menu, 0.2f, true,false);
        yield return StartCoroutine(pointCloud.ChangeColor());
        yield return FadeCanvas(false, cover2, 1, false);
        yield return FadeCanvas(true, menu, 0.2f, true,false);
        locked = false;
    }
    // Update is called once per frame
    void Update()
    {
            if (locked)
            {
                Debug.Log("LOCKED MOTHERFUCKER????!~?@%! @%!");
            }
            else
            {
                Debug.Log("NOW WE CAN OPEN MENUS, RIGHT, PIDOR?");
                if (Input.GetKeyDown(KeyCode.Escape))
                {

                    if (open)
                        StartCoroutine(FadeCanvas(false, menu, 0.2f, true, true));
                    else
                        StartCoroutine(FadeCanvas(true, menu, 0.2f, true, true));
                }
            }
    }
    IEnumerator FadeCanvas(bool visible, CanvasGroup menu, float dur, bool hideLines, bool controlInput = false)
    {

        if(!visible)
        {
            if (controlInput)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
            menu.blocksRaycasts = false;
            menu.interactable = false;
            if (hideLines)
            {
                graphLiner.enabled = false;
                for (int i = 0; i < graphLiner.transform.childCount; i++)
                {
                    if (graphLiner.transform.GetChild(i).GetComponent<LineRenderer>())
                        graphLiner.transform.GetChild(i).GetComponent<LineRenderer>().enabled = false;
                }
            }

        }
        float t = 0;
        float from = !visible ? 1 : 0;
        float to = 1 - from;
        while(t<1)
        {
            t += Time.deltaTime/ dur;
            menu.alpha = Mathf.SmoothStep(from, to, t);
            yield return null;
        }
        if (visible)
        {
            if (controlInput)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            menu.blocksRaycasts = true;
            menu.interactable = true;
            if (hideLines)
            {
                graphLiner.enabled = true;
                for (int i = 0; i < graphLiner.transform.childCount; i++)
                {
                    if (graphLiner.transform.GetChild(i).GetComponent<LineRenderer>())
                        graphLiner.transform.GetChild(i).GetComponent<LineRenderer>().enabled = true;
                }
            }
        }
        open = !open;
    }
}
