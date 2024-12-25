using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    // プレイヤーの向き情報
    [SerializeField] private Transform orientation;

    // 壁との距離やジャンプ可能な高さの設定
    [SerializeField] private float wallDistance = .5f; // 壁との最小距離
    [SerializeField] private float minimumJumpHeight = 1.5f; // 壁走り可能な最小ジャンプ高さ

    // 壁走り中の挙動に関する設定
    [SerializeField] private float wallRunGravity; // 壁走り中の重力
    [SerializeField] private float wallRunJumpForce; // 壁走り中のジャンプ力

    // カメラの設定（視野角や傾きの調整）
    [SerializeField] private Camera cam;
    [SerializeField] private float fov; // 通常時の視野角
    [SerializeField] private float wallRunfov; // 壁走り中の視野角
    [SerializeField] private float wallRunfovTime; // 視野角変更にかかる時間
    [SerializeField] private float camTilt; // カメラの傾き角度
    [SerializeField] private float camTiltTime; // カメラ傾きの変更速度

    // カメラの傾き角度を外部から取得可能に
    public float tilt { get; private set; }

    // 壁の左右判定
    private bool wallLeft = false; // 壁が左にあるか
    private bool wallRight = false; // 壁が右にあるか

    // レイキャストのヒット情報（左と右の壁）
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    // Rigidbody（物理挙動）への参照
    private Rigidbody rb;

    // 壁走り可能か判定する（プレイヤーが一定以上の高さにいる場合のみ可能）
    bool CanWallRun()
    {
        // 下方向にレイキャストし、地面までの距離をチェック
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    private void Start()
    {
        // Rigidbody コンポーネントを取得
        rb = GetComponent<Rigidbody>();
    }

    // 壁の左右の存在をチェックする
    void CheckWall()
    {
        // 左右方向にレイキャストを飛ばして壁を検知
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    private void Update()
    {
        // 壁の検知を実行
        CheckWall();

        // 壁走りが可能な条件を満たしているか判定
        if (CanWallRun())
        {
            if (wallLeft)
            {
                StartWallRun(); // 左側の壁で壁走りを開始
                UnityEngine.Debug.Log("wall running on the left"); // 左側の壁走りのデバッグログ
            }
            else if (wallRight)
            {
                StartWallRun(); // 右側の壁で壁走りを開始
                UnityEngine.Debug.Log("wall running on the right"); // 右側の壁走りのデバッグログ
            }
            else
            {
                StopWallRun(); // 壁走りを停止
            }
        }
        else
        {
            StopWallRun(); // 壁走りを停止
        }
    }

    // 壁走りを開始する処理
    void StartWallRun()
    {
        // 重力を無効化
        rb.useGravity = false;

        // 壁走り中はプレイヤーを下方向に引っ張る力を加える
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        // 視野角を壁走り中の値に変更
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

        // カメラの傾きを左右の壁に応じて設定
        if (wallLeft)
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if (wallRight)
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);

        // スペースキーで壁走り中にジャンプする処理
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                // 左壁走り中のジャンプ方向を計算
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // 垂直速度をリセット
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force); // ジャンプ力を加える
            }
            else if (wallRight)
            {
                // 右壁走り中のジャンプ方向を計算
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // 垂直速度をリセット
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force); // ジャンプ力を加える
            }
        }
    }

    // 壁走りを停止する処理
    void StopWallRun()
    {
        // 重力を有効化
        rb.useGravity = true;

        // 視野角を通常時の値に戻す
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);

        // カメラの傾きをリセット
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
