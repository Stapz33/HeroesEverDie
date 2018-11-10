using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_To_Push : MonoBehaviour
{
    private PlayerCharacterController myPlayer;

    // Use this for initialization
    void Start()
    {
        myPlayer = GetComponentInParent<PlayerCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            myPlayer.StopDash();
            other.GetComponent<PlayerCharacterController>().ExpulsePlayer(myPlayer.transform.forward);
        }
    }
}
