
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public PlayerController m_controller;

    private void Awake()
    {
        Instance = this;
        m_controller = GetComponentInChildren<PlayerController>();
    }
}
