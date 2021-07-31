using UnityEngine;
using UnityEngine.UI;
using System.Collections;//引用系統集合、管理API(協同程式:非同步處理)

public class ManuManager : MonoBehaviour
{
    public static ManuManager instance; //對戰管理實體物件
    [Header("開場黑色布塊")]
    public GameObject blackImage;
    [Header("卡片欄")]
    public Transform contentCard;
    [Header("名詞卡片物件")]
    public GameObject NamecardObject;
    public AudioSource aud;
    public AudioClip GetCardSound;

    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        Level.WhichLevel = 0;
        GameManager.GameStart = false;
        StartCoroutine(SpawnLevelCard());
    }
    public IEnumerator SpawnLevelCard()
    {
       if(Level.IfFinishLevel1 ==true)
        {
            yield return new WaitForSeconds(1f);
            Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
            temp1.Find("字").GetComponent<Text>().text = "第二章";
            aud.PlayOneShot(GetCardSound);
        }
    }
}
