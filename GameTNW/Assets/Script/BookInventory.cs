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
            Debug.Log("Book opened");
        }
        else
        {
            // �Դ��÷ӧҹ�ͧ PlayerController �����˹ѧ��ͻԴ
            if (playerController != null)
            {
                playerController.enabled = true;
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