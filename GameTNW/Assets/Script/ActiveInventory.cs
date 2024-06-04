using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using static TrashCan;
using static ItemPickup;
using System;
using static UnityEditor.Progress;
using UnityEngine.SceneManagement;


public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNum = 0;
    private SaraControls saraControls;
    public DialogBob dialogBob; // �������������Ѻ�����ҧ�ԧ�֧ DialogBob
    
    public AudioClip throwSound;
    public AudioClip wrongNumSound;
    public AudioClip doorSound;
    public AudioClip Slotsound;
    private AudioSource audioSource;
    public GameObject numofitems;
    public TextMeshProUGUI numofTrashText;  // ��ͧ�ʴ��ӹǹ��з���������/������������� �� 2/15
    public TextMeshProUGUI WrongCountText;  // ��ͧ����Ѻ�ʴ��ӹǹ�������ö�Դ��   �� 3 ��ҷ�駢�����ç���������Ŵ����� 2
    public int Wrongnum = 3;  //�ӹǹ�������ö��駼Դ�� ���������鹤�� 3 ������ö����¹��
    private int numClean = 0;
    private int totalTrashCount;
    private bool portal;
    public GameObject portalon;
    public GameObject portalOff;

    private void Awake()
    {
        saraControls = new SaraControls();
        saraControls.Enable(); // Enable the input actions
    }

    private void Start()
    {
        totalTrashCount = numofitems.transform.childCount;
        UpdateTrashText();
        audioSource = GetComponent<AudioSource>();
        if (saraControls != null)
        {
            saraControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        }
        if (dialogBob == null)
        {
            dialogBob = FindObjectOfType<DialogBob>();
        }
    }

    private void OnEnable()
    {
        if (saraControls != null)
        {
            saraControls.Enable();
        }
    }

    private void OnDisable()
    {
        if (saraControls != null)
        {
            saraControls.Disable();
        }
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue - 1);
    }

    private IEnumerator DeactivateNameAfterDelay(Transform slot, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Deactivate the name
        slot.GetChild(2).gameObject.SetActive(false);
    }


    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        PlaySlotSound();
        // Activate the name of the selected slot
        Transform selectedSlot = this.transform.GetChild(indexNum);
        selectedSlot.GetChild(2).gameObject.SetActive(true);

        // Start coroutine to deactivate the name after a delay
        StartCoroutine(DeactivateNameAfterDelay(selectedSlot, 1f));
    }

    public bool IsFull()
    {
        foreach (Transform inventorySlot in transform)
        {
            Image slotImage = inventorySlot.GetChild(1).GetComponent<Image>();

            // If there is any empty slot, return false
            if (slotImage.sprite == null)
            {
                return false;
            }
        }

        // If no empty slot is found, return true
        return true;
    }

    public void AddItemToSlots(Sprite itemSprite , string itemName , string itemType)
    {
        // Check if the active slot index is valid
        if (activeSlotIndexNum >= 0 && activeSlotIndexNum < transform.childCount)
        {
            // Loop through all inventory slots
            foreach (Transform inventorySlot in transform)
            {
                Image slotImage = inventorySlot.GetChild(1).GetComponent<Image>();

                // Check if the slot is empty
                if (slotImage.sprite == null)
                {
                    // Set the sprite of the slot image to the item sprite
                    slotImage.sprite = itemSprite;

                    // Enable the slot image
                    slotImage.enabled = true;

                    // Open the item after adding it to the slot
                    inventorySlot.GetChild(1).gameObject.SetActive(true);
                    TextMeshProUGUI itemNameText = inventorySlot.GetChild(2).GetComponent<TextMeshProUGUI>();
                    itemNameText.text = itemName;
                    itemNameText.enabled = true;
                    TextMeshProUGUI itemNameType = inventorySlot.GetChild(3).GetComponent<TextMeshProUGUI>();
                    itemNameType.text = itemType;
                    itemNameType.enabled = true;

                    // Debug message to indicate that the item has been added to the slot
                    Debug.Log("Item added to slot successfully: ");

                    return; // Exit the function after adding the item to the first empty slot
                }
            }

            Debug.LogWarning("No empty slot available.");
        }
        else
        {
            Debug.LogWarning("Active slot index is out of range.");
        }
    }


    // Function to clear the currently active item

    public void ClearActiveItemIfMatch(string trashType)
    {
        if (activeSlotIndexNum >= 0 && activeSlotIndexNum < transform.childCount)
        {
            Transform selectedSlot = transform.GetChild(activeSlotIndexNum);
            Image slotImage = selectedSlot.GetChild(1).GetComponent<Image>();

            if (slotImage.sprite != null)
            {
                TextMeshProUGUI itemType = selectedSlot.GetChild(3).GetComponent<TextMeshProUGUI>();
                string itemTypeText = itemType.text;

                if (string.Equals(itemTypeText, trashType.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    ClearSlot();
                    PlayThrowSound();
                    numClean++;
                    UpdateTrashText();
                    Debug.Log("Item thrown into trash: " + itemTypeText);
                }
                else
                {
                    // Ŵ�ӹǹ Wrongnum ŧ ��� ��� Wrongnum ���¡��� ������ҡѺ 0 ��� �����scene gameover
                    DecrementWrongCount();
                    PlayWrongNumSound();
                    if (dialogBob != null)
                    {
                        dialogBob.StartDialog();
                    }
                    else
                    {
                        Debug.LogWarning("DialogBob reference is not set.");
                    }
                 }
            }
            else
            {
                Debug.Log("����������� slot");
            }
        }
        else
        {
            Debug.Log("activeSlotIndexNum");
        }
    }

    private void UpdateTrashText()
    {
        numofTrashText.text = numClean + " / " + totalTrashCount.ToString();
        if (numClean == totalTrashCount)
        {
            portalon.SetActive(true);
            portalOff.SetActive(false) ;
            PlayDoorSound();
        }else
        {
            portalon.SetActive(false);
            portalOff.SetActive(true);
        }
    }

    private void DecrementWrongCount()
    {
        Wrongnum--;
        WrongCountText.text = Wrongnum.ToString();
        if (Wrongnum <= 0)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            // ���¡�ѧ������ش����������ѧ�ҡ Game Over ������س��ͧ���
            if (currentSceneName == "Map4")
            {
                SceneManager.LoadScene("GameOverScene4");
            }
            else if (currentSceneName == "Map3")
            {
                SceneManager.LoadScene("GameOverScene");
            }
            else if (currentSceneName == "Map2")
            {
                // ��������թҡ��� � ����ͧ���仩ҡ Game Over ���ǡѹ
                SceneManager.LoadScene("GameOverScene2");
            }
            else
            {
                SceneManager.LoadScene("GameOverScene1");
            }
        }
    }

    public void ClearSlot()
    {
        if (activeSlotIndexNum >= 0 && activeSlotIndexNum < transform.childCount)
        {
            Transform activeSlot = transform.GetChild(activeSlotIndexNum);
            Image slotImage = activeSlot.GetChild(1).GetComponent<Image>();

            if (slotImage.sprite != null)
            {
                slotImage.sprite = null;
                slotImage.enabled = false;
                activeSlot.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                activeSlot.GetChild(3).GetComponent<TextMeshProUGUI>().text = "";
                Debug.Log("Active item removed from slot: " + activeSlotIndexNum);
            }
        }
    }

    // �������§�Ϳ࿤����ͷ�駢��
    public void PlayThrowSound()
    {
        if (throwSound != null && audioSource != null)
        {
            audioSource.clip = throwSound;
            audioSource.Play();
        }
    }

    // �������§�Ϳ࿤����� Wrongnum Ŵŧ
    public void PlayWrongNumSound()
    {
        if (wrongNumSound != null && audioSource != null)
        {
            audioSource.clip = wrongNumSound;
            audioSource.Play();
        }
    }

    public void PlaySlotSound()
    {
        if (Slotsound != null && audioSource != null)
        {
            audioSource.clip = Slotsound;
            audioSource.Play();
        }
    }
    public void PlayDoorSound()
    {
        if (doorSound != null && audioSource != null)
        {
            audioSource.clip = doorSound;
            audioSource.Play();
        }
    }

}
