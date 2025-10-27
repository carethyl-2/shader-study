using UnityEngine;

public class LightColorChange : MonoBehaviour
{
    Light lightObject;

    void Start()
    {
        lightObject = GetComponent<Light>();
    }

    public void RandomizeLightColor()
    {
        Color newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        lightObject.color = newColor;
    }
}
