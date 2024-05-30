using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BookInventory : MonoBehaviour
{
    public GameObject bookUI; // GameObject ที่เป็น UI ของหนังสือ
    public List<GameObject> pages; // รายการของหน้าในหนังสือ
    private bool isBookOpen = false;
    private int currentPage = 0;
    public SaraController playerController; // เพิ่มตัวแปรสำหรับอ้างอิงไปยังสคริปต์ SaraController

    public AudioClip openBookSound;
    public AudioClip closeBookSound;
    public AudioClip pageFlipSound;
    private AudioSource audioSource;

    void Start()
    {
        // เริ่มต้นปิด UI ของหนังสือ
        bookUI.SetActive(false);
        // ปิดหน้าทั้งหมด ยกเว้นหน้าที่เริ่มต้น
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == currentPage);
        }

        // ค้นหา PlayerController ในเกม
        if (playerController == null)
        {
            playerController = FindObjectOfType<SaraController>();
        }

        // Initialize the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // ตรวจสอบการกดปุ่ม f เพื่อเปิด/ปิดหนังสือ
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            ToggleBook();
        }

        // ถ้าหนังสือเปิดอยู่ ตรวจสอบการกดปุ่ม a และ d เพื่อเปลี่ยนหน้า
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
            // ปิดการทำงานของ PlayerController เมื่อหนังสือเปิด
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
            // เปิดการทำงานของ PlayerController เมื่อหนังสือปิด
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

    // ฟังก์ชันที่เรียกใช้เมื่อกดปุ่มในหน้าหนังสือ
    public void OnPageButtonClick()
    {
        Debug.Log("Button on page " + currentPage + " clicked!");
        // เพิ่มการทำงานที่คุณต้องการที่นี่
    }
}