using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
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
    public GameObject portalon;
    public GameObject portalOff;
    public TMP_Text PluseWrong;
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


    IEnumerator ShowAndHideText(string text, float duration)
    {
        // Set PluseTime text
        PluseWrong.SetText(text);
        // Show PluseTime
        PluseWrong.gameObject.SetActive(true);

        // Initial alpha value
        float startAlpha = 1f;
        // Final alpha value
        float endAlpha = 0f;
        // Duration for fading
        float fadeDuration = 1f;

        // Start position
        Vector3 startPosition = PluseWrong.transform.position;
        // Target position (slightly above the start position)
        Vector3 targetPosition = startPosition + new Vector3(0f, 20f, 0f);
        // Duration for moving up
        float moveDuration = 1f;

        // Moving animation
        float timer = 0f;
        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            float t = timer / moveDuration;
            PluseWrong.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        // Fading animation
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            Color textColor = PluseWrong.color;
            textColor.a = alpha;
            PluseWrong.color = textColor;
            yield return null;
        }

        // Hide PluseTime
        PluseWrong.gameObject.SetActive(false);
        // Reset alpha value
        Color resetColor = PluseWrong.color;
        resetColor.a = startAlpha;
        PluseWrong.color = resetColor;
        // Reset position
        PluseWrong.transform.position = startPosition;
    }
    void Update()
    {
        // ตรวจสอบการกดปุ่ม T
        if (Input.GetKeyDown(KeyCode.R))
        {
            // เพิ่มเวลา 5 นาที
            Wrongnum += 1;
            // เรียกใช้ Coroutine เพื่อแสดงข้อความ "+300s" เป็นเวลาจำนวน 5 นาที
            StartCoroutine(ShowAndHideText("+1", 2f)); // 2 วินาทีคือระยะเวลาที่ต้องการให้แสดงข้อความ
            WrongCountText.text = Wrongnum.ToString();
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
            if (currentSceneName == "MapFinal")
            {
                SceneManager.LoadScene("GameOverScenes3");
            }
            else if (currentSceneName == "Map3")
            {
                SceneManager.LoadScene("GameOverScene");
            }
            else if (currentSceneName == "Map2")
            {
                // ��������թҡ��� � ����ͧ���仩ҡ Game Over ���ǡѹ
                SceneManager.LoadScene("GameOverScenes2");
            }
            else if (currentSceneName == "MapFortiktok")
            {
                SceneManager.LoadScene("GameOverScene");
            }
            else
            {
                SceneManager.LoadScene("GameOverScenes1");
            }
        }
    }

    public void ClearSlot()
    {
        if (activeSlotIndexNum >= 0 && activeSlotIndexNum < transform.childCount)
        {
            // Clear the active slot first
            Transform activeSlot = transform.GetChild(activeSlotIndexNum);
            Image slotImage = activeSlot.GetChild(1).GetComponent<Image>();

            if (slotImage.sprite != null)
            {
                // Start clearing the current active slot
                slotImage.sprite = null;
                slotImage.enabled = false;
                activeSlot.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                activeSlot.GetChild(3).GetComponent<TextMeshProUGUI>().text = "";

                // Shift items in subsequent slots up
                for (int i = activeSlotIndexNum; i < transform.childCount - 1; i++)
                {
                    Transform currentSlot = transform.GetChild(i);
                    Transform nextSlot = transform.GetChild(i + 1);

                    Image currentSlotImage = currentSlot.GetChild(1).GetComponent<Image>();
                    Image nextSlotImage = nextSlot.GetChild(1).GetComponent<Image>();

                    TextMeshProUGUI currentSlotName = currentSlot.GetChild(2).GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI nextSlotName = nextSlot.GetChild(2).GetComponent<TextMeshProUGUI>();

                    TextMeshProUGUI currentSlotType = currentSlot.GetChild(3).GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI nextSlotType = nextSlot.GetChild(3).GetComponent<TextMeshProUGUI>();

                    // Move the next slot's contents to the current slot
                    currentSlotImage.sprite = nextSlotImage.sprite;
                    currentSlotImage.enabled = nextSlotImage.sprite != null;

                    currentSlotName.text = nextSlotName.text;
                    currentSlotType.text = nextSlotType.text;

                    // Clear the next slot
                    nextSlotImage.sprite = null;
                    nextSlotImage.enabled = false;
                    nextSlotName.text = "";
                    nextSlotType.text = "";
                }

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
