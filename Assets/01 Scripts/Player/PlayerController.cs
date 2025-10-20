using UnityEngine.Events;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Rigidbody m_rigidbody;
    private Vector3 m_velocity;
    public float m_speed = 5.0f;
    public bool m_isGrounded;

    Vector3 m_input = Vector3.zero;

    [Header("Footsteps")]
    public float m_footstepTimeThreshold = 0.15f;
    const float m_footstepTimeScalar = 0.0005f;
    float m_footstepTimer;
    public UnityEvent m_footstepEvent;

    [Header("Jump/Land")]
    public float m_jumpPower = 5.0f;
    public UnityEvent m_jumpEvent;
    public UnityEvent<Vector3> m_landEvent;

    [Header("Noclip")]
    [SerializeField] Collider capsuleCollider;

    public enum MovementMode
    {
        FirstPerson,
        NoClip
    }

    public MovementMode movementMode = MovementMode.FirstPerson;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMovementMode();
        }

        // Move player
        Move();

        // Check jump inputs
        if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        m_rigidbody.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);

        m_jumpEvent.Invoke();
    }

    void Move()
    {
        // Assign axis inputs
        m_input.x = Input.GetAxisRaw("Horizontal");
        m_input.z = Input.GetAxisRaw("Vertical");

        // Calculate target velocities based on players orientation
        Vector3 forwardTargetVelocity = transform.forward * m_input.z;
        Vector3 sidewaysTargetVelocity = transform.right * m_input.x;

        // Normalize velocities and scale by speed
        Vector3 targetXZVelocity = (forwardTargetVelocity + sidewaysTargetVelocity).normalized * m_speed;



        if (movementMode == MovementMode.NoClip)
        {
            m_rigidbody.linearVelocity = new Vector3(targetXZVelocity.x, NoclipFlightInput() * m_speed, targetXZVelocity.z);
        }

        if (movementMode == MovementMode.FirstPerson)
        {
            // Assign velocity
            float controlFactor = m_isGrounded ? 1.0f : 0.25f;
            m_velocity = Vector3.Lerp(m_velocity, targetXZVelocity, Time.deltaTime * 4.0f * controlFactor);
            m_rigidbody.linearVelocity = new Vector3(m_velocity.x, m_rigidbody.linearVelocity.y, m_velocity.z);

            // Effects
            FootstepSFX();
        }
    }

    public void OnLanded(Vector3 _landingVelocity)
    {
        m_landEvent.Invoke(_landingVelocity);
    }

    void FootstepSFX()
    {
        // Play no footstep sfx if not grounded
        if (!m_isGrounded) { return; }

        m_footstepTimer -= m_rigidbody.linearVelocity.magnitude * Time.deltaTime;

        // If footstep interval has passed
        if (m_footstepTimer <= 0.0f)
        {
            // Play footstep sfx
            m_footstepTimer = m_footstepTimeThreshold;
            m_footstepEvent.Invoke();
        }
    }

    public void ToggleMovementMode()
    {
        switch (movementMode)
        {
            case MovementMode.FirstPerson:
                movementMode = MovementMode.NoClip;
                TextPrompt.Instance.SetTextPrompt("Mode: NoClip");
                m_rigidbody.useGravity = false;
                capsuleCollider.enabled = false;
                break;
            case MovementMode.NoClip:
                movementMode = MovementMode.FirstPerson;
                TextPrompt.Instance.SetTextPrompt("Mode: FirstPerson");
                m_rigidbody.useGravity = true;
                capsuleCollider.enabled = true;
                break;

            default:
                break;
        }
    }
    
    float NoclipFlightInput()
    {
        float input = 0.0f;
        if (Input.GetKey(KeyCode.Q))
        {
            input += 1.0f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            input -= 1.0f;
        }
        return input;
    }
}
