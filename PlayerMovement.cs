using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // �v���C���[�̍����i�X�Ό��m�Ɏg�p�j
    float playerHeight = 2f;

    // �v���C���[�̌�������Ɉړ�����������
    [SerializeField] Transform orientation;

    // �v���C���[�̈ړ����x�Ƌ󒆎��̈ړ��{��
    [SerializeField] public float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    // �ʏ푬�x�A�X�v�����g���x�A�������x
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    // �W�����v���̗�
    public float jumpForce = 5f;

    // �W�����v�ƃX�v�����g�̃L�[�ݒ�
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    // �ڒn���Ƌ󒆎��̃h���b�O�ݒ�
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    // ���͒l�i�ړ������j���i�[
    float horizontalMovement;
    float verticalMovement;

    // �n�ʌ��m�p
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; } // �ڒn��Ԃ����J

    // �ړ������ƌX�΂ɉ������ړ�����
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    // Rigidbody�̎Q��
    Rigidbody rb;

    // �X�΂����m���邽�߂̏��
    RaycastHit slopeHit;

    // �v���C���[���X�΂ɂ��邩�𔻒�
    private bool OnSlope()
    {
        // ���C�L���X�g�Œn�ʂ̖@�����擾
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            // �n�ʂ������łȂ���ΌX�Ώ�ɂ���Ɣ���
            return slopeHit.normal != Vector3.up;
        }
        return false; // �X�΂ł͂Ȃ��ꍇ
    }

    private void Start()
    {
        // Rigidbody�̎Q�Ƃ��擾���A��]���Œ�
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // �n�ʂƂ̐ڐG�𔻒�
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // ���͏����Ƒ��x�E�h���b�O�̐���
        MyInput();
        ControlDrag();
        ControlSpeed();

        // �W�����v����
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        // �X�Ώ�̈ړ��������v�Z
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    // ���͒l�̎擾�ƈړ������̌v�Z
    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    // �W�����v����
    void Jump()
    {
        if (isGrounded)
        {
            // �W�����v���ɐ������x�����Z�b�g
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            // �W�����v�͂�������
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    // �ړ����x�𐧌�
    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    // �h���b�O�𐧌�
    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        // �ړ����������s
        MovePlayer();
    }

    // �v���C���[�̈ړ�����
    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            // ���n��ł̈ړ�
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            // �X�Ώ�ł̈ړ�
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            // �󒆂ł̈ړ�
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
}
