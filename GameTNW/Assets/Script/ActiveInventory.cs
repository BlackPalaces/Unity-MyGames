using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNum = 0;
    private SaraControls saraControls;

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

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
    }

    public void AddItemToSlots(Sprite itemSprite)
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

                    // Debug message to indicate that the item has been added to the slot
                    Debug.Log("Item added to slot successfully.");

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


}
