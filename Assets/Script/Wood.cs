using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : BaseObstacle
{
    protected override void Start()
    {
        // ���� ��ֹ��� Ư�� ����
        maxHealth = 10f;
        base.Start();    // ���� Ŭ������ Start() ȣ��
    }
}
