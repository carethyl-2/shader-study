using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public PlayerController m_playerController;
    
    public float m_pitch = 0.0f;
    
    public float m_minPitch = -89.0f;
    public float m_maxPitch = 89.0f;

    public float m_verticalSensitivity = 5f;
    public float m_horizontalSensitivity = 5.0f;

    [Header("Interactable Raycast")]
    [SerializeField] float m_interactionRange = 3.0f;
    public bool m_canInteract = true;
    [SerializeField] IInteractable m_focusedInteractable;

    public bool rotateCamera = true;

    private void Awake()
    {
        Instance = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToggleCameraRotation()
    {
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;

        rotateCamera = !rotateCamera;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCameraRotation();
        }

        
        // Interaction
        InteractableCheck();
        if (Input.GetKeyDown(KeyCode.E)) { Interact(); }

        if (!rotateCamera) { return; }
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));


        m_pitch += mouseInput.y * m_verticalSensitivity;
        m_pitch = Mathf.Clamp(m_pitch, m_minPitch, m_maxPitch);
        
        transform.localRotation = Quaternion.Euler(m_pitch, transform.localRotation.y, transform.localRotation.z);

        Vector3 newAngles = m_playerController.transform.localRotation.eulerAngles + Vector3.up * (mouseInput.x * m_horizontalSensitivity);
        m_playerController.m_rigidbody.MoveRotation(Quaternion.Euler(newAngles));
    }

    /// <summary>  
    /// Interact with focused object.
    /// </summary>
    public void Interact()
    {
        // If cannot interact, do nothing
        if (!m_canInteract) { return; }

        // Call interact on focused object
        if (m_focusedInteractable != null) { m_focusedInteractable.Interact(); }
    }

    /// <summary>  
    /// Raycast infront of camera for interactable objects.
    /// </summary>
    void InteractableCheck()
    {
        // If cannot interact
        if (!m_canInteract)
        {
            // Defocus
            if (m_focusedInteractable != null)
            {
                m_focusedInteractable.OnLookAtEnd();
                m_focusedInteractable = null;
            }

            // And do nothing
            return;
        }

        // Draw interact ray
        Debug.DrawRay(transform.position, transform.forward * m_interactionRange, Color.red);

        // Perform raycast
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, m_interactionRange))
        {
            // If object being looked at is interactable
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactableObject))
            {
                // If no object is in focus
                if (m_focusedInteractable == null)
                {
                    // Focus on interactable
                    m_focusedInteractable = interactableObject;
                    m_focusedInteractable.OnLookAtStart();
                }

                // If object in focus is different from new object being looked at
                else if (m_focusedInteractable != interactableObject)
                {
                    // Switch focus
                    m_focusedInteractable.OnLookAtEnd();
                    m_focusedInteractable = interactableObject;
                    m_focusedInteractable.OnLookAtStart();
                }

                // Still looking at same interactable
                return;
            }
        }

        // If raycast doesn't find an interactable, but one was previously looked at
        if (m_focusedInteractable != null)
        {
            m_focusedInteractable.OnLookAtEnd();
            m_focusedInteractable = null;
        }
    }
}
