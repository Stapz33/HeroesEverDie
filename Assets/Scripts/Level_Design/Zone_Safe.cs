using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone_Safe : MonoBehaviour
{ 
    public float f_TimeLeft = 5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        f_TimeLeft -= Time.deltaTime;
        if(f_TimeLeft <= 0f)
        {
            GameManager.s_Singleton.SetupCountDownForNewZone();
            Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Player_Manager>().NoHealZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Player_Manager>().HealZone();
        }
    }
}
