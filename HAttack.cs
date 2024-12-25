using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class HAttack : MonoBehaviour
{
    // ユニットのタイプを設定するためのフィールド（デフォルトはStreamer）
    [SerializeField]
    private CommonParam.UnitType _unitType = CommonParam.UnitType.Streamer;

    // 入力管理クラスの参照
    [SerializeField]
    InputManager _inputManager;

    // 入力パラメータを格納する変数
    private InputManager.InputParam _inputParam;

    // アニメーションコンポーネントへの参照
    private Animation _animation;

    // 攻撃回数を記録するカウント変数
    private int count = 0;

    // 待機状態を判定するフラグ
    private bool _stayjudgement = false;

    // 待機時間を設定する変数
    [SerializeField]
    public float _hastay;

    // Streamerデータ中心情報への参照
    [SerializeField]
    CenterDataOfStreamer _centerDataOfStreamer;

    // 攻撃判定クラスへの参照
    [SerializeField]
    private AttackHitDetection _attackHitDetection;

    // 攻撃の威力を設定する変数（3段階の強度）
    [SerializeField]
    float ghAttackPower1 = 0f;
    [SerializeField]
    float ghAttackPower2 = 0f;
    [SerializeField]
    float ghAttackPower3 = 0f;

    // アニメーションの状態を管理するフラグ
    private bool _haAnimation = false;

    // 入力のオン/オフを管理するフラグ
    [SerializeField]
    bool InputOn = false;

    // Start is called before the first frame update
    void Start()
    {
        // 必要なコンポーネントを取得する処理
        if (_inputManager == null)
        {
            _inputManager = GetComponent<InputManager>();
        }
        if (_animation == null)
        {
            _animation = GetComponent<Animation>();
        }
        if (_centerDataOfStreamer == null)
        {
            _centerDataOfStreamer = GetComponent<CenterDataOfStreamer>();
        }

        // ユニットの入力パラメータを設定
        _inputParam = _inputManager.UnitInputParams[_unitType];

        // 待機処理を開始
        StartCoroutine(HAstay());
    }

    // 毎フレーム更新処理
    private void Update()
    {
        // 攻撃処理を実行
        MHAttack();
        // デバッグログで待機状態を確認
        Debug.Log(_stayjudgement);
    }

    // 重攻撃の処理
    private void MHAttack()
    {
        if (_stayjudgement)
        {
            if (_inputParam.Select)
            {
                // 待機状態を解除
                _stayjudgement = false;

                // アニメーションを再生
                _animation.MGhHeavyAttackAnima();

                // 攻撃がヒットした場合の処理
                if (_attackHitDetection.attackHitTheStreamer)
                {
                    // ダメージ処理を実行
                    Damege();
                    count++;
                }

                // カウントをリセット
                count = 0;
            }
        }
    }

    // ダメージを計算して適用
    void Damege()
    {
        if (count == 1)
        {
            _centerDataOfStreamer.stHp -= _attackHitDetection.ghAttackPower * ghAttackPower1;
        }
        else if (count == 2)
        {
            _centerDataOfStreamer.stHp -= _attackHitDetection.ghAttackPower * ghAttackPower2;
        }
        else if (count == 3)
        {
            _centerDataOfStreamer.stHp -= _attackHitDetection.ghAttackPower * ghAttackPower3;
        }
    }

    // 待機処理のコルーチン
    IEnumerator HAstay()
    {
        // 指定時間待機
        yield return new WaitForSeconds(_hastay);

        // デバッグログで待機時間を確認
        Debug.Log("stayカウントは" + _hastay);

        // 待機状態を有効にする
        _stayjudgement = true;
    }
}
