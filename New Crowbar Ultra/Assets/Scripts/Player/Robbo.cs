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
    [SerializeField] private float m_maxHealth = 100f;

    [Tooltip("Robbos speed at which he rotates")]
    [SerializeField] private float m_rotSpeed = 10;

    [Tooltip("Robbos speed at which he rotates")]
    [SerializeField] private float m_moveSpeed = 20f;

    private MultiTargetCamera m_camera;
    private GameObject m_crowBar;
    private float m_health;

    // Use this for initialization
    void Start () {
        m_health = m_maxHealth;
        // m_crowBar = GameObject.FindGameObjectWithTag("Crowbar");
        m_camera = FindObjectOfType<MultiTargetCamera>();

		if (!m_camera) {
			Debug.LogError("NO CAMERA!!!", this);
		}
	}

    // Update is called once per frame
    protected void FixedUpdate() {
		Slash();
		RobboMovement();
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
		float rotInput = Input.GetAxis("Horizontal");
		float movInput = Input.GetAxis("Vertical");

        transform.Rotate(0, rotInput * m_rotSpeed, 0);
        transform.Translate(0, 0, movInput * m_moveSpeed * Time.deltaTime);
    }
}
