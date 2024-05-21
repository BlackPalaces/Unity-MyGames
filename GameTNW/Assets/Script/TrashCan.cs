using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemPickup;
using static TrashCan;

public class TrashCan : MonoBehaviour
{
    public GameObject itemCanvasPrefab;  // Reference to the itemCanvas Prefab
    private GameObject player;  // Reference to the Player
    public float interactDistance = 3f; // ระยะห่างสูงสุดที่เราสามารถแสดงปุ่ม
    private GameObject currentCanvas;

    public enum TrashType
    {
        Hazardous,
        Recyclable,
        Biodegradable,
        General
    }

    [SerializeField]
    private TrashType trashType;
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
            Debug.Log("E key pressed.");

            string MyTrashType = trashType.ToString();
            Debug.Log(MyTrashType);
            FindObjectOfType<ActiveInventory>().ClearActiveItemIfMatch(MyTrashType);
        }
    }

  
}
