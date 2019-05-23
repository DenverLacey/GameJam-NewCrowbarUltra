using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarDamage : MonoBehaviour
{
    [Tooltip("Damage to the enemies")]
    [SerializeField] private float m_damage;

    public bool activated;

    public float m_rotationSpeed;

    public void Update()
    {
        if(activated)
        {
            transform.localEulerAngles += Vector3.forward + transform.up * m_rotationSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision Collision)
    {
        string tag = Collision.gameObject.tag;
        if(Collision.gameObject.layer == 11 && Collision.gameObject.GetComponent<EnemyActor>())
        {
            Debug.Log(Collision.gameObject.name);
            GetComponent<Rigidbody>().Sleep();
            GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            GetComponent<Rigidbody>().isKinematic = true;
            activated = false;

            EnemyActor enemy = Collision.gameObject.GetComponent<EnemyActor>();
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