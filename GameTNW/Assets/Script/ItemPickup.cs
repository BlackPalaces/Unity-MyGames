using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemPickup : MonoBehaviour
{
    public GameObject itemCanvasPrefab;  // Reference to the itemCanvas Prefab
    private GameObject player;  // Reference to the Player
    public float interactDistance = 3f; // ระยะห่างสูงสุดที่เราสามารถแสดงปุ่ม
    private GameObject currentCanvas;  // Reference to the current itemCanvas
    [SerializeField]
    private string itemName;
    public AudioClip itemPickupSound;

    public enum ItemType
    {
        Hazardous,
        Recyclable,
        Biodegradable,
        General
    }

    [SerializeField]
    private ItemType itemType;


    void Start()
    {
        player = GameObject.FindWithTag("Player");  // Find the player object
    }

    void Update()
    {
        if (player == null) return; // Ensure the player reference is not null

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

        // Pickup item if E key is pressed
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    void PickupItem()
    {
        // Check if the active inventory has available slots
        if (!IsInventoryFull())
        {
            Debug.Log("itemName is: " + itemName);
            Debug.Log("itemType is: " + itemType);
            Debug.Log("Item Picked Up! " + itemName + itemType);
            // Add logic to pick up the item (e.g., add to inventory)
            // Call AddItemToSlots function to add item to inventory slots
            if (itemPickupSound != null)
            {
                AudioSource.PlayClipAtPoint(itemPickupSound, transform.position);
            }
            else
            {
                Debug.LogError("itemPickupSound is null.");
            }
            Sprite itemSprite = GetComponent<SpriteRenderer>().sprite; // Assuming the item has a SpriteRenderer component
            string MyitemType = itemType.ToString();
            Debug.Log("MyitemType is: " + MyitemType);
            FindObjectOfType<ActiveInventory>().AddItemToSlots(itemSprite, itemName, MyitemType);

            // Destroy itemCanvas only if it exists
            if (currentCanvas != null)
            {
                Destroy(currentCanvas);
            }
            Destroy(gameObject);  // Destroy the item after picking up
        }
    }

    // Function to check if the inventory is full
    bool IsInventoryFull()
    {
        // Get reference to the ActiveInventory component
        ActiveInventory activeInventory = FindObjectOfType<ActiveInventory>();

        // Check if there are available slots in the inventory
        // Assuming ActiveInventory has a method to check if it's full
        if (activeInventory != null && activeInventory.IsFull())
        {
            Debug.Log("Inventory is full!");
            return true;
        }

        return false;
    }


}
