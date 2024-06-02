using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.1f; // ������Ҫ�������ҧ����ʴ��ѡ���
    public float shakeAmount = 2f; // �����ع�ç�ͧ������ҵ���ѡ��
    private TMP_Text textComponent; // ����๹���ͤ��� TMP
    private string fullText; // ��ͤ���������
    private string currentText = ""; // ��ͤ����Ѩ�غѹ���١�ʴ�
    private bool isTyping = false; // ʶҹТͧ��þ�����ͤ���
    private Coroutine shakeCoroutine;

    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        // ��������Ң�ͤ�������� GameObject �١�Դ��ҹ
        if (shakeCoroutine == null)
        {
            shakeCoroutine = StartCoroutine(ShakeTextContinuously());
        }
    }

    void OnDisable()
    {
        // ��ش���Ң�ͤ�������� GameObject �١�Դ��ҹ
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
            yield return null; // ����㹷ء� ���
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
