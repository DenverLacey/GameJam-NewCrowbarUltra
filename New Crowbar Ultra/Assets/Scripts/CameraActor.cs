using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraActor : MonoBehaviour
{
    [Tooltip("Smooth Time")]
    [SerializeField] private float m_smoothTime;

    private Robbo m_player;
    private Vector3 m_offset;
    private Vector3 m_velocity;

    // Start is called before the first frame update
    void Start() {
        m_player = FindObjectOfType<Robbo>();
        m_offset = transform.position - m_player.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.position = Vector3.SmoothDamp(transform.position, m_player.transform.position + m_offset, ref m_velocity, m_smoothTime);
    }
}
