using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class CrowbarThrow : MonoBehaviour
{
    private Rigidbody m_crowbarRb;
    private CrowbarDamage m_crowbar;
    private Vector3 m_origLocPos;
    private Vector3 m_origLocRot;
    private Vector3 m_pullPosition;
    private float m_returnTime;

    [Header("Public references")]
    public Transform weapon;
    public Transform hand;
    public Transform spine;
    public Transform curvePoint;
    [Space]
    [Header("Parameters")]
    public float m_throwPower = 100.0f;
    [Space]
    [Header("Bools")]
    public bool hasWeapon;
    public bool pulling;



    // Start is called before the first frame update
    void Start()
    {
        hasWeapon = true;
        pulling = false;
        m_crowbarRb = weapon.GetComponent<Rigidbody>();
        m_crowbar = weapon.GetComponent<CrowbarDamage>();
        m_origLocPos = weapon.localPosition;
        m_origLocRot = weapon.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !hasWeapon)
        {
            WeaponStartPull();
        }

        if(Input.GetMouseButtonDown(1) && hasWeapon)
        {
            WeaponThrow();
        }

        if (pulling)
        {
            if(m_returnTime < 1)
            {
                weapon.position = GetQuadraticCurvePoint(m_returnTime, m_pullPosition, curvePoint.position, hand.position);
                m_returnTime += Time.deltaTime * 1.5f;
            }
            else
            {
                WeaponCatch();
            }
        }
    }

    public void WeaponStartPull()
    {
        m_pullPosition = weapon.position;
        m_crowbarRb.Sleep();
        m_crowbarRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        m_crowbarRb.isKinematic = true;
        weapon.DORotate(new Vector3(-90, -90, 0), .2f).SetEase(Ease.InOutSine);
        weapon.DOBlendableLocalRotateBy(Vector3.right * 90, .5f);
        m_crowbar.activated = true;
        pulling = true;
    }

    public void WeaponCatch()
    {
        m_returnTime = 0;
        pulling = false;
        weapon.parent = hand;
        m_crowbar.activated = false;
        weapon.localEulerAngles = m_origLocRot;
        weapon.localPosition = m_origLocPos;
        hasWeapon = true;
    }

    public void WeaponThrow()
    {
        hasWeapon = false;
        m_crowbar.activated = true;
        m_crowbarRb.isKinematic = false;
        m_crowbarRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        weapon.parent = null;
        weapon.eulerAngles = new Vector3(0, -90 + transform.eulerAngles.y, 0);
        weapon.transform.position += transform.right / 5;
        m_crowbarRb.AddForce(transform.forward * m_throwPower + transform.up * 2, ForceMode.Impulse);
    }

    public Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
}