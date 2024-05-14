using UnityEngine;

public class SunScript : MonoBehaviour
{
    private float startTime; // เวลาเริ่มต้น
    private Renderer sunRenderer;

    void Start()
    {
        startTime = Time.time;
        sunRenderer = GetComponent<Renderer>();
        sunRenderer.material.color = Color.black; // ตั้งค่าสีเริ่มต้นเป็นดำ
    }

    void Update()
    {
        float currentTime = Time.time - startTime; // เวลาที่ผ่านไปตั้งแต่เริ่มต้น
        float t = Mathf.Clamp01(currentTime / 60f); // คำนวณเวลาให้อยู่ในช่วง 0-1 (0-60 วินาที)

        // สร้างสีใหม่โดยคำนวณตามเวลาและสี
        Color currentColor = Color.Lerp(Color.black, Color.white, t);
        sunRenderer.material.color = currentColor;
    }
}
