using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerCharacterController : MonoBehaviour
{

    public float moveSpeed;
    private float BaseMoveSpeed;
    private Rigidbody myRigidbody;
    private Player_Manager PlyrManager;
    private Animator a_myAnimator;
    public GameObject PlayerStunZone;
    public Transform StunZoneTransform;

    public string moveHorizontal, moveVertical,AButton, BButton;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Vector3 playerDirection;

    public Player_Stun_Zone g_StunZone;
    public float f_DelayToStun = 2.0f;

    [Header("Dash")]

    public float f_DashCountDown = 4.0f;
    public float f_DashForce;
    private bool b_IsOkToDash = true;
    private float f_ImpulseSave;
    private bool b_CanUseStun = true;
    public GameObject g_ZoneDashPrefab;



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
        if (!b_IsOkToDash)
        {
            if(f_DashCountDown - Time.deltaTime <= 0)
            {
                b_IsOkToDash = true;
            }
        }


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

            if (Input.GetButtonDown(AButton))
            {
                if (b_IsOkToDash)
                {
                    myRigidbody.AddForce(transform.forward * f_DashForce,ForceMode.Impulse);
                    g_ZoneDashPrefab.SetActive(true);
                }
            }
        }

        if (Input.GetButtonDown(BButton) && b_CanUseStun)
        {
            Instantiate(PlayerStunZone, StunZoneTransform.position, StunZoneTransform.rotation);
            b_CanUseStun = false;
            StartCoroutine(StunCooldown());
        }
    }

    public void StopDash()
    {
        myRigidbody.velocity = new Vector3(0, 0, 0); 
    }

    public void ExpulsePlayer(Vector3 impulseVector)
    {
       myRigidbody.AddForce(impulseVector * f_DashForce, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        myRigidbody.velocity = moveVelocity;
    }

    public void StunPlayer()
    {
        BaseMoveSpeed = moveSpeed;
        moveSpeed = 0f;
        StartCoroutine(Patobeur());
    }

    IEnumerator Patobeur()
    {
        yield return new WaitForSeconds(2.5f);
        moveSpeed = BaseMoveSpeed;
    }

    public IEnumerator StunCooldown()
    {
        yield return new WaitForSeconds(4);
        b_CanUseStun = true;
    }

}
