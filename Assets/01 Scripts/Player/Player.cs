
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public static Player Instance;
    [FormerlySerializedAs("controller")] public PlayerController m_controller;

    private void Awake()
    {
        Instance = this;
        m_controller = GetComponentInChildren<PlayerController>();
    }
}
