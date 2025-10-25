using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public PlayerController m_playerController;
    
    public float m_pitch = 0.0f;
    
    public float m_maxPitch = 89.0f;

    public float m_sensitivity = 1f;

    [SerializeField] Light flashlight;

    [Header("Interactable Raycast")]
    [SerializeField] bool m_interaction = true;
    [SerializeField] float m_interactionRange = 3.0f;
    public bool m_canInteract = true;
    [SerializeField] IInteractable m_focusedInteractable;

    public bool rotateCamera = true;

    Vector2 m_rawMouseInput = Vector2.zero;
    Vector2 m_smoothedMouseInput = Vector2.zero;

    private void Awake()
    {
        Instance = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToggleCameraRotation()
    {
        rotateCamera = !rotateCamera;
        Cursor.lockState = rotateCamera ? CursorLockMode.Locked : CursorLockMode.None;
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

        if (Input.GetKeyDown(KeyCode.F)) { flashlight.enabled = !flashlight.enabled; }



        m_smoothedMouseInput = Vector2.Lerp(m_smoothedMouseInput, m_rawMouseInput, Time.deltaTime * 10.0f);

        if (!rotateCamera)
        {
            m_rawMouseInput = Vector2.zero;
            return;
        }
        
        m_rawMouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
  
    }

    void LateUpdate()
    {
        Vector2 input = m_smoothedMouseInput;
        input *= m_sensitivity;

        m_pitch -= input.y;
        m_pitch = Mathf.Clamp(m_pitch, -m_maxPitch, m_maxPitch);
        
        transform.localRotation = Quaternion.Euler(m_pitch, transform.localRotation.y, transform.localRotation.z);

        //Vector3 newAngles = m_playerController.transform.localRotation.eulerAngles + Vector3.up * m_mouseInput.x;
        //m_playerController.m_rigidbody.MoveRotation(Quaternion.Euler(newAngles));

        m_playerController.transform.Rotate(new Vector3(0.0f, input.x, 0.0f));
    }

    /// <summary>  
    /// Interact with focused object.
    /// </summary>
    public void Interact()
    {
        if (!m_interaction) { return; }

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

        if (!m_interaction) { return; }

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
