using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerCharacterController : MonoBehaviour
{

    public float moveSpeed;
    private Rigidbody myRigidbody;
    private Player_Manager PlyrManager;
    private Animator a_myAnimator;

    public string moveHorizontal, moveVertical;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Vector3 playerDirection;

    public Player_Stun_Zone g_StunZone;
    public float f_DelayToStun = 2.0f;

    [Header("Dash")]

    public float f_DashSpeed;
    private float f_DashTime;
    public float f_DashForce;
    private bool b_IsOkToDash;



    // Use this for initialization
    void Start()
    {
        PlyrManager = GetComponent<Player_Manager>();
        myRigidbody = GetComponent<Rigidbody>();
        a_myAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector3(Input.GetAxisRaw(moveHorizontal), 0f, Input.GetAxisRaw(moveVertical));
        moveVelocity = moveInput * moveSpeed;
        if (moveVelocity == Vector3.zero)
        {
            PlyrManager.PlayerIsNotMoving();
            a_myAnimator.SetBool("InPursuit", false);
        }
        else if (moveVelocity != Vector3.zero)
        {
            a_myAnimator.SetBool("InPursuit", true);
            PlyrManager.PlayerIsMoving();
        }

        playerDirection = Vector3.right * Input.GetAxisRaw(moveHorizontal) + Vector3.forward * Input.GetAxisRaw(moveVertical);
        if (playerDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);

        if(Input.GetKeyDown("a"))
            {
                g_StunZone.gameObject.SetActive(true);
                List<PlayerCharacterController> players = g_StunZone.GetPlayerInTrigger();
                foreach(PlayerCharacterController stun in players)
                {
                    stun.SetPlayerSpeed();
                }
            }
        }

        if (Input.GetKeyDown("e"))
        {
            if (b_IsOkToDash)
            {
                myRigidbody.AddForce(transform.forward * f_DashForce);
                g_StunZone.gameObject.SetActive(true);

                List<PlayerCharacterController> players = g_StunZone.GetPlayerInTrigger();

                foreach (PlayerCharacterController push in players)
                {
                    push.DashPlayerEffect();
                }
                b_IsOkToDash = false;
            }
        }
    }

    public void DashPlayerEffect()
    {
        
    }

    private void FixedUpdate()
    {
        myRigidbody.velocity = moveVelocity;
    }

    public void SetPlayerSpeed()
    {
        moveSpeed = 0f;
        StartCoroutine(Patobeur());
    }

    IEnumerator Patobeur()
    {
        yield return new WaitForSeconds(f_DelayToStun);
    }

}
