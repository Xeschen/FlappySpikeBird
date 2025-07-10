using UnityEngine;

public class CameraZByAspect : MonoBehaviour
{
    public Camera targetCamera;
    public float referenceZ = -30f;                  // 9:16 비율에서의 기준 카메라 Z
    public float referenceAspect = 9f / 16f;         // 기준 종횡비

    private float currentAspect;

    private void Start()
    {
        AdjustCameraZ();
    }

    private void Update()
    {
        // 화면 크기 변경 시 카메라 Z 조정
        if (Mathf.Abs(currentAspect - ((float)Screen.width / Screen.height)) > 0.01f)
        {
            AdjustCameraZ();
        }
    }

    private void AdjustCameraZ()
    {
        currentAspect = (float)Screen.width / Screen.height;

        // Canvas Scaler (match = 0.5) 기준 보간: sqrt(reference / current)
        float scaleRatio = Mathf.Sqrt(referenceAspect / currentAspect);

        float z = referenceZ * scaleRatio;

        // 적용
        Vector3 pos = targetCamera.transform.position;
        pos.z = z;
        targetCamera.transform.position = pos;

        Debug.Log($"[CameraZByAspect] Aspect: {currentAspect:F4}, Z: {z:F3}");
    }
}