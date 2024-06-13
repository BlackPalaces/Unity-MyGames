using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class FillAmountController : MonoBehaviour
{
    public GameObject sunMask; // ประกาศตัวแปรสำหรับเก็บ GameObject ของ SunMask
    private Image sunMaskImage; // ประกาศตัวแปรสำหรับเก็บ component Image ของ SunMask

    [SerializeField]
    private float countdownTime = 350f;

    [SerializeField]
    private float startingProgressPercentage = 100f;
    public TMP_Text PluseTime;
    public TMP_Text TimeCount;

    void Start()
    {
        // เรียกใช้เมท็อด GetCompnent<Image>() เพื่อเข้าถึง component Image ของ SunMask
        sunMaskImage = sunMask.GetComponent<Image>();

        // คำนวณเวลาที่เหลือตามความคืบหน้าเริ่มต้นที่กำหนด
        float currentTime = countdownTime * (1f - startingProgressPercentage / 100f);

        // กำหนด Fill Amount ของ SunMask ตามความคืบหน้าเริ่มต้นที่กำหนด
        sunMaskImage.fillAmount = startingProgressPercentage / 100f;

        // เริ่มการนับถอยหลังโดยใช้เวลาที่คำนวณได้
        StartCoroutine(CountdownAndFill(currentTime));
    }


    IEnumerator ShowAndHideText(string text, float duration)
    {
        // Set PluseTime text
        PluseTime.SetText(text);
        // Show PluseTime
        PluseTime.gameObject.SetActive(true);

        // Initial alpha value
        float startAlpha = 1f;
        // Final alpha value
        float endAlpha = 0f;
        // Duration for fading
        float fadeDuration = 1f;

        // Start position
        Vector3 startPosition = PluseTime.transform.position;
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
            PluseTime.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        // Fading animation
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            Color textColor = PluseTime.color;
            textColor.a = alpha;
            PluseTime.color = textColor;
            yield return null;
        }

        // Hide PluseTime
        PluseTime.gameObject.SetActive(false);
        // Reset alpha value
        Color resetColor = PluseTime.color;
        resetColor.a = startAlpha;
        PluseTime.color = resetColor;
        // Reset position
        PluseTime.transform.position = startPosition;
    }



    void Update()
    {
        // ตรวจสอบการกดปุ่ม T
        if (Input.GetKeyDown(KeyCode.T))
        {
            // เพิ่มเวลา 5 นาที
            countdownTime += 300f;
            // เรียกใช้ Coroutine เพื่อแสดงข้อความ "+300s" เป็นเวลาจำนวน 5 นาที
            StartCoroutine(ShowAndHideText("+300s", 2f)); // 2 วินาทีคือระยะเวลาที่ต้องการให้แสดงข้อความ
        }


        // ตรวจสอบค่า fillAmount ในทุกเฟรม
        if (sunMaskImage.fillAmount >= 1f)
        {
            // ถ้าค่า fillAmount เท่ากับ 1 หรือมากกว่า ให้ย้ายไปยังฉาก Gameover
            string currentSceneName = SceneManager.GetActiveScene().name;
            // เรียกฟังก์ชั่นหยุดเกมหรือส่งไปยังฉาก Game Over ตามที่คุณต้องการ
            if (currentSceneName == "Map3")
            {
                SceneManager.LoadScene("GameOverScene");
            }
            else if (currentSceneName == "Map2")
            {
                SceneManager.LoadScene("GameOverScenes2");
            }
            else if (currentSceneName == "Map1")
            {
                SceneManager.LoadScene("GameOverScenes1");
            }
            else if (currentSceneName == "MapFinal")
            {
                SceneManager.LoadScene("GameOverScenes3");
            }
            else if (currentSceneName == "MapFortiktok")
            {
                SceneManager.LoadScene("GameOverScenes");
            }
            else
            {
                // เผื่อว่ามีฉากอื่น ๆ ที่ต้องการไปฉาก Game Over เดียวกัน
                SceneManager.LoadScene("Home");
            }
        }
    }
    IEnumerator CountdownAndFill(float currentTime)
    {
        while (currentTime <= countdownTime)
        {
            float fillAmount = Mathf.Clamp01((countdownTime - currentTime) / countdownTime);

            // กำหนด Fill Amount ของ SunMask จากค่า fillAmount ที่ได้
            sunMaskImage.fillAmount = 1f - fillAmount;

            float remainingTime = countdownTime - currentTime;
            TimeCount.SetText(FormatTime(remainingTime));

            // เพิ่มเวลาที่ผ่านไปตามเวลาในแต่ละ frame
            currentTime += Time.deltaTime;
            yield return null; // รอให้หน่วยเวลาของ Unity ประมวลผลต่อไป
        }

        // เมื่อเวลานับถอยหลังหมดลง กำหนด Fill Amount ให้เต็ม
        sunMaskImage.fillAmount = 1f;

    }
    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }


}
