using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [SerializeField] LineRenderer[] lineRenderers;//������ ����
    [SerializeField] Transform[] stripPositions; //������ �� �� ��
    [SerializeField] Transform center; // ������ �߽�
    [SerializeField] Transform idlePosition; // ������ ������ ���� ��ġ

    [SerializeField] Vector3 currentPosition; //������ ������� ��ġ

    [SerializeField] float maxLength; // ����� �� �ִ� ������ �ִ����

    [SerializeField] float bottomBoundary; // ������ �� �ִ� ������ �ּ� ����
    bool isMouseDonw;

    // �� �߻�
    [SerializeField] GameObject birdPrefab; // �� ������
    public float birdPositionOffset;        // ���� ���� ���� �Ÿ�
    public float pushingForce;              // ���� �߻��ϴ� ��
    [SerializeField] Rigidbody2D bird;
    [SerializeField] Collider2D birdCollider;
    private void Start()
    {
        // ���� �����ϱ�
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);

        CreateBird();
    }

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
        }
        else
        {
            // ������ ������ġ��
            ResetStrips();
        }
    }
    private void CreateBird()
    {
        // ���� �����ϱ�
        bird = Instantiate(birdPrefab).GetComponent<Rigidbody2D>();
        // ���� �߻��غ��߿��� �浹���� �ʰ��ϱ�
        birdCollider = bird.GetComponent<Collider2D>();
        birdCollider.enabled = false;
        // �߷°� ���� ���⵵ �����ʰ� �ϱ�
        bird.isKinematic = true;

        ResetStrips();
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
        bird.isKinematic = false;
        // ������ ��� ��ŭ ������ ���� �����ش�.
        Vector3 birdForce = (currentPosition - center.position) * pushingForce * -1;
        bird.velocity = birdForce;

        bird = null;
        birdCollider = null;
        // 1�ʵ� ���ο� �� ����
        yield return new WaitForSeconds(1f);
        CreateBird();
    }
    private void Shoot() // ���� �߻��ϱ�
    {
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

        if (bird)
        {
            // ���� ��ġ�� ���� �����ϱ�
            Vector3 dir = position - center.position;
            bird.transform.position = position + dir.normalized * birdPositionOffset;
            bird.transform.right = -dir.normalized;
        }
       
    }

    private Vector3 ClampBoundary(Vector3 vector)
    {
        //������ ���� ���̷� �������� ���ϰ� �����ϱ�
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
            return vector;
    }
}
