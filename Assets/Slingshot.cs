using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [SerializeField] LineRenderer[] lineRenderers;//새총의 고무줄
    [SerializeField] Transform[] stripPositions; //고무줄의 양 끝 점
    [SerializeField] Transform center; // 새총의 중심
    [SerializeField] Transform idlePosition; // 새총의 고무줄의 원래 위치

    [SerializeField] Vector3 currentPosition; //고무줄이 당겨지는 위치

    [SerializeField] float maxLength; // 당겨질 수 있는 고무줄의 최대길이

    [SerializeField] float bottomBoundary; // 내려갈 수 있는 고무줄의 최소 높이
    bool isMouseDonw;

    private void Start()
    {
        // 고무줄 연결하기
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

            // 월드 좌표로 변환하여 현재 당겨지는 고무줄 위치를 저장
            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // 고무줄을 중심에서 최대길이만큼 만 당길 수 있게하기
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition
                - center.position, maxLength);

            // 고무줄이 너무 아래로 가지못하게 제한
            currentPosition = ClampBoundary(currentPosition);
            // 고무줄 끝위치를 당긴 위치로 이동시키기
            SetStrips(currentPosition);
        }
        else
        {
            // 고무줄을 원래위치로
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
        //고무줄을 원래 위치로 이동시키기
        currentPosition = idlePosition.position;
        SetStrips(currentPosition);
    }
    private void SetStrips(Vector3 position)
    {
        //고무줄이 끝나는 위치
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);
    }

    private Vector3 ClampBoundary(Vector3 vector)
    {
        //고무줄이 일정 위치로 내려가지 못하게 제한하기
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
            return vector;
    }
}
