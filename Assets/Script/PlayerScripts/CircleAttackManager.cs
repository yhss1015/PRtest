using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttackManager : MonoBehaviour
{
    public GameObject circlePrefab; // 원형 공격 프리팹 (하나만 사용)
    public int maxCircles = 1; // 현재 활성화된 서클 개수 (최대 6개)
    public float radius = 2.7f; // 원이 도는 반지름
    public float rotationSpeed = 200f; // 회전 속도

    private List<GameObject> circles = new List<GameObject>();
    private float currentAngle = 0f; // 회전 각도
    private Coroutine rotationRoutine; // 현재 회전 코루틴

    public void StartCircleAttack()
    {
        if (rotationRoutine == null)
        {
            InitializeCircles();
            rotationRoutine = StartCoroutine(RotateCircles());
        }
    }

    public void StopCircleAttack()
    {
        if (rotationRoutine != null)
        {
            StopCoroutine(rotationRoutine);
            rotationRoutine = null;
        }

        ClearCircles();
    }

    public void UpdateCircleCount(int newCount)
    {

        maxCircles = Mathf.Clamp(newCount, 1, 6); // 최소 1개, 최대 6개 제한
        ClearCircles(); // 기존 서클을 모두 제거
        InitializeCircles(); // 개수 변경 시 위치 업데이트

    }

    private void InitializeCircles()
    {
        while (circles.Count < maxCircles)
        {
            GameObject newCircle = Instantiate(circlePrefab, transform.position, Quaternion.identity);
            newCircle.transform.SetParent(transform); // 플레이어를 따라다니게 설정
            circles.Add(newCircle);
        }

        while (circles.Count > maxCircles)
        {
            Destroy(circles[circles.Count - 1]);
            circles.RemoveAt(circles.Count - 1);
        }

        UpdateCirclePositions();
    }

    private void UpdateCirclePositions()
    {
        for (int i = 0; i < circles.Count; i++)
        {
            float angle = (360f / circles.Count) * i;
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
            circles[i].transform.position = transform.position + offset;
        }
    }

    private IEnumerator RotateCircles()
    {
        while (true)
        {
            currentAngle += rotationSpeed * Time.deltaTime;
            for (int i = 0; i < circles.Count; i++)
            {
                float angle = (360f / circles.Count) * i + currentAngle;
                Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
                circles[i].transform.position = transform.position + offset;
            }

            yield return null;
        }
    }

    private void ClearCircles()
    {
        foreach (GameObject circle in circles)
        {
            Destroy(circle);
        }
        circles.Clear();
    }
}