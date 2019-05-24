
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image healthBar;
    [SerializeField]
    private Robbo character;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (character)
            healthBar.fillAmount = character.GetComponent<Robbo>().m_health / character.GetComponent<Robbo>().m_maxHealth;
    }
}