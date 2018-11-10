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
        if(f_TimeLeft - Time.deltaTime == 0f)
        {
            GameManager.s_Singleton.SetupHealZoneCountdown();
            Destroy(this);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Player_Manager>().ZoneSafe();
        }
    }
}
