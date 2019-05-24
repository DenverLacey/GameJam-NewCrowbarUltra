using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWin : MonoBehaviour
{
    public void OnTriggerEnter(Collider Collision)
    {
        if (Collision.gameObject.GetComponent<Robbo>())
        {
            GameManager manager = FindObjectOfType<GameManager>();
            manager.WinState();
        }
    }
}
