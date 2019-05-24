/*
 * Summary:	Handles decision making, pathfinding and attack functionality of enemy
 * Author:	Denver Lacey
 * Date:	22/05/19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyActor : MonoBehaviour
{
	[Tooltip("How far away the enemy can see the player from.")]
	[SerializeField] private float m_viewRange = 10f;

	[Tooltip("How far can enemy reach with melee attack.")]
	[SerializeField] private float m_attackRange = 1.5f;

	[Tooltip("How fast the enemy can make consecutive melees.")]
	[SerializeField] private float m_meleeCooldown = 1f;

	[Tooltip("Enemy's maximum amount of health.")]
	[SerializeField] private float  m_maxHealth = 10f;

	[Tooltip("How much damage the enemy will deal.")]
	[SerializeField] private float m_damage = 10f;

	private float m_health;
	public float Health { get => m_health; }

	private NavMeshAgent m_navMeshAgent;
	private Robbo m_player;
	private Vector3 m_target;

	private float m_meleeTimer;

	public bool Agro { get; set; }
	public EnemyRoom Room { get; set; }

	private Animator m_animator;

	private enum AI_STATE {
		WANDER,
		ATTACK
	};

	private AI_STATE m_currentState;
	private AI_STATE m_oldState;

	void Start() {
		m_navMeshAgent = GetComponent<NavMeshAgent>();
		m_animator = GetComponent<Animator>();

		m_player = FindObjectOfType<Robbo>();
		m_currentState = AI_STATE.WANDER;
		m_oldState = m_currentState;
		m_health = m_maxHealth;
		m_meleeTimer = 0f;
		Agro = false;
	}

	/// <summary>
	///	Calls appropriate function based on current State
	/// </summary>
	void Update() {
		// determine state
		m_currentState = DetermineState();

		switch (m_currentState) {
			case AI_STATE.WANDER:
				if (Agro)
					Wander(); 
				else
					WanderRoom();
				break;

			case AI_STATE.ATTACK:
				if (Agro)
					Attack();
				break;

			default:
				Debug.LogError("State couldn't be determined!", this);
				break;
		}
		m_navMeshAgent.destination = m_target;

		// run animation
		m_animator.SetFloat("Speed", m_navMeshAgent.velocity.magnitude / m_navMeshAgent.speed);
	}

	/// <summary>
	///	Determines whether enemy should wander or attack
	/// </summary>
	/// <returns>
	///	An AI_STATE. Desired AI_STATE
	/// </returns>
	AI_STATE DetermineState() {
		AI_STATE s = AI_STATE.WANDER;

		Ray ray = new Ray(transform.position, m_player.transform.position - transform.position);

		if (Physics.Raycast(ray, out RaycastHit hit, m_viewRange)) {
			if (hit.collider.GetComponent<Robbo>()) {
				s = AI_STATE.ATTACK;
			}
		}

		return s;
	}

	/// <summary>
	///	Enemy wanders around nav mesh
	/// </summary>
	void Wander() {
		if (Vector3.Distance(transform.position, m_target) > 2f && m_oldState == AI_STATE.WANDER) {
			return;
		}

		// get nav mesh triangulation
		NavMeshTriangulation data = NavMesh.CalculateTriangulation();

		// pick random triangle (t)
		int t = 3 * Random.Range(0, data.vertices.Length / 3);

		// get vertex at t
		Vector3 point = data.vertices[t];

		// lerp point so point can be anywhere within a tri
		point = Vector3.Lerp(point, data.vertices[t + 1], Random.Range(0f, 1f));
		point = Vector3.Lerp(point, data.vertices[t + 2], Random.Range(0f, 1f));

		// set target destination
		m_target = point;

		m_oldState = AI_STATE.WANDER;
	}

	/// <summary>
	///	Enemy wanders just around their room
	/// </summary>
	void WanderRoom() {
		if (Vector3.Distance(transform.position, m_target) > 2f && m_oldState == AI_STATE.WANDER) {
			return;
		}

		// get random point in room
		Vector3 point = Room.RandomPoint();

		// find closest point on the nav mesh
		point = FindClosestPoint(point);

		// set target destination
		m_target = point;

		m_oldState = AI_STATE.WANDER;
	}

	/// <summary>
	///	Attacks player
	/// </summary>
	void Attack() {
		m_target = m_player.transform.position + (transform.position - m_player.transform.position).normalized * m_attackRange;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_player.transform.position - transform.position), .1f);
		if (Vector3.Distance(transform.position, m_target) <= m_attackRange) {
			m_meleeTimer -= Time.deltaTime;
			if (m_meleeTimer <= 0.0f) {
				if (Vector3.Distance(transform.position, m_player.transform.position) <= m_attackRange + 0.1f) {
					m_animator.SetTrigger("Attack");
					m_player.TakeDamage(m_damage);
					m_meleeTimer = m_meleeCooldown;
				}
			}
		}
		else {
			m_meleeTimer = m_meleeCooldown;
		}
		m_oldState = AI_STATE.ATTACK;
	}

	/// <summary>
	///	Damages Enemy's health
	/// </summary>
	/// <param name="damage">
	///	A Float. How much damage is dealt to enemy
	/// </param>
	public void TakeDamage(float damage) {
		m_health -= damage;

		if (m_health <= 0.0f) {
			Destroy(gameObject);
		}
	}

	/// <summary>
	///	Finds the closest point on the nav mesh to a source position
	/// </summary>
	/// <param name="source">
	///	A Vector3. The source position the user wants to query
	/// </param>
	/// <returns>
	///	A Vector3. The closest point on the nav mesh. Retuens Vector3.positiveInfinity if none found
	/// </returns>
	Vector3 FindClosestPoint(Vector3 source) {
		if (NavMesh.SamplePosition(source, out NavMeshHit hit, transform.lossyScale.y, NavMesh.GetAreaFromName("walkable"))) {
			if (hit.hit) {
				return source;
			}
		}
		if (NavMesh.FindClosestEdge(source, out hit, NavMesh.GetAreaFromName("walkable"))) {
			if (hit.hit) {
				return hit.position;
			}
		}
		return Vector3.positiveInfinity;
	}
}
