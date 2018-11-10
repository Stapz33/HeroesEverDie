using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stun_Zone : MonoBehaviour {

    private List<PlayerCharacterController> g_PlayerToStun;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            g_PlayerToStun.Add(other.GetComponent<PlayerCharacterController>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            g_PlayerToStun.Remove(other.GetComponent<PlayerCharacterController>());
        }
    }

    public List<PlayerCharacterController> GetPlayerInTrigger()
    {
        if (g_PlayerToStun != null)
        {
            return g_PlayerToStun;
        }
        return null;
    }
}
