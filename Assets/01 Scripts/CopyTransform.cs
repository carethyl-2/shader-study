/*
Bachelor of Software Engineering
Media Design School
Auckland
New Zealand
(c) 2024 Media Design School
File Name : CopyTransform.cs
Description : Will copy the values of another transform every frame.
Author : Kieran Bishop
Mail : kieran.bishop@mds.ac.nz
*/

using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    [SerializeField] Transform m_transformToCopy;
    [SerializeField] bool m_copyPosition = false;
    [SerializeField] bool m_copyRotation = false;
    [SerializeField] bool m_copyScale = false;
    [SerializeField] bool m_doInLateUpdate = false;
    [SerializeField] bool m_lerpPosition = false;
    [SerializeField] bool m_lerpRotation = false;
    [SerializeField] bool m_lerpScale = false;
    [SerializeField] float m_lerpSpeed = 10.0f;

    /// <summary>
    /// Runs every frame.
    /// </summary>
    void Update()
    {
        if (m_doInLateUpdate)
        {
            return;
        }

        PerformCopy();
    }

    /// <summary>
    /// Either lerp or copy transforms depending on m_lerpMovement.
    /// </summary>
    void PerformCopy()
    {
        CopyTransforms();
    }

    /// <summary>
    /// Runs every frame at the end of processing queue.
    /// </summary>
    void LateUpdate()
    {
        if (!m_doInLateUpdate)
        {
            return;
        }

        PerformCopy();
    }

    /// <summary>
    /// Lerp transform values of referenced object to this object, using lerpSpeed * deltaTime.
    /// </summary>
    void CopyTransforms()
    {
        // Ony function if reference to transform
        if (!m_transformToCopy)
        {
            return;
        }

        // Copy position
        if (m_copyPosition)
        {
            if (m_lerpPosition)
            {
                transform.position = Vector3.Lerp(transform.position, m_transformToCopy.position, Time.deltaTime * m_lerpSpeed);
            }

            else
            {
                transform.position = m_transformToCopy.position;
            }
        }

        // Copy rotation
        if (m_copyRotation)
        {
            if (m_lerpRotation)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, m_transformToCopy.rotation, Time.deltaTime * m_lerpSpeed);
            }

            else
            {
                
                transform.rotation = m_transformToCopy.rotation;
            }
        }

        // Copy scale
        if (m_copyScale)
        {
            if (m_lerpScale)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, m_transformToCopy.localScale, Time.deltaTime * m_lerpSpeed);
            }

            else
            {
                transform.localScale = m_transformToCopy.localScale;
            }
        }
    }
}
