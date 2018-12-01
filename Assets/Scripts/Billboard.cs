using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : UpdateSystemBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
    }
    public Camera m_Camera;
    // Update is called once per frame
    public override void OnUpdate(float dt)
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
    m_Camera.transform.rotation * Vector3.up);
    }
}
