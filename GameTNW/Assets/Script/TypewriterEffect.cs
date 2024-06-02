using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.1f; // ความล่าช้าระหว่างการแสดงอักขระ
    public float shakeAmount = 2f; // ความรุนแรงของการเขย่าตัวอักษร
    private TMP_Text textComponent; // คอมโพเนนต์ข้อความ TMP
    private string fullText; // ข้อความทั้งหมด
    private string currentText = ""; // ข้อความปัจจุบันที่ถูกแสดง
    private bool isTyping = false; // สถานะของการพิมพ์ข้อความ
    private Coroutine shakeCoroutine;

    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        // เริ่มเขย่าข้อความเมื่อ GameObject ถูกเปิดใช้งาน
        if (shakeCoroutine == null)
        {
            shakeCoroutine = StartCoroutine(ShakeTextContinuously());
        }
    }

    void OnDisable()
    {
        // หยุดเขย่าข้อความเมื่อ GameObject ถูกปิดใช้งาน
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }
    }

    public void StartTyping(string newText)
    {
        if (isTyping)
        {
            SkipTyping();
        }

        fullText = newText;
        textComponent.text = "";
        currentText = "";
        isTyping = true;

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

    IEnumerator ShakeTextContinuously()
    {
        while (true)
        {
            ShakeText();
            yield return null; // เขย่าในทุกๆ เฟรม
        }
    }

    private void ShakeText()
    {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            var verts = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                var original = verts[textInfo.characterInfo[i].vertexIndex + j];
                var offset = Random.insideUnitCircle * shakeAmount;
                verts[textInfo.characterInfo[i].vertexIndex + j] = original + new Vector3(offset.x, offset.y, 0);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    public void SkipTyping()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            textComponent.text = fullText;
            isTyping = false;
            shakeCoroutine = StartCoroutine(ShakeTextContinuously());
        }
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}
