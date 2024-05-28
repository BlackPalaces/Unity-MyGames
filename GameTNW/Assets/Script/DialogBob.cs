using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogBob : MonoBehaviour
{
    public GameObject dialogPanel; // UI Panel สำหรับการแสดง dialog
    public Image dialogImage; // Image สำหรับรูปตัวละคร
    public TextMeshProUGUI characterNameText; // Text สำหรับชื่อของตัวละคร
    public TextMeshProUGUI dialogText; // Text สำหรับข้อความ dialog
    public DialogContainer dialogContainer;
    private int currentDialogIndex = 0;
    private bool isDialogActive = false;
    private TypewriterEffect typewriterEffect; // Reference to TypewriterEffect

    void Start()
    {
        typewriterEffect = dialogText.GetComponent<TypewriterEffect>(); // Ensure typewriterEffect is assigned
    }

    public void StartDialog()
    {
        if (!isDialogActive)
        {
            isDialogActive = true;
            currentDialogIndex = 0;
            dialogPanel.SetActive(true);
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
            if (currentDialog != null && currentDialog.dialogText != null)
            {
                typewriterEffect.StartTyping(currentDialog.dialogText);
            }
            else
            {
                Debug.LogWarning("currentDialog or currentDialog.dialogText is null.");
            }

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

            float delay = currentDialog.dialogAudio != null ? currentDialog.dialogAudio.length + currentDialog.skipDelay : currentDialog.skipDelay;
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
        dialogPanel.SetActive(false);
    }
}
