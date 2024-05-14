using UnityEngine;

public class SunScript : MonoBehaviour
{
    private float startTime; // �����������
    private Renderer sunRenderer;

    void Start()
    {
        startTime = Time.time;
        sunRenderer = GetComponent<Renderer>();
        sunRenderer.material.color = Color.black; // ��駤������������繴�
    }

    void Update()
    {
        float currentTime = Time.time - startTime; // ���ҷ���ҹ仵�����������
        float t = Mathf.Clamp01(currentTime / 60f); // �ӹǳ�����������㹪�ǧ 0-1 (0-60 �Թҷ�)

        // ���ҧ�������¤ӹǳ������������
        Color currentColor = Color.Lerp(Color.black, Color.white, t);
        sunRenderer.material.color = currentColor;
    }
}
