using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CardScript : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 originalPosition;

    public Transform dropTarget; // Drag and drop ChooseB object here
    public float snapDistance = 50f; // Adjust this distance based on your needs

    public Texture2D dragCursor; // Add this line
    private Texture2D defaultCursor;
    private Vector2 cursorHotspot = Vector2.zero;


    private void Start()
    {
        originalPosition = transform.position;
        defaultCursor = null; // Default cursor (system cursor)
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            (RectTransform)transform,
            eventData.position,
            eventData.pressEventCamera,
            out var worldPoint
        );
        offset = transform.position - worldPoint;

        // Change the cursor
        Cursor.SetCursor(dragCursor, cursorHotspot, CursorMode.Auto);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                (RectTransform)transform,
                eventData.position,
                eventData.pressEventCamera,
                out var worldPoint
            );
            transform.position = worldPoint + offset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // Change the cursor back to default
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);

        float distance = Vector3.Distance(transform.position, dropTarget.position);
        if (distance <= snapDistance)
        {
            transform.position = dropTarget.position;

            // Check the tag of the object
            if (gameObject.CompareTag("HomeButton"))
            {
                // Perform action for Home
                Debug.Log("Home Image Dropped");
                // Example: Change to Home scene
                SceneManager.LoadScene("Home");
            }
            else if (gameObject.CompareTag("ExitButton"))
            {
                // Perform action for Exit
                Debug.Log("Exit Image Dropped");
                // Example: Quit application
                Application.Quit();
            }
        }
        else
        {
            transform.position = originalPosition;
        }
    }
}