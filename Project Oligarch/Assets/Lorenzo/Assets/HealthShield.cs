using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthShield : MonoBehaviour
{
    public float Health;
    public float maxHealth;
    public float Shield;
    public float maxShield;

    [SerializeField] Image Healthbar;
    [SerializeField] Image Shieldbar;

    void Start()
    {
        Health = maxHealth;
        Shield = maxShield;
    }

    // Update is called once per frame
    void Update()
    {
        Healthbar.fillAmount = Health / maxHealth;
        Shieldbar.fillAmount = Shield / maxShield;
    }
    
}
