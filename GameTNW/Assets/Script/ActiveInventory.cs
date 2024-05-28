using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using static TrashCan;
using static ItemPickup;
using System;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNum = 0;
    private SaraControls saraControls;
    public DialogBob dialogBob; // เพิ่มตัวแปรสำหรับการอ้างอิงถึง DialogBob

    private void Awake()
    {
        saraControls = new SaraControls();
        saraControls.Enable(); // Enable the input actions
    }

    private void Start()
    {
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
                    Debug.Log("Item thrown into trash: " + itemTypeText);
                }
                else
                {
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
                Debug.Log("ไม่มีไอเท็มใน slot");
            }
        }
        else
        {
            Debug.Log("activeSlotIndexNum");
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

}
