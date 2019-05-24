/*
 * Summary:	Roboo character mechanics and variables
 * Author:	Elisha Anagnostakis / Denver Lacey
 * Date:	22/05/19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Robbo : MonoBehaviour {

    [Tooltip("Robbos starting health")]
    [SerializeField] public float m_maxHealth = 100f;

    [Tooltip("Robbos speed at which he rotates")]
    [SerializeField] private float m_rotSpeed = 10;

    [Tooltip("Robbos speed at which he rotates")]
    [SerializeField] private float m_moveSpeed = 20f;

	[Header("Picking up crowbar stuff")]
	[Tooltip("Key to pick up crowbar")]
	[SerializeField] private KeyCode m_pickupCrowbarKey;

	[Tooltip("How far away player can be and still pick up crowbar")]
	[SerializeField] private float m_pickupDistance;

	[Tooltip("Robbo's right hand transform")]
	[SerializeField] private Transform m_rightHand;

	[Tooltip("Crowbar offset position")]
	[SerializeField] private Vector3 m_crowbarOffsetPosition;

	[Tooltip("Crowbar offset rotation")]
	[SerializeField] private Vector3 m_crowbarOffsetRotation;

	[Tooltip("Crowbar offset scale")]
	[SerializeField] private Vector3 m_crowbarOffsetScale;

	[Header("Swinging crowbar stuff")]
	[Tooltip("IK Goal for right hand")]
	[SerializeField] private Transform m_ikGoal;

	[Tooltip("Robbo's right arm transform")]
	[SerializeField] private Transform m_rightArm;

	[Tooltip("Moues Sensitivity")]
	[SerializeField] private float m_mouseSensitivity;

	[Tooltip("Min value for clamping arm movement")]
	[SerializeField] private float m_minArmConstraint;

	[Tooltip("Max value for clamping arm movement")]
	[SerializeField] private float m_maxArmConstraint;

    public GameObject m_crowBar;

    [HideInInspector]
    public float m_health;

	private Camera m_camera;

	bool m_attacking;
	private Animator m_animator;

    // Use this for initialization
    void Start () {
        m_health = m_maxHealth;
		m_camera = Camera.main;
		m_animator = GetComponent<Animator>();

		// m_crowBar = GameObject.FindGameObjectWithTag("Crowbar");
	}

	void Update() {
		if (Input.GetKeyDown(m_pickupCrowbarKey) &&
			Vector3.Distance(transform.position, m_crowBar.transform.position) <= m_pickupDistance) 
		{
			m_crowBar.transform.parent = m_rightHand;
			m_crowBar.transform.localPosition = m_crowbarOffsetPosition;
			m_crowBar.transform.localRotation = Quaternion.Euler(m_crowbarOffsetRotation);
			m_crowBar.transform.localScale = m_crowbarOffsetScale;
		}
    }

    // Update is called once per frame
    void FixedUpdate() {
		if (Input.GetMouseButton(0)) {
			m_attacking = true;
			m_animator.SetLayerWeight(1, 1);
			SwingCrowbar();
		}
		else {
			m_attacking = false;
			m_animator.SetLayerWeight(1, 0);
		}
		DoMovement();
	}

	void DoMovement()
    {
		float movInput = Input.GetAxis("Vertical");
		float rotInput = Input.GetAxis("Horizontal");

		m_animator.SetFloat("Speed", movInput);

		transform.Translate(0, 0, movInput * m_moveSpeed * Time.deltaTime);
		transform.Rotate(Vector3.up, rotInput * m_rotSpeed);
    }
	private void OnAnimatorIK() {
		if (m_attacking) {
			m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
			m_animator.SetIKPosition(AvatarIKGoal.RightHand, m_ikGoal.position);
		}
		else {
			m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
		}
	}

	void SwingCrowbar() {
		m_ikGoal.LookAt(m_rightArm.position);
		m_ikGoal.Translate(Input.GetAxis("Mouse X") * m_mouseSensitivity * Time.deltaTime, 0, 0);

		Vector3 direction = (m_ikGoal.position - transform.position).normalized;
		direction.y = 0;

		Vector3 position = transform.position + direction * 5;
		position.y = m_rightArm.position.y;

		m_ikGoal.position = position;
	}

	public void TakeDamage(float damage)
    {
        m_health -= damage;
        if(m_health <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
