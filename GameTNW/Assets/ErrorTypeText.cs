using UnityEngine;
using TMPro;
using System.Collections;

public class ErrorTypeText : MonoBehaviour
{
    public TextMeshProUGUI errorText; // อ้างอิงถึง TextMeshPro component
    public float typingSpeed = 0.05f; // ความเร็วในการพิมพ์แต่ละตัวอักษร
    public AudioClip typingSound; // เสียงพิมพ์

    private string fullText;
    private AudioSource audioSource;

    void Start()
    {
        // เก็บข้อความทั้งหมดไว้
        fullText = errorText.text;
        // ทำให้ข้อความเริ่มต้นเป็นว่างเปล่า
        errorText.text = "";
        // รับ AudioSource จาก GameObject นี้
        audioSource = GetComponent<AudioSource>();
        // เริ่ม Coroutine สำหรับการพิมพ์ข้อความ
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            // พิมพ์ตัวอักษร
            errorText.text += letter;
            // เล่นเสียงพิมพ์
            audioSource.PlayOneShot(typingSound);
            // รอเวลาตามความเร็วในการพิมพ์
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
