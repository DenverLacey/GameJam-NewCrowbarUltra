//*
// @brief Roboo character mechanics and variables
// Author: Elisha Anagnostakis
// Date: 22/05/19
//*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robbo : MonoBehaviour {

    [Tooltip("Robbos starting health")]
    [SerializeField] private float m_maxHealth;

    [Tooltip("Robbos speed at which he rotates")]
    [SerializeField] private float m_rotSpeed;

    [Tooltip("Robbos speed at which he rotates")]
    [SerializeField] private float m_moveSpeed;

    private MultiTargetCamera m_camera;
    private GameObject m_robbo;
    private GameObject m_crowBar;
    private float m_health;


    // Use this for initialization
    void Start () {
        m_rotSpeed = 500.0f;
        m_moveSpeed = 20.0f;
        m_maxHealth = 100f;
        m_health = m_maxHealth;
        m_robbo = GameObject.FindGameObjectWithTag("Robbo");
        m_crowBar = GameObject.FindGameObjectWithTag("Crowbar");
        m_camera = GameObject.FindObjectOfType<MultiTargetCamera>();
	}

    // Update is called once per frame
    protected void FixedUpdate() {
        if (m_robbo != null)
        {
            Slash();
            RobboMovement();
        }
	}

    public void Slash() {
        if (m_crowBar != null)
        {
            Vector3 mouesInput = new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0);
            // do stuff
        }
    }

    public void RobboMovement()
    {
        var rotInput = Input.GetAxis("Horizontal") * Time.deltaTime * m_rotSpeed;
        var movInput = Input.GetAxis("Vertical") * Time.deltaTime * m_moveSpeed;

        Vector3 rawDir = m_camera.transform.TransformDirection(rotInput, 0, movInput);
        float translate = rawDir.z;

        transform.Rotate(0, rotInput, 0);
        transform.Translate(0, 0, translate);
    }
}     