using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AxisButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool heldDown;
    public UnityEvent buttonTickEvent;

    public void OnPointerDown(PointerEventData _eventData)
    {
        heldDown = true;
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
        heldDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (heldDown)
        {
            buttonTickEvent.Invoke();
        }
    }
}
