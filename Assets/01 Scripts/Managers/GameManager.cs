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
    bool snapVertices = true;
    bool affineTextureMapping = true;

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
    void Start()
    {

        // Enable snap vertices
        snapVertices = true;
        Shader.SetGlobalInteger("_Snap_Vertices", snapVertices ? 1 : 0);

        // Enable affine texture mapping
        affineTextureMapping = true;
        Shader.SetGlobalInteger("_Affine_Texture_Mapping", affineTextureMapping ? 1 : 0);
    }

    /// <summary>
    /// Runs every frame.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleLowResolution();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            snapVertices = !snapVertices;
            ToggleSnapVertices(snapVertices);
        }

        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            affineTextureMapping = !affineTextureMapping;
            ToggleAffineTextureMapping(affineTextureMapping);
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


    public void ToggleSnapVertices(bool _snapVertices)
    {
        Shader.SetGlobalInteger("_Snap_Vertices", _snapVertices ? 1 : 0);

        Debug.Log("Set Snap Vertices: " + _snapVertices);
        TextPrompt.Instance.SetTextPrompt("Vertex Snapping: " + (_snapVertices ? "Enabled" : "Disabled"));
    }

    
    public void ToggleAffineTextureMapping(bool _affineTextureMapping)
    {
        Shader.SetGlobalInteger("_Affine_Texture_Mapping", _affineTextureMapping ? 1 : 0);

        Debug.Log("Set Affine Texture Mapping: " + _affineTextureMapping);
        TextPrompt.Instance.SetTextPrompt("Affine Texture Mapping: " + (_affineTextureMapping ? "Enabled" : "Disabled"));
    }
}