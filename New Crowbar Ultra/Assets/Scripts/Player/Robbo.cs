/*
 * Summary:	Roboo character mechanics and variables
 * Author:	Elisha Anagnostakis
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

	private Animator m_animator;
	public bool Attacking { get; set; }

	private Vector2 m_IKGoal;

    private GameObject m_crowBar;
    public float m_health;


    // Use this for initialization
    void Start () {
        m_health = m_maxHealth;
		m_animator = GetComponent<Animator>();
		// m_crowBar = GameObject.FindGameObjectWithTag("Crowbar");
		Attacking = false;
	}

    // Update is called once per frame
    protected void FixedUpdate() {
		if (Input.GetMouseButtonDown(0)) {
			Attacking = true;
		}
		if (Input.GetMouseButtonUp(0)) {
			Attacking = false;
		}

		RobboMovement();
	}

	private void OnAnimatorIK() {
		if (Attacking) {
			m_IKGoal += new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Moues Y"));
			m_animator.SetIKPosition(AvatarIKGoal.RightHand, new Vector3(m_IKGoal.x, m_IKGoal.y, 0));
			m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
		}
		else {
			m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
		}
	}

	public void RobboMovement()
    {
		float rotInput = Input.GetAxis("Horizontal");
		float movInput = Input.GetAxis("Vertical");

        transform.Rotate(0, rotInput * m_rotSpeed, 0);
        transform.Translate(0, 0, movInput * m_moveSpeed * Time.deltaTime);
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
