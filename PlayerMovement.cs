using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // プレイヤーの高さ（傾斜検知に使用）
    float playerHeight = 2f;

    // プレイヤーの向きを基準に移動方向を決定
    [SerializeField] Transform orientation;

    // プレイヤーの移動速度と空中時の移動倍率
    [SerializeField] public float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    // 通常速度、スプリント速度、加速速度
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    // ジャンプ時の力
    public float jumpForce = 5f;

    // ジャンプとスプリントのキー設定
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    // 接地時と空中時のドラッグ設定
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    // 入力値（移動方向）を格納
    float horizontalMovement;
    float verticalMovement;

    // 地面検知用
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; } // 接地状態を公開

    // 移動方向と傾斜に応じた移動方向
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    // Rigidbodyの参照
    Rigidbody rb;

    // 傾斜を検知するための情報
    RaycastHit slopeHit;

    // プレイヤーが傾斜にいるかを判定
    private bool OnSlope()
    {
        // レイキャストで地面の法線を取得
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            // 地面が水平でなければ傾斜上にいると判定
            return slopeHit.normal != Vector3.up;
        }
        return false; // 傾斜ではない場合
    }

    private void Start()
    {
        // Rigidbodyの参照を取得し、回転を固定
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // 地面との接触を判定
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // 入力処理と速度・ドラッグの制御
        MyInput();
        ControlDrag();
        ControlSpeed();

        // ジャンプ処理
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        // 傾斜上の移動方向を計算
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    // 入力値の取得と移動方向の計算
    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    // ジャンプ処理
    void Jump()
    {
        if (isGrounded)
        {
            // ジャンプ時に垂直速度をリセット
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            // ジャンプ力を加える
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    // 移動速度を制御
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

    // ドラッグを制御
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
        // 移動処理を実行
        MovePlayer();
    }

    // プレイヤーの移動処理
    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            // 平地上での移動
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            // 傾斜上での移動
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            // 空中での移動
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
}
