using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;//引用系統集合、管理API(協同程式:非同步處理)

public class dialogue : MonoBehaviour
{
    public static dialogue instance; //對戰管理實體物件
    public float charsPerSecond = 0.2f; //打字時間間隔
    public string[] words; //保存需要顯示的文字
    public int strindex = 0; //控制語句
    private bool isActive;
    private float timer = 0; //計時器
    public Text myText;
    public int currentPos = 0; //當前打字位置
    public bool islongWriting = false;
    public GameObject CannotPointImage;
    public GameObject GameOverImage;
    public GameObject PassImage;
    public AudioSource aud;
    public AudioClip typeSound;
    public bool PlaySound =false;
    public GameObject blackImage;
    public bool HaveSantance;
    public bool GameOverBool;
    public bool PassBool;
    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        if(Level.WhichLevel == 1)
        {
            string[] wordsText = { "哎呀~頭好痛，只記得昨天和朋友喝了幾杯，不小心喝醉了，之後的事完全沒印象...", "[我]從[床]上起來，發現自己已經被[移動到][房間]，我的朋友也不在這個房間，不過這裡為什麼這麼安靜…", "我正對面的[門]好像是唯一的出口", "看來我需要「檢視」一下這個陌生的地方到底是哪裡…" };
            words = wordsText;
        }
        if (Level.WhichLevel == 2)
        {
            string[] wordsText = { "我使用鑰匙打開門後，外面就突然下起了大雨，一道閃電滑過，而眼前的景象把我從逃離的幻想拉回殘酷的現實", "「我」稍微看了一下，這陰暗的房間似乎是「客廳」，看來要好好的[檢視]一下有甚麼有用的情報..." };
            words = wordsText;
        }
        Invoke("StartEffect", 3);
        timer = 0;
    }

    public void Update()
    {
        OnStartWriter();
        if (Input.GetMouseButtonDown(0))
        {
            NextSantance();
            GameOver();
            PassGame();
        }
    }

    public void StartEffect()
    {
        CannotPointImage.SetActive(true);
        isActive = true;
        blackImage.SetActive(false);
    }

    /// 打字
    public void OnStartWriter()
    {
        if (isActive == true)
        {
            HaveSantance = false;
            timer += Time.deltaTime;
            if (timer >= charsPerSecond)
            { //判断計時器時間是否到達
                timer = 0;
                currentPos++;
                if (PlaySound ==false)
                {
                    aud.PlayOneShot(typeSound,5f);
                    PlaySound = true;
                }

                myText.text = words[strindex].Substring(0, currentPos); //刷新文本顯示内容

                if (currentPos >= words[strindex].Length)
                {
                    if (strindex < words.Length - 1)
                    {
                        isActive = false;
                        aud.Stop();
                        PlaySound = false;
                        timer = 0;
                        currentPos = 0;
                        HaveSantance = true;
                        //Invoke("NextSantance", 1.5f);
                    }
                    else
                    {
                        strindex = 0;
                        OnFinish();
                    }
                }
            }
        }
    }
    /// 結束打字，初始化數據
    public void OnFinish()
    {
        isActive = false;
        aud.Stop();
        PlaySound = false;
        HaveSantance = false;
        timer = 0;
        currentPos = 0;
        Invoke("imageCover", 0.5f);
        if (GameManager.instance.GameOver == true) GameOverBool = true;
        if (GameManager.instance.pass == true) PassBool = true;
    }
    public void NextSantance()
    {
        if (HaveSantance == true)
        {
            CannotPointImage.SetActive(true);
            timer = 0;
            currentPos = 0;
            strindex++; //下一句
            GameManager.instance.FirstSantance++;
            StartCoroutine(GameManager.instance.FristCard());
            isActive = true;
        }
    }
    public void imageCover()
    {
        CannotPointImage.SetActive(false);
    }
    public void GameOver()
    {
        if (GameOverBool == true) GameOverImage.SetActive(true);
    }
    public void PassGame()
    {
        if(PassBool == true) PassImage.SetActive(true);
    }
}    
