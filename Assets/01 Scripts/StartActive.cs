using UnityEngine;

public class StartActive : MonoBehaviour
{
    public bool startActive = true;
    
    void Start()
    {
        gameObject.SetActive(startActive);
        Destroy(this);
    }
}
