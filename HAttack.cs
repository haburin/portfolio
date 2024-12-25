using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class HAttack : MonoBehaviour
{
    // ���j�b�g�̃^�C�v��ݒ肷�邽�߂̃t�B�[���h�i�f�t�H���g��Streamer�j
    [SerializeField]
    private CommonParam.UnitType _unitType = CommonParam.UnitType.Streamer;

    // ���͊Ǘ��N���X�̎Q��
    [SerializeField]
    InputManager _inputManager;

    // ���̓p�����[�^���i�[����ϐ�
    private InputManager.InputParam _inputParam;

    // �A�j���[�V�����R���|�[�l���g�ւ̎Q��
    private Animation _animation;

    // �U���񐔂��L�^����J�E���g�ϐ�
    private int count = 0;

    // �ҋ@��Ԃ𔻒肷��t���O
    private bool _stayjudgement = false;

    // �ҋ@���Ԃ�ݒ肷��ϐ�
    [SerializeField]
    public float _hastay;

    // Streamer�f�[�^���S���ւ̎Q��
    [SerializeField]
    CenterDataOfStreamer _centerDataOfStreamer;

    // �U������N���X�ւ̎Q��
    [SerializeField]
    private AttackHitDetection _attackHitDetection;

    // �U���̈З͂�ݒ肷��ϐ��i3�i�K�̋��x�j
    [SerializeField]
    float ghAttackPower1 = 0f;
    [SerializeField]
    float ghAttackPower2 = 0f;
    [SerializeField]
    float ghAttackPower3 = 0f;

    // �A�j���[�V�����̏�Ԃ��Ǘ�����t���O
    private bool _haAnimation = false;

    // ���͂̃I��/�I�t���Ǘ�����t���O
    [SerializeField]
    bool InputOn = false;

    // Start is called before the first frame update
    void Start()
    {
        // �K�v�ȃR���|�[�l���g���擾���鏈��
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

        // ���j�b�g�̓��̓p�����[�^��ݒ�
        _inputParam = _inputManager.UnitInputParams[_unitType];

        // �ҋ@�������J�n
        StartCoroutine(HAstay());
    }

    // ���t���[���X�V����
    private void Update()
    {
        // �U�����������s
        MHAttack();
        // �f�o�b�O���O�őҋ@��Ԃ��m�F
        Debug.Log(_stayjudgement);
    }

    // �d�U���̏���
    private void MHAttack()
    {
        if (_stayjudgement)
        {
            if (_inputParam.Select)
            {
                // �ҋ@��Ԃ�����
                _stayjudgement = false;

                // �A�j���[�V�������Đ�
                _animation.MGhHeavyAttackAnima();

                // �U�����q�b�g�����ꍇ�̏���
                if (_attackHitDetection.attackHitTheStreamer)
                {
                    // �_���[�W���������s
                    Damege();
                    count++;
                }

                // �J�E���g�����Z�b�g
                count = 0;
            }
        }
    }

    // �_���[�W���v�Z���ēK�p
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

    // �ҋ@�����̃R���[�`��
    IEnumerator HAstay()
    {
        // �w�莞�ԑҋ@
        yield return new WaitForSeconds(_hastay);

        // �f�o�b�O���O�őҋ@���Ԃ��m�F
        Debug.Log("stay�J�E���g��" + _hastay);

        // �ҋ@��Ԃ�L���ɂ���
        _stayjudgement = true;
    }
}
