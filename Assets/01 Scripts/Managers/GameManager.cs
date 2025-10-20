/*
Bachelor of Software Engineering
Media Design School
Auckland
New Zealand
(c) 2025 Media Design School
File Name : GameManager.cs
Description : Singleton containing various game functionality
Author : Kieran Bishop
Mail : kieran.bishop@mds.ac.nz
*/

using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    bool lowResolutionEnabled = true;
    [SerializeField] RenderTexture lowResolutionRenderTexture;
    [SerializeField] GameObject lowResRawImageUIElement;

    /// <summary>
    /// Called once before Start
    /// </summary>
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }
    #endregion

    [Header("Audio")]
    public GameObject audioPlayerPrefab;

    /// <summary>
    /// Execution begins here.
    /// </summary>
    void Start() {}

    /// <summary>
    /// Runs every frame.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleLowResolution();
        }

    }
    
    public void ToggleLowResolution()
    {
        lowResolutionEnabled = !lowResolutionEnabled;

        if (lowResolutionEnabled)
        {

            Camera.main.targetTexture = lowResolutionRenderTexture;
            lowResRawImageUIElement.SetActive(true);
            TextPrompt.Instance.SetTextPrompt("Low Resolution: Enabled");
        }

        else
        {
            Camera.main.targetTexture = null;
            lowResRawImageUIElement.SetActive(false);
            TextPrompt.Instance.SetTextPrompt("Low Resolution: Disabled");
        }
    }

}
