using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Summary:	Handles all enemies in room instance, turns enemies agro
 * Author:	Denver Lacey
 * Date:	5/22/19
 */

public class EnemyRoom : MonoBehaviour
{
	private BoxCollider[] m_colliders;
	private List<EnemyActor> m_actors;

    // Start is called before the first frame update
    void Awake() {
		m_actors = new List<EnemyActor>();
    }

	private void Start() {
		m_colliders = GetComponents<BoxCollider>();
	}

	/// <summary>
	///		If collided with player, all enemies.Agro = true
	///		If collided with enemy, added to m_actors
	/// </summary>
	/// <param name="other">
	///		A Collider. The Collider that has entered the room
	/// </param>
	private void OnTriggerEnter(Collider other) {
		// if other is player
		if (other.GetComponent<Robbo>()) {
			foreach (EnemyActor a in m_actors) {
				a.Agro = true;
			}
		}
		// if other is an enemy actor
		else if (other.GetComponent<EnemyActor>()) {
			m_actors.Add(other.GetComponent<EnemyActor>());
			m_actors[m_actors.Count - 1].Room = this;
		}
	}

	/// <summary>
	///		Finds if point is inside room
	/// </summary>
	/// <param name="point">
	///		 A Vector3. The point the user is querying
	/// </param>
	/// <returns>
	///		A Boolean. If point is inside room
	/// </returns>
	public bool IsPointInRoom(Vector3 point) {
		foreach (BoxCollider c in m_colliders) {
			if (IsPointInCollider(point, c)) {
				return true;
			}
		}
		return false;
	}

	/// <summary>
	///		Finds if point is inside collider
	/// </summary>
	/// <param name="point">
	///		A Vector3. The point the user is querying
	/// </param>
	/// <param name="collider">
	///		A BoxCollider. The collider the user is querying
	/// </param>
	/// <returns>
	///		A Boolean. If point is inside collider
	/// </returns>
	bool IsPointInCollider(Vector3 point, BoxCollider collider) {
		if (collider.bounds.min.x <= point.x && point.x <= collider.bounds.max.x &&
			collider.bounds.min.y <= point.y && point.y <= collider.bounds.max.y) 
		{
			return true;
		}
		return false;
	}

	/// <summary>
	///		Finds a random point within the room
	/// </summary>
	/// <returns>
	///		A Vector3. The random point that was calculated
	/// </returns>
	public Vector3 RandomPoint() {
		// pick random collider in room
		int index = Random.Range(0, m_colliders.Length);
		BoxCollider collider = m_colliders[index];

		// define boundary corners
		float minx = collider.bounds.min.x;
		float maxx = collider.bounds.max.x;
		float minz = collider.bounds.min.z;
		float maxz = collider.bounds.max.z;

		// lerp to find random point within collider
		float x = Mathf.Lerp(minx, maxx, Random.Range(0f, 1f));
		float z = Mathf.Lerp(minz, maxz, Random.Range(0f, 1f));

		return new Vector3(x, 0, z);
	}
}
