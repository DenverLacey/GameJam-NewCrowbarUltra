using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarDamage : MonoBehaviour
{
    [Tooltip("Damage to the enemies")]
    [SerializeField] private float m_damage;

    private GameObject m_enemy;
    [SerializeField] private GameObject m_blood;

    private void Start()
    {
        m_damage = 10.0f;
        m_enemy = GameObject.FindGameObjectWithTag("Enemy");
    }
    void OnCollisionEnter(Collision other)
    {
        string tag = other.gameObject.tag;
        if(tag == "Enemy")
        {
            //m_enemy.TakeDamage(m_damage);
            GameObject temp = Instantiate(m_blood, transform.localPosition, Quaternion.identity);
            Destroy(temp, 1);
        }
    }
}