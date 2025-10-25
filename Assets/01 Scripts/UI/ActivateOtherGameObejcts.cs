using System.Collections.Generic;
using UnityEngine;

public class ActivateOtherGameObejcts : MonoBehaviour
{
    public List<GameObject> otherObjects = new List<GameObject>();

    public void ActivateOtherObjects(bool _activate)
    {
        if (_activate)
        {
            SetActive();
        }

        else
        {
            SetInactive();
        }
    }

    void SetActive()
    {
        foreach (GameObject gameObject in otherObjects)
        {
            gameObject.SetActive(true);
        }
    }
    
    void SetInactive()
    {
        foreach(GameObject gameObject in otherObjects)
        {
            gameObject.SetActive(false);
        }
    }
}
