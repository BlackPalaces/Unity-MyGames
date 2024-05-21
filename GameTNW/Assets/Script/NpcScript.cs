using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcScript : MonoBehaviour
{
    public GameObject dialogBox; // Dialog box ที่จะแสดงเมื่อกดปุ่ม E
    public GameObject itemCanvasPrefab;
    private GameObject currentCanvas;// NPC มีการชน
    public float interactDistance = 2f; // ระยะห่างสูงสุดที่เราสามารถแสดงปุ่ม
    public TMP_Text dialogText; // Reference สำหรับ Text Component ที่ใช้แสดงข้อความ
    public Image dialogImage; // Reference สำหรับ Image Component ที่ใช้แสดงภาพ
    public TMP_Text characterNameText; // Reference สำหรับ Text Component ที่ใช้แสดงชื่อตัวละคร
    public bool allowRepeatedDialog = true;
    private bool isPlayerInRange = false; // เช็คว่าผู้เล่นอยู่ในระยะหรือไม่
    private bool isDialogActive = false; // เช็คว่าการสนทนากำลังทำงานหรือไม่
    private int currentDialogIndex = 0; // ดัชนีของ dialog ปัจจุบัน
    public DialogContainer dialogContainer; // อ้างอิงถึง DialogContainer
    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");  // Find the player object
    }

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactDistance);
        bool playerNearby = false;

        // Check if player is nearby
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerNearby = true;
                break;
            }
        }

        // Show/hide itemCanvas
        if (playerNearby)
        {
            if (currentCanvas == null)
            {
                // Instantiate itemCanvasPrefab and set its position to the left of the item
                currentCanvas = Instantiate(itemCanvasPrefab, transform.position + Vector3.left * 1f + Vector3.up * 2f, Quaternion.identity);
            }
            else if (!currentCanvas.activeSelf)
            {
                currentCanvas.SetActive(true);
            }
        }
        else
        {
            // Destroy currentCanvas if player is not nearby
            if (currentCanvas != null)
            {
                Destroy(currentCanvas);
                currentCanvas = null;
            }
        }

        // ถ้าอยู่ในระยะและกดปุ่ม E และยังไม่ได้ทำการสนทนากับ NPC หรือเมื่อตั้งค่า allowRepeatedDialog เป็น true
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && (!isDialogActive || allowRepeatedDialog))
        {
            if (dialogContainer != null && dialogContainer.dialogs != null && dialogContainer.dialogs.Count > 0)
            {
                StartDialog(); // เริ่มการสนทนา
            }
            else
            {
                Debug.LogWarning("DialogContainer or dialogs list is not properly initialized.");
            }
        }
    }

    private void StartDialog()
    {
        if (!isDialogActive)
        {
            isDialogActive = true;
            currentDialogIndex = 0;
            dialogBox.SetActive(true);
            ShowDialog(currentDialogIndex);
        }
        else
        {
            Debug.Log("Already in conversation.");
        }
    }

    private void ShowNextDialog()
    {
        if (dialogContainer != null && dialogContainer.dialogs != null && currentDialogIndex < dialogContainer.dialogs.Count)
        {
            currentDialogIndex++;
            if (currentDialogIndex < dialogContainer.dialogs.Count)
            {
                ShowDialog(currentDialogIndex);
            }
            else
            {
                EndDialog(); // จบการสนทนา
            }
        }
        else
        {
            Debug.LogWarning("Cannot show next dialog. DialogContainer is not properly initialized or index is out of range.");
        }
    }


    private void ShowDialog(int index)
    {
        if (dialogContainer != null && dialogContainer.dialogs != null && index >= 0 && index < dialogContainer.dialogs.Count)
        {
            NpcDialog currentDialog = dialogContainer.dialogs[index];
            dialogText.text = currentDialog.dialogText;
            dialogImage.sprite = currentDialog.characterSprite;
            characterNameText.text = currentDialog.characterName;

            if (currentDialog.dialogAudio != null)
            {
                // เล่นเสียงของ Dialog พร้อมปรับระดับเสียง
                AudioSource npcAudioSource = GetComponent<AudioSource>(); // หา AudioSource ที่เชื่อมโยงกับ NPC
                if (npcAudioSource == null)
                {
                    npcAudioSource = gameObject.AddComponent<AudioSource>(); // ถ้าไม่มี AudioSource ให้เพิ่ม AudioSource เข้าไป
                }
                npcAudioSource.clip = currentDialog.dialogAudio; // กำหนดเสียงของ Dialog
                npcAudioSource.volume = currentDialog.audioVolume; // กำหนดระดับเสียง
                npcAudioSource.Play(); // เล่นเสียง
            }



            /* StartCoroutine(ShowNextDialogAfterDelay(currentDialog.skipDelay));*/
            float delay = currentDialog.dialogAudio.length + currentDialog.skipDelay;
            StartCoroutine(ShowNextDialogAfterDelay(delay));
        }
        else
        {
            Debug.LogWarning("Cannot show dialog. DialogContainer is not properly initialized or index is out of range.");
        }
    }




    // ฟังก์ชันสำหรับเรียกใช้ ShowNextDialog() หลังจากเวลาที่กำหนด
    private IEnumerator ShowNextDialogAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowNextDialog();
    }


    private void EndDialog()
    {
        isDialogActive = false;
        dialogBox.SetActive(false);
    }
}
