using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcScript : MonoBehaviour
{
    public GameObject dialogBox; // Dialog box �����ʴ�����͡����� E
    public GameObject button; // ���������ʴ������ NPC �ա�ê�
    public float interactDistance = 2f; // ������ҧ�٧�ش����������ö�ʴ�����
    public TMP_Text dialogText; // Reference ����Ѻ Text Component ������ʴ���ͤ���
    public Image dialogImage; // Reference ����Ѻ Image Component ������ʴ��Ҿ
    public TMP_Text characterNameText; // Reference ����Ѻ Text Component ������ʴ����͵���Ф�
    public bool allowRepeatedDialog = true;
    private bool isPlayerInRange = false; // ����Ҽ���������������������
    private bool isDialogActive = false; // ����ҡ��ʹ��ҡ��ѧ�ӧҹ�������
    private int currentDialogIndex = 0; // �Ѫ�բͧ dialog �Ѩ�غѹ
    public DialogContainer dialogContainer; // ��ҧ�ԧ�֧ DialogContainer

    private void Update()
    {
        // ��Ǩ�ͺ������ҧ�����ҧ NPC ��м�����
        float distance = Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position);

        // ���������ҧ���¡���������ҡѺ���з���˹�
        if (distance <= interactDistance)
        {
            // �ʴ�����
            button.SetActive(true);
            isPlayerInRange = true;
        }
        else
        {
            // ��͹����
            button.SetActive(false);
            isPlayerInRange = false;
        }

        // ��������������С����� E ����ѧ�����ӡ��ʹ��ҡѺ NPC ��������͵�駤�� allowRepeatedDialog �� true
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && (!isDialogActive || allowRepeatedDialog))
        {
            if (dialogContainer != null && dialogContainer.dialogs != null && dialogContainer.dialogs.Count > 0)
            {
                StartDialog(); // ��������ʹ���
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
            dialogText.text = currentDialog.dialogText;
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



            /* StartCoroutine(ShowNextDialogAfterDelay(currentDialog.skipDelay));*/
            float delay = currentDialog.dialogAudio.length + currentDialog.skipDelay;
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
        dialogBox.SetActive(false);
    }
}
