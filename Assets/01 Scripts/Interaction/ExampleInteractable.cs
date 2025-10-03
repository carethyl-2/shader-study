/*
Bachelor of Software Engineering
Media Design School
Auckland
New Zealand
(c) 2025 Media Design School
File Name : ExampleInteractable.cs
Description : Example class of using interactable interface.
Author : Kieran Bishop
Mail : kieran.bishop@mds.ac.nz
*/

using UnityEngine;

public class ExampleInteractable : MonoBehaviour, IInteractable
{
    Vector3 m_startingPosition;
    float m_lerpSpeed = 5.0f;

    Material m_defaultMaterial;

    [SerializeField] MeshRenderer m_meshRenderer;
    [SerializeField] Material m_glowingMaterial;

    /// <summary>
    /// Execution begins here.
    /// </summary>
    void Start()
    {
        m_startingPosition = transform.position;

        m_defaultMaterial = m_meshRenderer.material;
    }

    /// <summary>
    /// Runs every frame.
    /// </summary>
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, m_startingPosition, Time.deltaTime * m_lerpSpeed);
    }

    /// <summary>
    /// Implementation for IInteractable interface.
    /// </summary>
    public void Interact()
    {
        Vector3 newPositionOffset = Random.insideUnitSphere.normalized;
        transform.position += newPositionOffset;

        //GameManager.Instance.m_player.m_firstPersonComponent.PickupItem(gameObject);
    }

    /// <summary>
    /// Implementation for IInteractable interface.
    /// </summary>
    public void OnLookAtStart()
    {
        m_meshRenderer.material = m_glowingMaterial;
        //Debug.Log("Started looking at example interactabl.");
    }

    /// <summary>
    /// Implementation for IInteractable interface.
    /// </summary>
    public void OnLookAtEnd()
    {
        m_meshRenderer.material = m_defaultMaterial;
        //Debug.Log("Stopped looking at example interactable.");
    }
}
