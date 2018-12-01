using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dragger : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public LineRenderer tangent;
    public Curve controller;
    public int number;
    Vector3 offset;

        public void OnPointerClick(PointerEventData data)
        {
            if (data.button == PointerEventData.InputButton.Right)
            {
               // 
                controller.DeleteKey(number);
               // 
            }
        
        }
        public void Destroy()
        {
        tangent.GetComponent<Tangent>().Destroy();
        Destroy(gameObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
        if (eventData.button == PointerEventData.InputButton.Right)
            return;
        //return;
        // transform.localPosition = Input.mousePosition + offset;
        //this.transform.localPosition += (Vector3)eventData.delta * 1f;

        Rect rect = new Rect(controller.coordinates.rect);

        Vector3 mousePos = controller.GetMouseLocalPos();
        Vector3 mouseDir = mousePos - transform.localPosition;

        Vector3 nextRawDragPoint = transform.localPosition + mouseDir;


        //dragger
        if (nextRawDragPoint.x > controller.coordinates.rect.width)
        {
            nextRawDragPoint = new Vector3(controller.coordinates.rect.width, nextRawDragPoint.y, 0);
        }
        else if (nextRawDragPoint.x < 1)
        {
            nextRawDragPoint = new Vector3(1, nextRawDragPoint.y, 0);
        }
        if (nextRawDragPoint.y > controller.coordinates.rect.height)
            nextRawDragPoint = new Vector3(nextRawDragPoint.x, controller.coordinates.rect.height, 0);
        else if (nextRawDragPoint.y < 1)
            nextRawDragPoint = new Vector3(nextRawDragPoint.x, 1, 0);

       // Debug.Log("transform.localPosition: " + transform.localPosition);
      //  Debug.Log("nextRawDragPoint: " + nextRawDragPoint);
       // Debug.Log("drag delta: " +( nextRawDragPoint - transform.localPosition));
        Vector3 dragDelta = nextRawDragPoint - transform.localPosition;
        transform.localPosition = nextRawDragPoint;


        
        tangent.GetComponent<Tangent>().tan1.transform.localPosition += mouseDir;
        tangent.GetComponent<Tangent>().tan2.transform.localPosition += mouseDir;
        tangent.SetPosition(0, tangent.GetPosition(0) + mouseDir);
        tangent.SetPosition(1, tangent.GetPosition(1) + mouseDir);

        //Find nearest dragger
        //

        Dragger nearest = controller.GetNearestDragger(transform.localPosition, number);
        //Debug.Log("nearest number: " + nearest.number);
        //Swap numbers
        //
        //if current is ahead

        controller.SetDragger(number);

        if ((nearest.transform.localPosition.x < transform.localPosition.x && nearest.number > number)
    || (nearest.transform.localPosition.x > transform.localPosition.x && nearest.number < number))
        {
            Debug.Log("LEAP!!");
            int t = nearest.number;
            nearest.number = number;
            number = t;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
        //Debug.Log("Released? tan: " + tan);
    }

}