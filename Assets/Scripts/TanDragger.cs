using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TanDragger : MonoBehaviour, IDragHandler, IEndDragHandler
{
    
    public Tangent tangent;
    public Dragger dragger;
    public PointCreator pointCreator;
    float tanIn, tanOut;

    float angleIn, angleOut;

    Vector3 outVec, inVec, projectedOut, projectedIn;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Update()
    {
        //Debug.DrawRay(dragger.transform.position, Vector3.Project(vec2,projectedIn).normalized * 1/Mathf.Sin(angleIn));

      //  Debug.DrawRay(dragger.transform.position, Vector3.Project(vec1, projectedOut).normalized * 1 / Mathf.Sin(angleOut));

    }
    // Update is called once per frame
    public void OnDrag(PointerEventData eventData)
    {



        Vector3 mousePos = GetComponentInParent<Curve>().GetMouseLocalPos();
        Vector3 mouseDir = mousePos - dragger.transform.localPosition;
        Vector3 oldRot = tangent.tan1.transform.localPosition - dragger.transform.localPosition;
        float rotAngle1 = Vector3.SignedAngle(oldRot, mouseDir, dragger.transform.forward);

        tangent.tan1.transform.RotateAround(dragger.transform.position, transform.forward, rotAngle1);
        tangent.tan2.transform.RotateAround(dragger.transform.position, transform.forward, rotAngle1);
        dragger.tangent.SetPosition(0, tangent.tan1.transform.localPosition);
        dragger.tangent.SetPosition(1, tangent.tan2.transform.localPosition);

        //  Debug.Log(eventData.delta);
        //Debug.DrawRay(dragger.transform.position, mouseDir);
        //Debug.DrawRay(dragger.transform.position, oldRot);

        outVec = dragger.transform.right/tangent.controller.maxHorizontalValue;
        Vector3 tanVec = (tangent.tan2.transform.position - dragger.transform.position );
        
        //tanVec = new Vector3(tanVec.x, tanVec.y, tanVec.z);

        float angle = Vector3.SignedAngle(outVec, tanVec, dragger.transform.forward);
        //float angle = Vector3.Angle(outVec, tanVec);

        tanOut = Mathf.Tan(angle * Mathf.Deg2Rad);
       // Debug.DrawRay(dragger.transform.position, tanVec, Color.red);
       // Debug.DrawRay(dragger.transform.position, outVec, Color.red);
        //Debug.Log("alpha: " + angle + " tangent: " + tanOut);
        //projectedOut = new Vector3(1 / 32, tangent.tan2.transform.localPosition.y, 0);
        //angleOut = 
        //projectedOut = (tangent.tan2.transform.localPosition - dragger.transform.localPosition).normalized * 100;
        //angleOut = Mathf.Abs(Mathf.PI / 2 - Vector3.Angle(vec1, projectedOut) * Mathf.Deg2Rad);

       
        //tanOut = (Vector3.Project(vec1, projectedOut).normalized * 1 / Mathf.Sin(angleIn)).y;

       // vec2 = -dragger.transform.right;
        projectedIn = (tangent.tan1.transform.localPosition - dragger.transform.localPosition).normalized * 100;

        //angleIn = Mathf.Abs(Mathf.PI / 2 - Vector3.Angle(vec2, projectedIn) * Mathf.Deg2Rad);
        tanOut *= tangent.controller.tanMultiplier;
       // tanIn = (Vector3.Project(vec2, projectedIn).normalized * 1 / Mathf.Sin(angleIn)).y;
        tanIn = tanOut;



        //
        // Debug.Log("What?");
        // tan = Mathf.Tan(Vector3.Angle(tangent.tan1.transform.localPosition - dragger.transform.localPosition, dragger.transform.right) * Mathf.Deg2Rad);
        //tan2 = Mathf.Tan(Vector3.Angle(tangent.tan2.transform.localPosition - dragger.transform.localPosition, dragger.transform.right) * Mathf.Deg2Rad);
        // Debug.Log("In tangent: " + tanIn);
        // Debug.Log("out tangent: " + tanOut);
        tangent.controller.ApplyTangent(tanIn, dragger.number);


    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
        //Debug.Log("Released? tan: " + tan);
    }
}
