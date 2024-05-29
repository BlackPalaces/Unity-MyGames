using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FillAmountController : MonoBehaviour
{
    public GameObject sunMask; // ��С�ȵ��������Ѻ�� GameObject �ͧ SunMask
    private Image sunMaskImage; // ��С�ȵ��������Ѻ�� component Image �ͧ SunMask

    [SerializeField]
    private float countdownTime = 350f;

    [SerializeField]
    private float startingProgressPercentage = 100f;

    void Start()
    {
        // ���¡������ʹ GetCompnent<Image>() ������Ҷ֧ component Image �ͧ SunMask
        sunMaskImage = sunMask.GetComponent<Image>();

        // �ӹǳ���ҷ������͵�������׺˹��������鹷���˹�
        float currentTime = countdownTime * (1f - startingProgressPercentage / 100f);

        // ��˹� Fill Amount �ͧ SunMask ��������׺˹��������鹷���˹�
        sunMaskImage.fillAmount = startingProgressPercentage / 100f;

        // �������ùѺ�����ѧ�������ҷ��ӹǳ��
        StartCoroutine(CountdownAndFill(currentTime));
    }

    void Update()
    {
        // ��Ǩ�ͺ��� fillAmount 㹷ء���
        if (sunMaskImage.fillAmount >= 1f)
        {
            // ��Ҥ�� fillAmount ��ҡѺ 1 �����ҡ���� ���������ѧ�ҡ Gameover
            string currentSceneName = SceneManager.GetActiveScene().name;
            // ���¡�ѧ������ش����������ѧ�ҡ Game Over ������س��ͧ���
            if (currentSceneName == "Map3")
            {
                SceneManager.LoadScene("GameOverScene");
            }
            else if (currentSceneName == "Map2")
            {
                SceneManager.LoadScene("Home");
            }
            else
            {
                // ��������թҡ��� � ����ͧ���仩ҡ Game Over ���ǡѹ
                SceneManager.LoadScene("Home");
            }
        }
    }
    IEnumerator CountdownAndFill(float currentTime)
    {
        while (currentTime <= countdownTime)
        {
            float fillAmount = Mathf.Clamp01((countdownTime - currentTime) / countdownTime);

            // ��˹� Fill Amount �ͧ SunMask �ҡ��� fillAmount �����
            sunMaskImage.fillAmount = 1f - fillAmount;

            // �������ҷ���ҹ仵����������� frame
            currentTime += Time.deltaTime;
            yield return null; // �����˹������Ңͧ Unity �����żŵ���
        }

        // ��������ҹѺ�����ѧ���ŧ ��˹� Fill Amount ������
        sunMaskImage.fillAmount = 1f;

    }



}
