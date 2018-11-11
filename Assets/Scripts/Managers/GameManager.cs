using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public GameObject ZoneSafePrefab;
    public GameObject HealZone;
    public List<Transform> l_SpawnPointBase;
    public GameObject ParticleZone;
    public Text WinText;
    public GameObject WinMenu;

    private bool b_IsInInvertMode = false;

    private Transform spawnPointTransform = null;

    [Header("Timer")]

    public Text t_Timer;
    public float f_MaxTimer;
    private float f_currentTimer;

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
        Time.timeScale = 1;
        SetupPlayersManager();
    }

    // Use this for initialization
    void Start()
    {
        SetupHealZoneCountdown();
        f_currentTimer = f_MaxTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (b_IsOkToRandom)
        {
            ZoneTimer -= Time.deltaTime;

            if (ZoneTimer <= 0)
            {
                GroundHeal();
                b_IsOkToRandom = false;
            }
        }

        f_currentTimer -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(f_currentTimer / 60f);
        int seconds = Mathf.FloorToInt(f_currentTimer - minutes * 60f);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        t_Timer.text = niceTime.ToString();

        if (f_currentTimer <= 0)
        {
            f_currentTimer = 0;
            GameOver();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        float MinHP = 100f;
        string PlayerName = "none";
        foreach (Player_Manager player in PlayersList)
        {
            player.Endgame();
            if (player.GetPlayerHp() < MinHP)
            {
                PlayerName = player.GetPlayerName();
                MinHP = player.GetPlayerHp();
            }
        }
        WinMenu.SetActive(true);
        WinText.text = PlayerName + " Wins !";
    }

    public void GroundHeal()
    {
        spawnPointTransform = l_SpawnPointBase[Random.Range(0, l_SpawnPointBase.Count)];
        l_SpawnPointBase.Remove(spawnPointTransform);
        foreach (Player_Manager player in PlayersList)
        {
            player.HealZone();
            b_IsInInvertMode = true;
        }
        ParticleZone.SetActive(true);
        Instantiate(ZoneSafePrefab, spawnPointTransform.position, Quaternion.identity);
    }

    public void SetupHealZoneCountdown()
    {
        if (l_SpawnPointBase.Count > 0)
        {
            ZoneTimer = l_CoolDown[Random.Range(0, l_CoolDown.Count)];
            b_IsOkToRandom = true;
        }
    }

    public void SetupPlayersManager()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < Players.Length; i++)
        {
            PlayersList.Add(Players[i].GetComponent<Player_Manager>());
        }
    }

    public void UpdateTimer()
    {
        f_currentTimer -= Time.deltaTime;
        t_Timer.text = f_currentTimer.ToString();
        if (f_currentTimer <= 0)
        {
            GameOver();
        }
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
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

    public void SetupCountDownForNewZone()
    {
        b_IsInInvertMode = false;
        ParticleZone.SetActive(false);
        foreach (Player_Manager player in PlayersList)
        {
            player.NoHealZone();
        }
        Instantiate(HealZone, spawnPointTransform.position, Quaternion.identity);
        SetupHealZoneCountdown();
    }

    public bool IsInvertMode()
    {
        return b_IsInInvertMode;
    }

}
