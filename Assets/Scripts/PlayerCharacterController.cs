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
    private Vector3 ImpulseVector;

    public Player_Stun_Zone g_StunZone;
    public float f_DelayToStun = 2.0f;

    [Header("Dash")]

    public float f_DashCountDown = 4.0f;
    public float f_DashForce;
    private bool b_IsKnockedBack = false;
    private bool b_CanDash = true;
    private bool b_IsOkToDash = false;
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
            if (!b_IsOkToDash && !b_IsKnockedBack)
            transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);

            if (Input.GetButton(AButton) && b_CanDash)
            {
                b_IsOkToDash = true;
                b_CanDash = false;
                StartCoroutine(DashCD());
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

    public IEnumerator DashCD()
    {
        yield return new WaitForSeconds(0.12f);
        b_IsOkToDash = false;
        yield return new WaitForSeconds(1f);
        b_CanDash = true;
    }

    public void ExpulsePlayer(Vector3 impulseVector)
    {
        b_IsKnockedBack = true;
        ImpulseVector = impulseVector;
        StartCoroutine(KnockBackCD());
    }

    public IEnumerator KnockBackCD()
    {
        yield return new WaitForSeconds(0.12f);
        b_IsKnockedBack = false;
    }

    private void FixedUpdate()
    {
        if (b_IsKnockedBack)
        {
            myRigidbody.AddForce(ImpulseVector * f_DashForce * 3, ForceMode.Impulse);
        }
        if (b_IsOkToDash)
        {
            myRigidbody.AddForce(transform.forward * f_DashForce, ForceMode.Impulse);
            g_ZoneDashPrefab.SetActive(true);
        }
        else if (!b_IsOkToDash)
        {
            myRigidbody.velocity = moveVelocity;
        }
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
