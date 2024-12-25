using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理に必要
using UnityEngine.UI; // UI要素の操作に必要

public class CubeMove : MonoBehaviour
{
    public float speed; // キューブの移動速度
    private Rigidbody rb; // キューブの物理挙動
    public float time; // 現在のタイム
    public float bestTime; // ベストタイム
    public Text TimerText; // 現在のタイムを表示するテキスト
    public Text BestTimeText; // ベストタイムを表示するテキスト
    private bool isClear; // ゲームクリア状態を判定

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
        time = 0.0f; // タイムを初期化
        speed = 0.1f; // 初期の移動速度を設定
        isClear = false; // 初期状態はクリアしていない
        bestTime = PlayerPrefs.GetFloat("TIMER", 100); // ベストタイムを取得、未設定時は100をデフォルト値とする
    }

    void Update()
    {
        if (!isClear) // ゲームクリアしていない場合
        {
            time += Time.deltaTime; // 経過時間を加算
            TimerText.text = time.ToString("F2"); // 現在のタイムをUIに反映
        }

        // ベストタイムの表示更新
        if (bestTime < 100) // ベストタイムが設定されている場合
        {
            BestTimeText.text = "ベストタイム " + bestTime.ToString("F2");
        }
        else // ベストタイム未設定の場合
        {
            BestTimeText.text = "ベストタイム ";
        }

        // 矢印キーでキューブを移動
        if (Input.GetKey(KeyCode.UpArrow)) 
        {
            transform.Translate(0f, 0f, speed);
        }
        if (Input.GetKey(KeyCode.DownArrow)) 
        {
            transform.Translate(0f, 0f, speed * -1);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            transform.Translate(speed * -1, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.RightArrow)) 
        {
            transform.Translate(speed, 0f, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Sphere") // 「Sphere」に接触した場合
        {
            isClear = true; // ゲームクリア状態に設定
            other.gameObject.SetActive(false); // 「Sphere」を非表示にする
            Judgement(); // ベストタイムを更新するか判定
            SceneManager.LoadScene("clear"); // クリア画面に移動
        }
    }

    // ベストタイムを更新するかを判定する関数
    void Judgement()
    {
        if (bestTime > time) // 現在のタイムがベストタイムより短い場合
        {
            bestTime = time; // ベストタイムを更新
            PlayerPrefs.SetFloat("TIMER", bestTime); // ベストタイムを保存
            PlayerPrefs.Save(); // PlayerPrefsを保存
        }
    }
}
