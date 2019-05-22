﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Summary:	Handles decision making, pathfinding and attack functionality of enemy
 * Authour:	Denver Lacey
 * Date:	5/22/19
 */

 [RequireComponent(typeof(NavMeshAgent))]
public class EnemyActor : MonoBehaviour
{
	[Tooltip("How far away the enemy can see the player from.")]
	[SerializeField] private float m_viewRange;

	[Tooltip("How far can enemy reach with melee attack.")]
	[SerializeField] private float m_attackRange;

	[Tooltip("How fast the enemy can make consecutive melees.")]
	[SerializeField] private float m_meleeCooldown;

	[Tooltip("Enemy's maximum amount of health.")]
	[SerializeField] private int m_maxHealth;

	[Tooltip("How much damage the enemy will deal.")]
	[SerializeField] private int m_damage;

	private float m_health;
	public float Health { get => m_health; }

	private NavMeshAgent m_navMeshAgent;
	private Transform m_player;
	private Vector3 m_target;

	private float m_meleeTimer;

	private enum AI_STATE {
		WANDER,
		ATTACK
	};

	private AI_STATE m_currentState;
	private AI_STATE m_oldState;

	void Start() {
		m_navMeshAgent = GetComponent<NavMeshAgent>();

		m_player = GameObject.FindGameObjectWithTag("Player").transform;
		m_currentState = AI_STATE.WANDER;
		m_oldState = m_currentState;
		m_health = m_maxHealth;
		m_meleeTimer = m_meleeCooldown;
	}

	/// <summary>
	///		Calls appropriate function based current State
	/// </summary>
	void Update() {
		// determine state
		m_currentState = DetermineState();

		switch (m_currentState) {
			case AI_STATE.WANDER:
				Wander();
				break;

			case AI_STATE.ATTACK:
				Attack();
				break;

			default:
				Debug.LogError("State couldn't be determined!", this);
				break;
		}
		m_navMeshAgent.destination = m_target;
	}

	/// <summary>
	///		Determines whether enemy should wander or attack
	/// </summary>
	/// <returns>
	///		Desired AI_STATE
	/// </returns>
	AI_STATE DetermineState() {
		AI_STATE s = AI_STATE.WANDER;

		Ray ray = new Ray(transform.position, m_player.position - transform.position);

		if (Physics.Raycast(ray, out RaycastHit hit, m_viewRange)) {
			if (hit.collider.tag == "Player") {
				s = AI_STATE.ATTACK;
			}
		}

		return s;
	}

	/// <summary>
	///		Enemy wanders around nav mesh
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
		point = Vector3.Lerp(point, data.vertices[t + 1], Random.Range(0, 1));
		point = Vector3.Lerp(point, data.vertices[t + 2], Random.Range(0, 1));

		// set target destination
		m_target = point;

		m_oldState = AI_STATE.WANDER;
	}

	/// <summary>
	///		Attacks player
	/// </summary>
	void Attack() {
		m_target = m_player.position + (transform.position - m_player.position).normalized * m_attackRange;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_player.position - transform.position), .1f);
		if (Vector3.Distance(transform.position, m_target) <= m_attackRange) {
			m_meleeTimer -= Time.deltaTime;
			if (m_meleeTimer <= 0.0f) {
				if (Vector3.Distance(transform.position, m_player.position) <= m_attackRange) {
					// m_player.TakeDamage(m_damage);
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
	///		Damages Enemy's health
	/// </summary>
	/// <param name="damage">
	///		how much damage was dealt to enemy
	/// </param>
	public void TakeDamage(float damage) {
		m_health -= damage;

		if (m_health <= 0.0f) {
			Destroy(gameObject);
		}
	}

	/// <summary>
	///		Finds the closest point on the nav mesh to a source position
	/// </summary>
	/// <param name="source">
	///		A Vector3. The source position the user wants to query
	/// </param>
	/// <returns>
	///		A Vector3. The closest point on the nav mesh. Retuens Vector3.positiveInfinity if none found
	/// </returns>
	Vector3 FindClosestPoint(Vector3 source) {
		if (NavMesh.SamplePosition(source, out NavMeshHit hit, transform.lossyScale.y, NavMesh.GetAreaFromName("walkable"))) {
			if (hit.hit) {
				return source;
			}
		}
		if (NavMesh.FindClosestEdge(source, out hit, NavMesh.GetAreaFromName("walkable"))) {
			return hit.position;
		}
		return Vector3.positiveInfinity;
	}
}
