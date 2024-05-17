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
}
