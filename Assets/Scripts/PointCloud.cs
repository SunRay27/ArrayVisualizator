using UnityEngine;

using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Threading;

public class PointCloud : MonoBehaviour
{
   // [Serializable]
    public struct Point
    {
        public Vector3 position;
        public byte color;
    }



   // protected ComputeBuffer compute_buffer;

    public static PointCloud instance;

    // [SerializeField]








   /* 

    */






    public int[] brightnessStats = new int[33];
    byte[] originalBrightness;
    Mesh[] meshes;
    int pointCount;

    public Material material;
    //int numPoints = 10000000;

    // Use this for initialization
    void Awake()
    {
        instance = this;
    }
    string path;
    void Start()
    {
        
        path = System.AppDomain.CurrentDomain.BaseDirectory;
       // GetComponent<MeshFilter>().mesh = mesh;
       // StartCoroutine(CreateMeshes());

     //   UpdateUI();
        // CreateMesh();
        //StartCoroutine(GenerateMesh());
       // Debug.Log(GetPoints().Length);



    }
    Point[] GetPoints()
    {
        string path = MenuManager.path;
        string[] filePaths = Directory.GetFiles(path);
        int counter = 0;
        //Count points count
        for (int z = 0; z < filePaths.Length; z++)
        {
            using (FileStream fs = File.Open(filePaths[z], FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
               // int y = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //int x = 0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (Char.IsDigit(line[i]))
                        {
                            byte alpha = (byte)((line[i] - '0'));
                            if (i + 1 < line.Length)
                            {
                                if (Char.IsDigit(line[i + 1]))
                                {
                                    i++;
                                }
                            }
                            counter++;
                        }
                    }
                }
                
            }
            //yield return null;
        }
        Point[] points = new Point[counter];
        originalBrightness = new byte[counter];
        counter = 0;
        for (int z = 0; z < filePaths.Length; z++)
        {
            using (FileStream fs = File.Open(filePaths[z], FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                 int y = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    int x = 0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (Char.IsDigit(line[i]))
                        {
                            byte alpha = (byte)((line[i] - '0'));
                            if (i + 1 < line.Length)
                            {
                                if (Char.IsDigit(line[i + 1]))
                                {
                                     alpha = (byte)((line[i] - '0') * 10 + (line[i + 1] - '0'));
                                    i++;
                                }
                            }

                             Point p = new Point();
                             p.color = alpha;
                             p.position = new Vector3(x * 0.1f, y * 0.1f, z * 0.1f);

                             points[counter] = p;
                             originalBrightness[counter] = alpha;
                             brightnessStats[alpha]++;
                             counter++;
                             x++;
                        }
                    }
                     y++;
                }
            }
            //yield return null;
        }
        return points;
    }
    T[] Take<T>(ref T[] array, int count, int offset)
    {
        count = offset + count > array.Length ? array.Length - offset : count;
        T[] newArray = new T[count];
       // Debug.Log($"Cloud size: {array.Length} and offset+count= {offset + count} and newArraySize={newArray.Length} and count={count} and offset={offset}");

        for (int i = offset; i < offset + count; i++)
        {
            if (i < array.Length)
                newArray[i - offset] = array[i];
            else
                return newArray;
        }
        return newArray;
    }
    public IEnumerator GenerateMesh()
    {
        Point[] points = GetPoints();
        int number = points.Length;
        int vertLimit = 65535;
        int numberOfMeshes = number / vertLimit + 1;
        Debug.Log("Number of meshes: " + numberOfMeshes);
        int lastIndex = 0;
        meshes = new Mesh[numberOfMeshes];
        for (int i = 0; i < numberOfMeshes; i++)
        {
            Debug.Log($"processing {i + 1} mesh : ");
            Mesh m = new Mesh();
            Point[] pts = Take(ref points, vertLimit, lastIndex);
            Vector3[] verts = new Vector3[pts.Length];
            Color[] colors = new Color[pts.Length];
            int[] indecies = new int[pts.Length];
            for (int j = 0; j < pts.Length; j++)
            {
                verts[j] = pts[j].position;
                colors[j] = new Color32(255,255,255, pts[j].color);
                indecies[j] = j;
            }
            m.vertices = verts;
            m.colors = colors;
            m.SetIndices(indecies, MeshTopology.Points, 0);
            meshes[i] = m;

            lastIndex += vertLimit;
           // yield return null;
        }
        int count1 = 0;
        for (int i = 0; i < meshes.Length; i++)
        {
            count1 += meshes[i].vertices.Length;
            GameObject mesh = new GameObject();
            mesh.isStatic = true;
            MeshRenderer renderer = mesh.AddComponent<MeshRenderer>();
            MeshFilter filter = mesh.AddComponent<MeshFilter>();
            filter.mesh = meshes[i];
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            //yield return null;
        }

        int max = brightnessStats.Max();
        float mult = 255f / max;
        for (int i = 0; i < 33; i++)
        {
            GameObject p = Instantiate(new GameObject());
         //   p.transform.parent = lineRenderer.transform;
            p.transform.localPosition = new Vector3(i * (256 / 32f) + 14, +14, 0);
            p.transform.localRotation = Quaternion.identity;

            p.AddComponent<RectTransform>();
            p.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            p.GetComponent<RectTransform>().anchorMax = Vector2.zero;
            p.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            p.transform.localScale = Vector3.one;
            p.AddComponent<Image>();
            p.GetComponent<RectTransform>().sizeDelta = new Vector2(256 / 32, brightnessStats[i] * mult);
            p.GetComponent<Image>().color = new Color(0, 0, 1, 0.5f);
            p.GetComponent<Image>().raycastTarget = false;
        }
        Debug.Log("Number of points:" + count1);
        yield return null;
    }
   // ArrayColorChanger job2 = new ArrayColorChanger();
    public void CreateMeshes()
    {
        // This pattern lets us interrupt the work at a safe point if neeeded.
        // Do Work...
       /* points = new List<Point>();
        string path = MenuManager.path;
        string[] filePaths = Directory.GetFiles(path);

        //int pointCount = GetPointsCount();

        //points.Capacity = pointCount;
       // originalBrightness = new byte[pointCount];

        for (int z = 0; z < filePaths.Length; z++)
        {
            // Debug.Log(z);
            using (FileStream fs = File.Open(filePaths[z], FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                int counter = 0;
                string line;
                int y = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    // Debug.Log(z);
                    
                    int x = 0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (Char.IsDigit(line[i]))
                        {
                            byte alpha = (byte)((line[i] - '0'));
                            if (i + 1 < line.Length)
                            {
                                if (Char.IsDigit(line[i + 1]))
                                {
                                    alpha = (byte)((line[i] - '0') * 10 + (line[i + 1] - '0'));
                                    i++;
                                }
                            }

                            
                            Point p = new Point();
                            p.color = new Color32(255, 255, 255, alpha);
                            p.position = new Vector3(x * 0.1f, y * 0.1f, z * 0.1f);

                            points.Add(p);
                            originalBrightness.Add(alpha);
                            //originalBrightness[counter] = alpha;
                            brightnessStats[alpha]++;
                            x++;
                            counter++;
                        }
                    }
                    y++;
                }

            }
        }


        compute_buffer = new ComputeBuffer(points.Count/2+1, sizeof(float) * 7, ComputeBufferType.Default);
        Debug.Log("Points count: "+points.Count);
        compute_buffer.SetData(points.Take(points.Count/2+1).ToArray());


        //Debug.Log("FINISHED?");

        //UI part
        int max = brightnessStats.Max();
        float mult = 255f/max;
        for (int i = 0; i < 33; i++)
        {
            GameObject p = Instantiate(new GameObject());
            p.transform.parent = lineRenderer.transform;
            p.transform.localPosition = new Vector3(i*(256/32f) + 14,+14,0);
            p.transform.localRotation = Quaternion.identity;
            
            p.AddComponent<RectTransform>();
            p.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            p.GetComponent<RectTransform>().anchorMax = Vector2.zero;
            p.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            p.transform.localScale = Vector3.one;
            p.AddComponent<Image>();
            p.GetComponent<RectTransform>().sizeDelta = new Vector2(256 / 32, brightnessStats[i] * mult);
            p.GetComponent<Image>().color = new Color(0, 0, 1, 0.5f);
            p.GetComponent<Image>().raycastTarget = false;
           // yield return null;
        }*/
    }
    void OnDestroy()
    {
        //if(compute_buffer != null)
       // compute_buffer.Release();
    }
    public IEnumerator ChangeColor()
    {
          Debug.Log("CALLED???");
         // job2.points = points;
         // job2.br = originalBrightness;
          byte[] bri = new byte[originalBrightness.Length];
          int c = 0;
          for (int i = 0; i < originalBrightness.Length; i++)
          {
            //  bri[i] = (byte)Mathf.Clamp(curve.Evaluate(originalBrightness[i]),0,255);
              if (c > 500000)
              {
                  c = 0;
                  yield return null;
              }
              c++;
          }
        c = 0;
        int lastIndex = 0;
        for (int i = 0; i < meshes.Length; i++)
        {
            Color[] colors = new Color[meshes[i].vertexCount];
            for (int j = 0; j < meshes[i].vertexCount; j++)
            {
                colors[j] = new Color32(255,255,255, bri[j + lastIndex]);
            }
            lastIndex += meshes[i].vertexCount;
            meshes[i].colors = colors;

            if (c > 5)
            {
                c = 0;
                yield return null;
            }
            c++;
        }
         // job2.values = bri;
         // job2.Start();

          //yield return StartCoroutine(job2.WaitFor());
          //compute_buffer.SetData(points);
          Debug.Log("FINISHED?");
        //yield return null;
    }


    void OnPostRender()
    {
      //  material.SetPass(0);
      //  material.SetBuffer("cloud", compute_buffer);
        //material.SetBuffer("cloud2", compute_buffer2);
       // material.SetBuffer("cloud3", compute_buffer3);
     //  if(points != null)
      //  Graphics.DrawProcedural(MeshTopology.Points, points.Count, 1);
    }
}