using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager s_Singleton;

    private List<Player_Manager> PlayersList = new List<Player_Manager>();

    public float MaxHealth;
    public float HealMultiplier;
    public float DamageMultiplier;

    public List<float> l_CoolDown;
    private bool b_IsOkToRandom;
    private float ZoneTimer;

    private bool b_IsInSafeZone;
    public GameObject g_ZoneSafePrefab;
    public List<Transform> l_SpawnPoint;

    private void Awake()
    {
        if (s_Singleton != null)
        {
            Destroy(gameObject);
        }
        else if (s_Singleton == null)
        {
            s_Singleton = this;
        }
        SetupPlayersManager();
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(b_IsOkToRandom)
        {
            ZoneTimer -= Time.deltaTime;

            if(ZoneTimer <= 0)
            {
                GroundHeal();
                b_IsOkToRandom = false;
            }
        }
	}

    public void GroundHeal()
    {
        Transform spawnPointTransform = l_SpawnPoint[Random.Range(0, l_SpawnPoint.Count)];
        foreach (Player_Manager player in PlayersList)
        {
            player.Heal();
        }
        Instantiate(g_ZoneSafePrefab, spawnPointTransform.position, Quaternion.identity);
    }

    public void SetupHealZoneCountdown()
    {
        ZoneTimer = Random.Range(0f, l_CoolDown.Count);
        b_IsOkToRandom = true;
    }

    public void SetupPlayersManager()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < Players.Length; i++)
        {
            PlayersList.Add(Players[i].GetComponent<Player_Manager>());
        }
    }

    public float GetGenericMaxHealth()
    {
        return MaxHealth;
    }

    public float GetGenericHealMultiplier()
    {
        return HealMultiplier;
    }

    public float GetGenericDamageMultiplier()
    {
        return DamageMultiplier;
    }

}
