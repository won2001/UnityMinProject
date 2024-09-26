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

    private void Start()
    {
        // ���� �����ϱ�
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);
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
        }
        else
        {
            // ������ ������ġ��
            ResetStrips();
        }
    }

    private void OnMouseDown()
    {
        isMouseDonw = true;
    }
    private void OnMouseUp()
    {
        isMouseDonw = false;
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
    }

    private Vector3 ClampBoundary(Vector3 vector)
    {
        //������ ���� ��ġ�� �������� ���ϰ� �����ϱ�
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
            return vector;
    }
}
