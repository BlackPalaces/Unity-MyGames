using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcScript : MonoBehaviour
{
    public GameObject dialogBox; // Dialog box �����ʴ�����͡����� E
    public GameObject itemCanvasPrefab;
    private GameObject currentCanvas;// NPC �ա�ê�
    public float interactDistance = 2f; // ������ҧ�٧�ش����������ö�ʴ�����
    public TMP_Text dialogText; // Reference ����Ѻ Text Component ������ʴ���ͤ���
    public Image dialogImage; // Reference ����Ѻ Image Component ������ʴ��Ҿ
    public TMP_Text characterNameText; // Reference ����Ѻ Text Component ������ʴ����͵���Ф�
    public bool allowRepeatedDialog = true;
    private bool isPlayerInRange = false; // ����Ҽ���������������������
    private bool isDialogActive = false; // ����ҡ��ʹ��ҡ��ѧ�ӧҹ�������
    private int currentDialogIndex = 0; // �Ѫ�բͧ dialog �Ѩ�غѹ
    public DialogContainer dialogContainer; // ��ҧ�ԧ�֧ DialogContainer
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

        // ��������������С����� E ����ѧ�����ӡ��ʹ��ҡѺ NPC ��������͵�駤�� allowRepeatedDialog �� true
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && (!isDialogActive || allowRepeatedDialog))
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
