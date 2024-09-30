using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    //[SerializeField] AudioClip pullSound;
    [SerializeField] LineRenderer[] lineRenderers;//������ ����
    [SerializeField] Transform[] stripPositions; //������ �� �� ��
    [SerializeField] Transform center; // ������ �߽�
    [SerializeField] Transform idlePosition; // ������ ������ ���� ��ġ

    [SerializeField] Vector3 currentPosition; //������ ������� ��ġ

    [SerializeField] float maxLength; // ����� �� �ִ� ������ �ִ����

    [SerializeField] float bottomBoundary; // ������ �� �ִ� ������ �ּ� ����
    bool isMouseDonw;
    private AudioSource audioSource;

    [Header("BirdShot")]// �� �߻�
    [SerializeField] GameObject[] birdPrefab; // �� ������
    public float birdPositionOffset;        // ���� ���� ���� �Ÿ�
    public float pushingForce;              // ���� �߻��ϴ� ��
    [SerializeField] Rigidbody2D birdRigid;
    [SerializeField] Collider2D birdCollider;
    private Bird currentBird;
    private int currentBirdIndex = 0;
    private bool isBirdInAir; // ���� ���߿� �ִ��� ����
    private void Start()
    {
        // ���� �����ϱ�
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);

        CreateBird();
    }

    private bool SlingshotSound = false;
    private void Update()
    {
        if (isMouseDonw)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            // ���� ��ǥ�� ��ȯ�Ͽ� ���� ������� ���� ��ġ�� ����
            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // ������ �߽ɿ��� �ִ���̸�ŭ �� ��� �� �ְ��ϱ�
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition
                - center.position, maxLength);

            // ������ �ʹ� �Ʒ��� �������ϰ� ����
            currentPosition = ClampBoundary(currentPosition);
            // ���� ����ġ�� ��� ��ġ�� �̵���Ű��
            SetStrips(currentPosition);

            if (birdCollider)
            {
                birdCollider.enabled = true;
            }
            if (!SlingshotSound)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Slingshot);
                SlingshotSound = true;
            }
        }
        else
        {
            // ������ ������ġ��
            ResetStrips();
            SlingshotSound = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentBirdIndex = 0;
            SwitchBird();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentBirdIndex = 1;
            SwitchBird();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentBirdIndex = 2;
            SwitchBird();
        }
        if (Input.GetMouseButtonDown(0) && isBirdInAir && currentBird != null)
        {
            currentBird.UseSkill();
        }
    }
    private void CreateBird()
    {
        // ���� �����ϱ�
        birdRigid = Instantiate(birdPrefab[currentBirdIndex]).GetComponent<Rigidbody2D>();
        // ���� �߻��غ��߿��� �浹���� �ʰ��ϱ�
        birdCollider = birdRigid.GetComponent<Collider2D>();
        birdCollider.enabled = false;
        // �߷°� ���� ���⵵ �����ʰ� �ϱ�
        birdRigid.isKinematic = true;

        currentBird = birdRigid.GetComponent<Bird>();
        ResetStrips();
    }
    private void SwitchBird()
    {
        // ���� ���� �ı��ϰ� ���ο� ���� ����
        if (birdRigid != null)
        {
            Destroy(birdRigid.gameObject);
        }
        CreateBird();
    }
    private void OnMouseDown()
    {
        isMouseDonw = true;
    }
    private void OnMouseUp()
    {
        isMouseDonw = false;
        Shoot();
    }
    private IEnumerator ShootRroutine()
    {
        birdRigid.isKinematic = false;
        // ������ ��� ��ŭ ������ ���� �����ش�.
        Vector3 birdForce = (currentPosition - center.position) * pushingForce * -1;
        birdRigid.velocity = birdForce;
        isBirdInAir = true;

        birdRigid = null;
        birdCollider = null;
        // 1�ʵ� ���ο� �� ����
        yield return new WaitForSeconds(1f);
        CreateBird();
    }
    private void Shoot() // ���� �߻��ϱ�
    {
        if (currentBirdIndex == 0) // ������
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.FlyRed);
        }
        else if (currentBirdIndex == 1) // �����
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.FlyYellow);
        }
        else if (currentBirdIndex == 2) // ������
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.FlyBlack); // �ʿ� ��
        }
        StartCoroutine(ShootRroutine());
    }
     
    private void ResetStrips()
    {
        //������ ���� ��ġ�� �̵���Ű��
        currentPosition = idlePosition.position;
        SetStrips(currentPosition);
    }
    private void SetStrips(Vector3 position)
    {
        //������ ������ ��ġ
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (birdRigid)
        {
            // ���� ��ġ�� ���� �����ϱ�
            Vector3 dir = position - center.position;
            birdRigid.transform.position = position + dir.normalized * birdPositionOffset;
            birdRigid.transform.right = -dir.normalized;
        }
       
    }

    private Vector3 ClampBoundary(Vector3 vector)
    {
        //������ ���� ���̷� �������� ���ϰ� �����ϱ�
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
            return vector;
    }
}
