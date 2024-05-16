using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.1f; // ความล่าช้าระหว่างการแสดงอักขระ
    private TMP_Text textComponent; // คอมโพเนนต์ข้อความ TMP
    private string fullText; // ข้อความทั้งหมด
    private string currentText = ""; // ข้อความปัจจุบันที่ถูกแสดง
    private bool isTyping = false; // สถานะของการพิมพ์ข้อความ

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        fullText = textComponent.text;
        textComponent.text = "";
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textComponent.text = currentText;
            yield return new WaitForSeconds(delay);
        }
        isTyping = false;
    }

    public void SkipTyping()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            textComponent.text = fullText;
            isTyping = false;
        }
    }
}
