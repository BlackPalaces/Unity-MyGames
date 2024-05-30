using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BookInventory : MonoBehaviour
{
    public GameObject bookUI; // GameObject ����� UI �ͧ˹ѧ���
    public List<GameObject> pages; // ��¡�âͧ˹���˹ѧ���
    private bool isBookOpen = false;
    private int currentPage = 0;
    public SaraController playerController; // �������������Ѻ��ҧ�ԧ��ѧʤ�Ի�� SaraController

    public AudioClip openBookSound;
    public AudioClip closeBookSound;
    public AudioClip pageFlipSound;
    private AudioSource audioSource;

    void Start()
    {
        // ������鹻Դ UI �ͧ˹ѧ���
        bookUI.SetActive(false);
        // �Դ˹�ҷ����� ¡���˹�ҷ���������
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == currentPage);
        }

        // ���� PlayerController ���
        if (playerController == null)
        {
            playerController = FindObjectOfType<SaraController>();
        }

        // Initialize the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // ��Ǩ�ͺ��á����� f �����Դ/�Դ˹ѧ���
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            ToggleBook();
        }

        // ���˹ѧ����Դ���� ��Ǩ�ͺ��á����� a ��� d ��������¹˹��
        if (isBookOpen)
        {
            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                PreviousPage();
            }
            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                NextPage();
            }
        }
    }

    void ToggleBook()
    {
        isBookOpen = !isBookOpen;
        bookUI.SetActive(isBookOpen);

        if (isBookOpen)
        {
            // �Դ��÷ӧҹ�ͧ PlayerController �����˹ѧ����Դ
            if (playerController != null)
            {
                playerController.enabled = false;
                if (playerController.audioSource != null && playerController.audioSource.isPlaying)
                {
                    playerController.audioSource.Stop();
                }
            }
            // Play the open book sound effect
            if (audioSource != null && openBookSound != null)
            {
                audioSource.PlayOneShot(openBookSound);
            }
            Debug.Log("Book opened");
        }
        else
        {
            // �Դ��÷ӧҹ�ͧ PlayerController �����˹ѧ��ͻԴ
            if (playerController != null)
            {
                playerController.enabled = true;
            }
            // Play the close book sound effect
            if (audioSource != null && closeBookSound != null)
            {
                audioSource.PlayOneShot(closeBookSound);
            }
            Debug.Log("Book closed");
        }
    }

    void NextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            pages[currentPage].SetActive(false);
            currentPage++;
            pages[currentPage].SetActive(true);
            // Play the page flip sound effect
            if (audioSource != null && pageFlipSound != null)
            {
                audioSource.PlayOneShot(pageFlipSound);
            }
            Debug.Log("Next Page: " + currentPage);
        }
    }

    void PreviousPage()
    {
        if (currentPage > 0)
        {
            pages[currentPage].SetActive(false);
            currentPage--;
            pages[currentPage].SetActive(true);
            // Play the page flip sound effect
            if (audioSource != null && pageFlipSound != null)
            {
                audioSource.PlayOneShot(pageFlipSound);
            }
            Debug.Log("Previous Page: " + currentPage);
        }
    }

    // �ѧ��ѹ������¡������͡������˹��˹ѧ���
    public void OnPageButtonClick()
    {
        Debug.Log("Button on page " + currentPage + " clicked!");
        // ������÷ӧҹ���س��ͧ��÷����
    }
}