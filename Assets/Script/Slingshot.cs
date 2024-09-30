using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    //[SerializeField] AudioClip pullSound;
    [SerializeField] LineRenderer[] lineRenderers;//새총의 고무줄
    [SerializeField] Transform[] stripPositions; //고무줄의 양 끝 점
    [SerializeField] Transform center; // 새총의 중심
    [SerializeField] Transform idlePosition; // 새총의 고무줄의 원래 위치

    [SerializeField] Vector3 currentPosition; //고무줄이 당겨지는 위치

    [SerializeField] float maxLength; // 당겨질 수 있는 고무줄의 최대길이

    [SerializeField] float bottomBoundary; // 내려갈 수 있는 고무줄의 최소 높이
    bool isMouseDonw;
    private AudioSource audioSource;

    [Header("BirdShot")]// 새 발사
    [SerializeField] GameObject[] birdPrefab; // 새 프리팹
    public float birdPositionOffset;        // 새와 고무줄 사이 거리
    public float pushingForce;              // 새를 발사하는 힘
    [SerializeField] Rigidbody2D birdRigid;
    [SerializeField] Collider2D birdCollider;
    private Bird currentBird;
    private int currentBirdIndex = 0;
    private bool isBirdInAir; // 새가 공중에 있는지 여부
    private void Start()
    {
        // 고무줄 연결하기
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

            // 월드 좌표로 변환하여 현재 당겨지는 고무줄 위치를 저장
            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // 고무줄을 중심에서 최대길이만큼 만 당길 수 있게하기
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition
                - center.position, maxLength);

            // 고무줄이 너무 아래로 가지못하게 제한
            currentPosition = ClampBoundary(currentPosition);
            // 고무줄 끝위치를 당긴 위치로 이동시키기
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
            // 고무줄을 원래위치로
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
        // 새를 생성하기
        birdRigid = Instantiate(birdPrefab[currentBirdIndex]).GetComponent<Rigidbody2D>();
        // 새가 발사준비중에는 충돌하지 않게하기
        birdCollider = birdRigid.GetComponent<Collider2D>();
        birdCollider.enabled = false;
        // 중력과 힘의 영향도 받지않게 하기
        birdRigid.isKinematic = true;

        currentBird = birdRigid.GetComponent<Bird>();
        ResetStrips();
    }
    private void SwitchBird()
    {
        // 현재 새를 파괴하고 새로운 새를 생성
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
        // 고무줄을 당긴 만큼 새에게 힘을 가해준다.
        Vector3 birdForce = (currentPosition - center.position) * pushingForce * -1;
        birdRigid.velocity = birdForce;
        isBirdInAir = true;

        birdRigid = null;
        birdCollider = null;
        // 1초뒤 새로운 새 생성
        yield return new WaitForSeconds(1f);
        CreateBird();
    }
    private void Shoot() // 새를 발사하기
    {
        if (currentBirdIndex == 0) // 빨간새
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.FlyRed);
        }
        else if (currentBirdIndex == 1) // 노란새
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.FlyYellow);
        }
        else if (currentBirdIndex == 2) // 검은새
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.FlyBlack); // 필요 시
        }
        StartCoroutine(ShootRroutine());
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

        if (birdRigid)
        {
            // 새의 위치와 방향 설정하기
            Vector3 dir = position - center.position;
            birdRigid.transform.position = position + dir.normalized * birdPositionOffset;
            birdRigid.transform.right = -dir.normalized;
        }
       
    }

    private Vector3 ClampBoundary(Vector3 vector)
    {
        //고무줄이 일정 높이로 내려가지 못하게 제한하기
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
            return vector;
    }
}
