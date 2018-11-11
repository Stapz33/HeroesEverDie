using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour {

    [Header("Heal")]
    
    private float f_PlayerHP = 100f;
    private float f_MaxHealth;

    private float f_HealMultiplier;
    private float f_DamageMultiplier;

    private bool b_IsInHealZone = false;
    private bool b_IsPlayerMoving;

    private bool b_IsInSafeZone;


    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start ()
    {
        SetupPlayer();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(b_IsInHealZone && f_PlayerHP < f_MaxHealth)
        {
            f_PlayerHP += Time.deltaTime * f_HealMultiplier;
            if (f_PlayerHP > f_MaxHealth)
                f_PlayerHP = f_MaxHealth;
        }
        else if (b_IsPlayerMoving && f_PlayerHP > 0 && !b_IsInHealZone)
        {
            f_PlayerHP -= Time.deltaTime * f_DamageMultiplier;
            if (f_PlayerHP <= 0)
                Destroy(gameObject);
        }
	}

    public void HealZone()
    {
        b_IsInHealZone = true;
    }

    public void NoHealZone()
    {
        b_IsInHealZone = false;
    }

    public void ZoneSafe()
    {
        if (!b_IsInSafeZone)
        b_IsInSafeZone = true;
    }

    public void NotZoneSafe()
    {
        if (b_IsInSafeZone)
        b_IsInSafeZone = false;
    }

    public void GroundHeal()
    {
        
    }

    public void PlayerIsMoving()
    {
        if (!b_IsPlayerMoving)
        {
            b_IsPlayerMoving = true;
        }
    }

    public void PlayerIsNotMoving()
    {
        if (b_IsPlayerMoving)
        {
            b_IsPlayerMoving = false;
        }
    }

    public void SetupPlayer()
    {
        f_MaxHealth = GameManager.s_Singleton.GetGenericMaxHealth();
        f_PlayerHP = f_MaxHealth;
        f_HealMultiplier = GameManager.s_Singleton.GetGenericHealMultiplier();
        f_DamageMultiplier = GameManager.s_Singleton.GetGenericDamageMultiplier();
    }
}
