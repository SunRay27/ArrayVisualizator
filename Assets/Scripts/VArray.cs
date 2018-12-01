using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;


public struct VPoint
{
    public byte intensivity;
    public float x, y, z;

    public VPoint(byte rawIntensivity, float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        intensivity = rawIntensivity;// (byte)(rawIntensivity * rawIntensivity / 100);
    }
}
public struct VArray
{
    //List<VPoint> pts;
   // public List<Transform> realP;
    void Create(Transform prefab, Color32 color, float x, float y, float z, byte br)
    {
        Transform p;
        p = GameObject.Instantiate(prefab, new Vector3(x * 0.05f, y * 0.05f, z * 0.05f), Quaternion.identity) as Transform;
        p.GetComponent<SpriteRenderer>().color = new Color32(color.r, color.g, color.b, br);
    }
    public VArray(string folderPath, Transform prefab, Color32 color)
    {
        folderPath += "\\ArrayData\\";
        // pts = new List<VPoint>();
        // realP = new List<Transform>();
        
        //string dirPath = AppDomain.CurrentDomain.BaseDirectory;
        string[] filePaths = Directory.GetFiles(folderPath);
        //string[] files = new string[filePaths.Length];

        for (int z = 0; z < filePaths.Length; z++)
        {
            using (FileStream fs = File.Open(filePaths[z], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                int y = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    byte brightness = 0;
                    int x = 0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if(Char.IsDigit(line[i]))
                        {
                            if (i + 1 < line.Length)
                            {
                                if (Char.IsDigit(line[i + 1]))
                                {
                                    Create(prefab, color, x, y, z, brightness);
                                    brightness = (byte)((line[i] - '0') * 10 + (line[i + 1] - '0'));
                                }
                                else
                                {
                                    Create(prefab, color, x, y, z, brightness);
                                    brightness = (byte)((line[i] - '0'));
                                }

                            }
                            else
                            {
                                Create(prefab, color, x, y, z, brightness);
                                brightness = (byte)((line[i] - '0'));
                            }
                            x++;
                        }
                    }
                    y++;
                }
            }
        }

        //for (int i = 0; i < filePaths.Length; i++)
          //  files[i] = File.ReadAllText(filePaths[i]);
        
       /* for (int z = 0; z < files.Length; z++)
        {
            string[] planes = files[z].Split('\n');
            for (int y = 0; y < planes.Length; y++)
            {
                string[] rawPts = planes[y].Split('\t');
                for (int x = 0; x < rawPts.Length; x++)
                {
                    Debug.Log(rawPts[x]);
                    if (rawPts[x] != "" && rawPts[x] != " ")
                    {
                        p = GameObject.Instantiate(prefab, new Vector3(x * 0.05f, y * 0.05f, z * 0.05f), Quaternion.identity) as Transform;
                    }

                    //pts.Add(new VPoint(Convert.ToByte(rawPts[x]), x, y, z));
                }
            }
        }*/
    }
    public void Apply(Transform prefab, Color color)
    {
       // foreach (VPoint p in pts)
       // {
            //Transform point = GameObject.Instantiate(prefab, new Vector3(p.x*0.05f, p.y * 0.05f, p.z * 0.05f), Quaternion.identity) as Transform;
            //point.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, p.intensivity);
           // realP.Add(point);
       // }
    }
}
