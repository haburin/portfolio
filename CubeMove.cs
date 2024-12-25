using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �V�[���Ǘ��ɕK�v
using UnityEngine.UI; // UI�v�f�̑���ɕK�v

public class CubeMove : MonoBehaviour
{
    public float speed; // �L���[�u�̈ړ����x
    private Rigidbody rb; // �L���[�u�̕�������
    public float time; // ���݂̃^�C��
    public float bestTime; // �x�X�g�^�C��
    public Text TimerText; // ���݂̃^�C����\������e�L�X�g
    public Text BestTimeText; // �x�X�g�^�C����\������e�L�X�g
    private bool isClear; // �Q�[���N���A��Ԃ𔻒�

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody���擾
        time = 0.0f; // �^�C����������
        speed = 0.1f; // �����̈ړ����x��ݒ�
        isClear = false; // ������Ԃ̓N���A���Ă��Ȃ�
        bestTime = PlayerPrefs.GetFloat("TIMER", 100); // �x�X�g�^�C�����擾�A���ݒ莞��100���f�t�H���g�l�Ƃ���
    }

    void Update()
    {
        if (!isClear) // �Q�[���N���A���Ă��Ȃ��ꍇ
        {
            time += Time.deltaTime; // �o�ߎ��Ԃ����Z
            TimerText.text = time.ToString("F2"); // ���݂̃^�C����UI�ɔ��f
        }

        // �x�X�g�^�C���̕\���X�V
        if (bestTime < 100) // �x�X�g�^�C�����ݒ肳��Ă���ꍇ
        {
            BestTimeText.text = "�x�X�g�^�C�� " + bestTime.ToString("F2");
        }
        else // �x�X�g�^�C�����ݒ�̏ꍇ
        {
            BestTimeText.text = "�x�X�g�^�C�� ";
        }

        // ���L�[�ŃL���[�u���ړ�
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
        if (other.gameObject.name == "Sphere") // �uSphere�v�ɐڐG�����ꍇ
        {
            isClear = true; // �Q�[���N���A��Ԃɐݒ�
            other.gameObject.SetActive(false); // �uSphere�v���\���ɂ���
            Judgement(); // �x�X�g�^�C�����X�V���邩����
            SceneManager.LoadScene("clear"); // �N���A��ʂɈړ�
        }
    }

    // �x�X�g�^�C�����X�V���邩�𔻒肷��֐�
    void Judgement()
    {
        if (bestTime > time) // ���݂̃^�C�����x�X�g�^�C�����Z���ꍇ
        {
            bestTime = time; // �x�X�g�^�C�����X�V
            PlayerPrefs.SetFloat("TIMER", bestTime); // �x�X�g�^�C����ۑ�
            PlayerPrefs.Save(); // PlayerPrefs��ۑ�
        }
    }
}
