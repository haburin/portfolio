using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    // �v���C���[�̌������
    [SerializeField] private Transform orientation;

    // �ǂƂ̋�����W�����v�\�ȍ����̐ݒ�
    [SerializeField] private float wallDistance = .5f; // �ǂƂ̍ŏ�����
    [SerializeField] private float minimumJumpHeight = 1.5f; // �Ǒ���\�ȍŏ��W�����v����

    // �Ǒ��蒆�̋����Ɋւ���ݒ�
    [SerializeField] private float wallRunGravity; // �Ǒ��蒆�̏d��
    [SerializeField] private float wallRunJumpForce; // �Ǒ��蒆�̃W�����v��

    // �J�����̐ݒ�i����p��X���̒����j
    [SerializeField] private Camera cam;
    [SerializeField] private float fov; // �ʏ펞�̎���p
    [SerializeField] private float wallRunfov; // �Ǒ��蒆�̎���p
    [SerializeField] private float wallRunfovTime; // ����p�ύX�ɂ����鎞��
    [SerializeField] private float camTilt; // �J�����̌X���p�x
    [SerializeField] private float camTiltTime; // �J�����X���̕ύX���x

    // �J�����̌X���p�x���O������擾�\��
    public float tilt { get; private set; }

    // �ǂ̍��E����
    private bool wallLeft = false; // �ǂ����ɂ��邩
    private bool wallRight = false; // �ǂ��E�ɂ��邩

    // ���C�L���X�g�̃q�b�g���i���ƉE�̕ǁj
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    // Rigidbody�i���������j�ւ̎Q��
    private Rigidbody rb;

    // �Ǒ���\�����肷��i�v���C���[�����ȏ�̍����ɂ���ꍇ�̂݉\�j
    bool CanWallRun()
    {
        // �������Ƀ��C�L���X�g���A�n�ʂ܂ł̋������`�F�b�N
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    private void Start()
    {
        // Rigidbody �R���|�[�l���g���擾
        rb = GetComponent<Rigidbody>();
    }

    // �ǂ̍��E�̑��݂��`�F�b�N����
    void CheckWall()
    {
        // ���E�����Ƀ��C�L���X�g���΂��ĕǂ����m
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    private void Update()
    {
        // �ǂ̌��m�����s
        CheckWall();

        // �Ǒ��肪�\�ȏ����𖞂����Ă��邩����
        if (CanWallRun())
        {
            if (wallLeft)
            {
                StartWallRun(); // �����̕ǂŕǑ�����J�n
                UnityEngine.Debug.Log("wall running on the left"); // �����̕Ǒ���̃f�o�b�O���O
            }
            else if (wallRight)
            {
                StartWallRun(); // �E���̕ǂŕǑ�����J�n
                UnityEngine.Debug.Log("wall running on the right"); // �E���̕Ǒ���̃f�o�b�O���O
            }
            else
            {
                StopWallRun(); // �Ǒ�����~
            }
        }
        else
        {
            StopWallRun(); // �Ǒ�����~
        }
    }

    // �Ǒ�����J�n���鏈��
    void StartWallRun()
    {
        // �d�͂𖳌���
        rb.useGravity = false;

        // �Ǒ��蒆�̓v���C���[���������Ɉ�������͂�������
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        // ����p��Ǒ��蒆�̒l�ɕύX
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

        // �J�����̌X�������E�̕ǂɉ����Đݒ�
        if (wallLeft)
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if (wallRight)
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);

        // �X�y�[�X�L�[�ŕǑ��蒆�ɃW�����v���鏈��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                // ���Ǒ��蒆�̃W�����v�������v�Z
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // �������x�����Z�b�g
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force); // �W�����v�͂�������
            }
            else if (wallRight)
            {
                // �E�Ǒ��蒆�̃W�����v�������v�Z
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // �������x�����Z�b�g
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force); // �W�����v�͂�������
            }
        }
    }

    // �Ǒ�����~���鏈��
    void StopWallRun()
    {
        // �d�͂�L����
        rb.useGravity = true;

        // ����p��ʏ펞�̒l�ɖ߂�
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);

        // �J�����̌X�������Z�b�g
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
