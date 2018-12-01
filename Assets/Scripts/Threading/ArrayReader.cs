using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static PointCloud;

public class ArrayReader : ThreadedJob
{
    //public Vector3[] InData;  // arbitary job data
   // public Vector3[] OutData; // arbitary job data

    public int[] stats;
    public Point[] points;
    public byte[] brightness;

    public float xMult = 0.1f;
    public float yMult = 0.1f;
    public float zMult = 0.1f;

   
    protected override void ThreadFunction()
    {

    }
}
public class ArrayColorChanger : ThreadedJob
{
    //public Vector3[] InData;  // arbitary job data
    // public Vector3[] OutData; // arbitary job data


    public List<Point> points;
    public byte[] values;

    protected override void ThreadFunction()
    {
        for (int i = 0; i < points.Count; i++)
        {
            //Debug.Log("InProgres...");
            //Point a = new Point();
            //a.color = new Color32(255, 255, 255, values[i]);
           // a.position = points[i].position;
           // points[i] = a;
        }
    }
}