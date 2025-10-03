
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public LayerMask m_environmentLayerMask;
    [SerializeField] private PlayerController m_playerController;
    
    private void OnTriggerEnter(Collider other)
    {
        if (m_environmentLayerMask.IncludesLayer(other.gameObject.layer))
        {
            m_playerController.OnLanded(m_playerController.m_rigidbody.linearVelocity);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (m_environmentLayerMask.IncludesLayer(other.gameObject.layer))
        {
            m_playerController.m_isGrounded = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (m_environmentLayerMask.IncludesLayer(other.gameObject.layer))
        {
            m_playerController.m_isGrounded = false;
        }
    }
}