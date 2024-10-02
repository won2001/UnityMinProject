using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : BaseObstacle
{
    protected override void Start()
    {
        // 나무 장애물의 특성 설정
        maxHealth = 10f;
        base.Start();    // 상위 클래스의 Start() 호출
    }
}
