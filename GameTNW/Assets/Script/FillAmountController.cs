using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FillAmountController : MonoBehaviour
{
    public GameObject sunMask; // ประกาศตัวแปรสำหรับเก็บ GameObject ของ SunMask
    private Image sunMaskImage; // ประกาศตัวแปรสำหรับเก็บ component Image ของ SunMask

    [SerializeField]
    private float countdownTime = 350f;

    [SerializeField]
    private float startingProgressPercentage = 100f;

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

    IEnumerator CountdownAndFill(float currentTime)
    {
        while (currentTime <= countdownTime)
        {
            float fillAmount = Mathf.Clamp01((countdownTime - currentTime) / countdownTime);

            // กำหนด Fill Amount ของ SunMask จากค่า fillAmount ที่ได้
            sunMaskImage.fillAmount = 1f - fillAmount;

            // เพิ่มเวลาที่ผ่านไปตามเวลาในแต่ละ frame
            currentTime += Time.deltaTime;
            yield return null; // รอให้หน่วยเวลาของ Unity ประมวลผลต่อไป
        }

        // เมื่อเวลานับถอยหลังหมดลง กำหนด Fill Amount ให้เต็ม
        sunMaskImage.fillAmount = 1f;
    }
}
