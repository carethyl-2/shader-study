using UnityEngine;
using TMPro;

public class TextPrompt : MonoBehaviour
{
    #region Singleton
    public static TextPrompt Instance;

    /// <summary>
    /// Called once before Start
    /// </summary>
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }
    #endregion

    TMP_Text tmpText;

    float timer = 0.0f;


    void Start()
    {
        tmpText = GetComponent<TMP_Text>();    
    }

    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;

            Color textColor = tmpText.color;
            textColor.a = timer;
            tmpText.color = textColor;            
        }
    }

    public void SetTextPrompt(string _text)
    {
        timer = 1.0f;
        tmpText.text = _text;
    }
}
