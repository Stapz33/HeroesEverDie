using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour {

    public Player_Manager PlayerLife;
    private Image life;

    private void Start()
    {
        life = GetComponent<Image>();
    }

    private void Update()
    {
        life.fillAmount = PlayerLife.GetPlayerHp() / 100f;
    }
}
