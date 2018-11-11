using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stun_Zone : MonoBehaviour {

    private List<PlayerCharacterController> g_PlayerToStun = new List<PlayerCharacterController>();
    private GameObject StunFX;
    private GameObject SpriteUI;

	// Use this for initialization
	void Start () {
        StunFX = transform.GetChild(0).gameObject;
        SpriteUI = transform.GetChild(1).gameObject;
        StartCoroutine(CastStun());
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

    public void StunPlayers()
    {
        SpriteUI.SetActive(false);
        StunFX.SetActive(true);
        if (g_PlayerToStun.Count > 0)
        {
            foreach (PlayerCharacterController Player in g_PlayerToStun)
            {
                Player.StunPlayer();
            }
            StartCoroutine(Kill());
            
        }
    }

    public IEnumerator CastStun()
    {
        yield return new WaitForSeconds(0.5f);
        StunPlayers();
    }

    public IEnumerator Kill()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
