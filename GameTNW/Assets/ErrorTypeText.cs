using UnityEngine;
using TMPro;
using System.Collections;

public class ErrorTypeText : MonoBehaviour
{
    public TextMeshProUGUI errorText; // ��ҧ�ԧ�֧ TextMeshPro component
    public float typingSpeed = 0.05f; // ��������㹡�þ�������е���ѡ��
    public AudioClip typingSound; // ���§�����

    private string fullText;
    private AudioSource audioSource;

    void Start()
    {
        // �红�ͤ������������
        fullText = errorText.text;
        // ������ͤ��������������ҧ����
        errorText.text = "";
        // �Ѻ AudioSource �ҡ GameObject ���
        audioSource = GetComponent<AudioSource>();
        // ����� Coroutine ����Ѻ��þ�����ͤ���
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            // ��������ѡ��
            errorText.text += letter;
            // ������§�����
            audioSource.PlayOneShot(typingSound);
            // �����ҵ����������㹡�þ����
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
