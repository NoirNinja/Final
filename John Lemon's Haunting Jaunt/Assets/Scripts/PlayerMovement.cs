using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();
        currentStamina = maxStamina;

        if (sprintIcon != null)
            sprintIcon.enabled = false;

    }

    void FixedUpdate()
    {

        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);


        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    void OnAnimatorMove()
    {
        if (isSprinting)
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude * 3f);
        }
        else
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        }
        m_Rigidbody.MoveRotation(m_Rotation);
    }


    public float maxStamina = 5f;
    public float staminaRechargeRate = 2f;
    public float staminaDrainRate = 1f;

    public UnityEngine.UI.Image sprintIcon;

    private float currentStamina;
    private bool isSprinting = false;
    private bool canSprint = true;
    private float moveSpeed;

    private Rigidbody rb;
    private Vector3 moveDirection;


    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);
        bool moving = moveDirection.magnitude > 0;

        if (shiftHeld && moving && canSprint)
        {
            isSprinting = true;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0);
            if (currentStamina == 0)
            {
                canSprint = false;
            }
        }
        else
        {
            isSprinting = false;

            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRechargeRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);
                if (currentStamina == maxStamina)
                {
                    canSprint = true;
                }
            }
        }
        if (sprintIcon != null)
            sprintIcon.enabled = isSprinting;
    }

    public float GetStaminaPercent()
    {
        return currentStamina / maxStamina;
    }

}