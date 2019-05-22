using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarDamage : MonoBehaviour
{
    [Tooltip("Damage to the enemies")]
    [SerializeField] private float m_damage;

    [Tooltip("Blood squirt when enemy gets hit")]
    [SerializeField] private GameObject m_blood;
    void OnCollisionEnter(Collision other)
    {
        string tag = other.gameObject.tag;
        if(tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyActor>().TakeDamage(m_damage);
            GameObject temp = Instantiate(m_blood, transform.position, Quaternion.identity);
            Destroy(temp, 1);
        }
    }
}