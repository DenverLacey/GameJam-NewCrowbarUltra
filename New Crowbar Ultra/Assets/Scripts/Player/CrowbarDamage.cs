using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarDamage : MonoBehaviour
{
    [Tooltip("Damage to the enemies")]
    [SerializeField] private float m_damage;

   // [Tooltip("Blood squirt when enemy gets hit")]
    //[SerializeField] private GameObject m_blood;

    void OnCollisionEnter(Collision other)
    {
        string tag = other.gameObject.tag;
        if(other.gameObject.GetComponent<EnemyActor>())
        {
            EnemyActor enemy = other.gameObject.GetComponent<EnemyActor>();
            enemy.TakeDamage(m_damage);
            
            Debug.Log("attacked enemy");
            //GameObject temp = Instantiate(m_blood, transform.position, transform.rotation) as GameObject;
            //Destroy(temp);
        }
        else
        {
            Debug.Log("attck failed");
        }
    }
}