using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogBob : MonoBehaviour
{
    public GameObject dialogPanel; // UI Panel ����Ѻ����ʴ� dialog
    public Image dialogImage; // Image ����Ѻ�ٻ����Ф�
    public TextMeshProUGUI characterNameText; // Text ����Ѻ���ͧ͢����Ф�
    public TextMeshProUGUI dialogText; // Text ����Ѻ��ͤ��� dialog
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
                EndDialog(); // �����ʹ���
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
                // ������§�ͧ Dialog �������Ѻ�дѺ���§
                AudioSource npcAudioSource = GetComponent<AudioSource>(); // �� AudioSource ���������§�Ѻ NPC
                if (npcAudioSource == null)
                {
                    npcAudioSource = gameObject.AddComponent<AudioSource>(); // �������� AudioSource ������� AudioSource ����
                }
                npcAudioSource.clip = currentDialog.dialogAudio; // ��˹����§�ͧ Dialog
                npcAudioSource.volume = currentDialog.audioVolume; // ��˹��дѺ���§
                npcAudioSource.Play(); // ������§
            }

            float delay = currentDialog.dialogAudio != null ? currentDialog.dialogAudio.length + currentDialog.skipDelay : currentDialog.skipDelay;
            StartCoroutine(ShowNextDialogAfterDelay(delay));
        }
        else
        {
            Debug.LogWarning("Cannot show dialog. DialogContainer is not properly initialized or index is out of range.");
        }
    }

    // �ѧ��ѹ����Ѻ���¡�� ShowNextDialog() ��ѧ�ҡ���ҷ���˹�
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
