using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class pointCloud : MonoBehaviour
{
    public Material material;
    protected int number = 17000000;
    //protected ComputeBuffer compute_buffer;
    struct Point
    {
        public Vector3 position;
        public Color color;
    }
    Mesh[] meshes;
    void Start()
    {
        //compute_buffer = new ComputeBuffer(half, sizeof(float) * 7, ComputeBufferType.Raw);
        Point[] cloud = new Point[number];
        
        for (uint i = 0; i < number; ++i)
        {
            cloud[i] = new Point();
            cloud[i].position = new Vector3();
            cloud[i].position.x = Random.Range(-300f, 300f);
            cloud[i].position.y = Random.Range(-300f, 300f);
            cloud[i].position.z = Random.Range(-300f, 300f);
            cloud[i].color = new Color(1, 1, 1, Random.Range(0f, 1f));
        }
        CreateMeshes(cloud);
        //compute_buffer.SetData(cloud);

    }
    T[] Take<T> (ref T[] array, int count, int offset)
    {
        count = offset + count > array.Length ? array.Length - offset : count;
        T[] newArray = new T[count];
        Debug.Log($"Cloud size: {array.Length} and offset+count= {offset + count} and newArraySize={newArray.Length} and count={count} and offset={offset}");

        for (int i = offset; i < offset + count; i++)
        {
            if (i < array.Length)
                newArray[i-offset] = array[i];
            else
                return newArray;
        }
        return newArray;
    }
    void CreateMeshes(Point[] cloud)
    {
        int vertLimit = 65535;
        int numberOfMeshes = number / vertLimit + 1;
        Debug.Log("Number of meshes: " + numberOfMeshes);
        int lastIndex = 0;
        meshes = new Mesh[numberOfMeshes];
        for (int i = 0; i < numberOfMeshes; i++)
        {
            Debug.Log($"processing {i+1} mesh : ");
            Mesh m = new Mesh();
            Point[] points = Take(ref cloud, vertLimit, lastIndex);
            Vector3[] verts = new Vector3[points.Length];
            Color[] colors = new Color[points.Length];
            int[] indecies = new int[points.Length];
            for (int j = 0; j < points.Length; j++)
            {
                verts[j] = points[j].position;
                colors[j] = points[j].color;
                indecies[j] = j;
            }
            m.vertices = verts;
            m.colors = colors;
            m.SetIndices(indecies,MeshTopology.Points,0);
            meshes[i] = m;

            lastIndex += vertLimit;
        }
        int count1 = 0;
        for (int i = 0; i < meshes.Length; i++)
        {
            count1 += meshes[i].vertices.Length;
            GameObject mesh = new GameObject();
            MeshRenderer renderer = mesh.AddComponent<MeshRenderer>();
            MeshFilter filter = mesh.AddComponent<MeshFilter>();
            filter.mesh = meshes[i];
            renderer.material = material;
        }
        Debug.Log("Number of points:" + count1);
    }
    void OnPostRender()
    {
        //material.SetPass(0);
       // material.SetBuffer("cloud", compute_buffer);
       // Graphics.DrawProcedural(MeshTopology.Points, half, 1);
    }

    void OnDestroy()
    {
      //  compute_buffer.Release();
    }
}