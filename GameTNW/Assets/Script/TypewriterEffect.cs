using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.1f; // ������Ҫ�������ҧ����ʴ��ѡ���
    private TMP_Text textComponent; // ����๹���ͤ��� TMP
    private string fullText; // ��ͤ���������
    private string currentText = ""; // ��ͤ����Ѩ�غѹ���١�ʴ�
    private bool isTyping = false; // ʶҹТͧ��þ�����ͤ���

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
