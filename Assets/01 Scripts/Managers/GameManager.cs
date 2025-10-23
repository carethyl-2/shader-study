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

using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;
    

    bool lowResolutionEnabled = true;
    bool snapVertices = true;
    bool affineTextureMapping = true;
    bool dither = true;
    bool colorQuantization = true;

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

    public GameObject graphicsMenuUIObject;
    public PlayerController playerController;

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

        dither = true;
        Shader.SetGlobalInteger("_Dither", dither ? 1 : 0);
    }

    /// <summary>
    /// Runs every frame.
    /// </summary>
    void Update()
    {
        // Toggle graphics menu
        if (Input.GetKeyDown(KeyCode.M))
        {
            graphicsMenuUIObject.SetActive(!graphicsMenuUIObject.activeSelf);
            CameraController.Instance.ToggleCameraRotation();
        }

        /*
        // Toggle low resolution
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleLowResolution();
        }

        // Toggle Vertex Snapping
        if (Input.GetKeyDown(KeyCode.T))
        {
            snapVertices = !snapVertices;
            ToggleSnapVertices(snapVertices);
        }

        // Toggle AFM
        if (Input.GetKeyDown(KeyCode.Y))
        {
            affineTextureMapping = !affineTextureMapping;
            ToggleAffineTextureMapping(affineTextureMapping);
        }

        // Toggle Dithering
        if (Input.GetKeyDown(KeyCode.U))
        {
            dither = !dither;
            ToggleDither(dither);
        }

        // Toggle Color Quantization
        if (Input.GetKeyDown(KeyCode.I))
        {
            colorQuantization = !colorQuantization;
            ToggleColorQuantization(colorQuantization);
        }
        */
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


    public void ToggleSnapVertices()
    {
        snapVertices = !snapVertices;

        Shader.SetGlobalInteger("_Snap_Vertices", snapVertices ? 1 : 0);

        Debug.Log("Set Snap Vertices: " + snapVertices);
        TextPrompt.Instance.SetTextPrompt("Vertex Snapping: " + (snapVertices ? "Enabled" : "Disabled"));
    }


    public void ToggleAffineTextureMapping()
    {
        affineTextureMapping = !affineTextureMapping;

        Shader.SetGlobalInteger("_Affine_Texture_Mapping", affineTextureMapping ? 1 : 0);

        Debug.Log("Set Affine Texture Mapping: " + affineTextureMapping);
        TextPrompt.Instance.SetTextPrompt("Affine Texture Mapping: " + (affineTextureMapping ? "Enabled" : "Disabled"));
    }

    public void ToggleDither()
    {
        dither = !dither;

        Shader.SetGlobalInteger("_Dither", dither ? 1 : 0);

        Debug.Log("Set Dither: " + dither);
        TextPrompt.Instance.SetTextPrompt("Dither: " + (dither ? "Enabled" : "Disabled"));
    }

    public void ToggleColorQuantization()
    {
        colorQuantization = !colorQuantization;

        Shader.SetGlobalInteger("_Color_Quantization", colorQuantization ? 1 : 0);

        Debug.Log("Set Color Quantization: " + colorQuantization);
        TextPrompt.Instance.SetTextPrompt("Color Quantization: " + (colorQuantization ? "Enabled" : "Disabled"));
    }

    public void ToggleNoclip()
    {
        if (playerController) playerController.ToggleMovementMode();
    }

    public void TakeScreenshot()
    {
        StopAllCoroutines();
        StartCoroutine(TakeScreenshotRoutine());
    }

    public void AdjustFOV(float _fovChange)
    {
        _fovChange *= Time.deltaTime;

        Camera.main.fieldOfView += _fovChange;
    }

    public void SetFOV(float _fov)
    {
        Camera.main.fieldOfView = _fov;
    }
    
    IEnumerator TakeScreenshotRoutine()
    {
        graphicsMenuUIObject.SetActive(false);
        ScreenCapture.CaptureScreenshot("Screenshot_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png");

        yield return new WaitForSeconds(0.1f);

        graphicsMenuUIObject.SetActive(true);
    }
}