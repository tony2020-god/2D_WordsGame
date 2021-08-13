using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Collections;//引用系統集合、管理API(協同程式:非同步處理)
using System.Collections.Generic; // 系統.集合.一般

// UGUI拖動圖片,腳本掛在Image上即可
public class GameManager : MonoBehaviour
{
    public static GameManager instance; //對戰管理實體物件
    [Header("名詞欄位1")]
    public GameObject Name1;
    [Header("動詞欄位")]
    public GameObject Motion;
    [Header("名詞欄位2")]
    public GameObject Name2;
    [Header("名詞卡片物件")]
    public GameObject NamecardObject;
    [Header("動詞卡片物件")]
    public GameObject MotioncardObject;
    [Header("卡片欄")]
    public Transform contentCard;
    [Header("Leve1閱讀過的字")]
    public static List<String> ReadWordsLevel1 = new List<String>();
    [Header("Leve2閱讀過的字")]
    public static List<String> ReadWordsLevel2 = new List<String>();

    public string WhereIsMe;
    public bool  GameOver = false;
    public bool pass = false;
    public static bool GameStart = false;
    public int FirstSantance = 0;
    public bool StopDia;
    
    #region 音效欄位
    public AudioSource aud;
    public AudioClip WalkSound;
    public AudioClip DoorLockSound;
    public AudioClip DoorOpenSound;
    public AudioClip FireSound;
    public AudioClip DeadSound;
    public AudioClip TakeLadderSound;
    public AudioClip ClimbLadderSound;
    public AudioClip GetCardSound;
    public AudioClip InGameSound;
    public AudioClip ZombeShutSound;
    public AudioClip ZombeRunShutSound;
    public AudioClip ZombeEatSound;
    public AudioClip OpenCabinetSound;
    public AudioClip OpenDrawerSound;
    public AudioClip OpenGateSound;
    public AudioClip OpenIronDoorSound;
    #endregion

    #region 第一章 欄位
    public bool OpenCabinet = false;
    public bool getopen = false;
    public bool getroom = false;
    public bool getceiling = false;
    public bool getladder = false;
    public bool getclimb;
    public bool getkey;
    public bool ladderMoveTocabinet;
    public bool seeladder = false;
    public bool seekey = false;
    public bool downladder = false;
    public bool Neerladder = false;
    public bool KeyOpenDoor = false;
    public string WhereIsLadder;
    #endregion

    #region 第二章 欄位
    public float time = 10;
    public int timer = 10;
    public GameObject defenceText;
    public bool IfAttack;
    public bool OpenIronDoor;
    public bool SeeLivingRoom;
    public bool SeeDesk;
    public bool SeeSofa;
    public bool SeeDrawer;
    public bool OpenDrawer;
    public bool GetGun;
    public bool GetWhiteFlag;
    public bool GetCarpet;
    public bool SeeCarpet;
    public bool GetGate;
    public bool NeerSofa;
    public bool RaiseGun;
    public bool RaiseWhiteFlag;
    public string WhereIsSofa;
    #endregion

    public void Awake()
    {   
        instance = this;
    }

    public void Start()
    {
        downladder = true;
        WhereIsSofa = "鐵門";
        WhereIsLadder = "櫃子";
        time = 20;
    }

    //選單     
    public void InMenu()
    {
        if (Name1.GetComponent<BlockFull>().IfBlockfull == true && Motion.GetComponent<BlockFull>().IfBlockfull == true && Name2.GetComponent<BlockFull>().IfBlockfull == true)
        {
            //我開始第一章
            if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "開始玩" && Name2.GetComponent<BlockFull>().CurrentText == "第一章")
            {
                aud.PlayOneShot(InGameSound);
                ManuManager.instance.blackImage.SetActive(true);
                Invoke("GoStart", 3);
                Level.WhichLevel = 1;
            }
            //我開始第二章
            if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "開始玩" && Name2.GetComponent<BlockFull>().CurrentText == "第二章")
            {
                aud.PlayOneShot(InGameSound);
                ManuManager.instance.blackImage.SetActive(true);
                Invoke("GoStart", 3);
                Level.WhichLevel = 2;
            }
        } 
    }

    //第一章 遊戲內容
    public IEnumerator Level1()
    {
        if (Name1.GetComponent<BlockFull>().IfBlockfull == true && Motion.GetComponent<BlockFull>().IfBlockfull == true && Name2.GetComponent<BlockFull>().IfBlockfull == true)
        {
            dialogue.instance.end = false;
            dialogue.instance.strindex = 0;
            //我移動到門
            if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "門")
            {
                if(downladder == false)
                {
                    string[] wordsText = { "我一心只想移動到門，忘記我還在梯子上，就不小心摔死了..." };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    aud.PlayOneShot(DeadSound, 1f);
                    GameOver = true;
                }
                else
                {
                    if (WhereIsMe != "門")
                    {
                        string[] wordsText = { "我移動到門前" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(WalkSound,1f);
                        WhereIsMe = "門";
                    }
                    else
                    {
                        string[] wordsText = { "我已經在門前了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                }             
            }
            //我檢視門
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "門")
            {
                if (WhereIsMe !="門")
                {
                    string[] wordsText = { "門太遠了，我無法檢視" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    string[] wordsText = { "我稍微檢視一「下」門，喇叭鎖的設計有點奇怪，不知道為什麼鑰匙孔是朝向房間的…而且門似乎可以被「開啟」" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    if (getopen == false)
                    {
                        getopen = true;
                        yield return WaitForSecondsOrKeyPress(1.6f, true);
                        Transform temp1 = Instantiate(MotioncardObject, contentCard).transform;
                        temp1.Find("字").GetComponent<Text>().text = "下";
                        aud.PlayOneShot(GetCardSound);
                        yield return WaitForSecondsOrKeyPress(8.2f, false);
                        Transform temp2 = Instantiate(MotioncardObject, contentCard).transform;
                        temp2.Find("字").GetComponent<Text>().text = "開啟";
                        aud.PlayOneShot(GetCardSound);                       
                    }
                }
            }
            //我開啟門 (結束)
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "開啟" && Name2.GetComponent<BlockFull>().CurrentText == "門")
            {          
                if(WhereIsMe == "門")
                {
                    if (KeyOpenDoor == false)
                    {
                        string[] wordsText = { "我嘗試開啟，但發現門是被反鎖的" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(DoorLockSound, 1f);
                    }
                    else
                    {
                        if(WhereIsLadder !="門")
                        {
                            string[] wordsText = { "開鎖之後，門順利地被我打開…但似乎有種不好的預感…" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                            pass = true;
                            Level.IfFinishLevel1 = true;
                            aud.PlayOneShot(DoorOpenSound, 5f);
                            yield return WaitForSecondsOrKeyPress(10f, false);
                            SceneManager.LoadScene("選單");
                        }
                        else
                        {
                            string[] wordsText = { "梯子卡在門前，我無法開門" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                        }
                    }
                }
                else
                {
                    string[] wordsText = { "我離門太遠了，無法開啟" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //鑰匙開啟門
            else if (Name1.GetComponent<BlockFull>().CurrentText == "鑰匙" && Motion.GetComponent<BlockFull>().CurrentText == "開啟" && Name2.GetComponent<BlockFull>().CurrentText == "門")
            {
                if (WhereIsMe == "門")
                {
                    if (seekey == false)
                    {
                        string[] wordsText = { "我尚未取得鑰匙" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(DoorLockSound, 1f);
                    }
                    else
                    {
                        if (WhereIsLadder != "門")
                        {
                            KeyOpenDoor = true;
                            string[] wordsText = { "我試著用鑰匙打開門門鎖，果然順利的解鎖了" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                            aud.PlayOneShot(DoorLockSound, 1f);                     
                        }
                        else
                        {
                            string[] wordsText = { "梯子卡在門前，我無法開門" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                        }
                    }
                }
                else
                {
                    string[] wordsText = { "我離門太遠了，無法開啟" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //我檢視房間
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "房間")
            {
                string[] wordsText = { "我稍微觀察一下房間，房間內只有床和「櫃子」，在床旁邊的牆上則有一扇「窗戶」" };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
                if (getroom == false)
                {
                    getroom = true;
                    yield return WaitForSecondsOrKeyPress(4f, true);
                    Transform temp2 = Instantiate(NamecardObject, contentCard).transform;
                    temp2.Find("字").GetComponent<Text>().text = "櫃子";
                    aud.PlayOneShot(GetCardSound);
                    yield return WaitForSecondsOrKeyPress(3.2f, false);
                    Transform temp3 = Instantiate(NamecardObject, contentCard).transform;
                    temp3.Find("字").GetComponent<Text>().text = "窗戶";
                    aud.PlayOneShot(GetCardSound);                    
                }
            }
            //我移動到櫃子
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "櫃子")
            {
                if (downladder == false)
                {
                    string[] wordsText = { "我已經在櫃子旁邊了" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    if (WhereIsMe != "櫃子")
                    {
                        string[] wordsText = { "我移動到櫃子前" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(WalkSound, 5f);
                        WhereIsMe = "櫃子";
                    }
                    else
                    {
                        string[] wordsText = { "我已經在櫃子前了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                }       
            }
            //我移動到房間
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "房間")
            {            
                    string[] wordsText = { "我本來就在房間裡面" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();             
            }
            //我移動到窗戶
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "窗戶")
            {
                if (downladder == false)
                {
                    string[] wordsText = { "我一心只想移動到門，忘記我還在梯子上，就不小心摔死了..." };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    aud.PlayOneShot(DeadSound, 5f);
                    GameOver = true;
                }
                else
                {
                    string[] wordsText = { "當我移動到窗戶後，看到底下有人便向他求救，卻不幸被遠方的狙擊手射穿了腦袋...." };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    aud.PlayOneShot(WalkSound, 1f);
                    yield return WaitForSecondsOrKeyPress(2f, true);
                    aud.PlayOneShot(FireSound, 1f);
                    yield return WaitForSecondsOrKeyPress(3f, false);
                    aud.PlayOneShot(DeadSound, 1f);
                    GameOver = true;
                }           
            }
            //我檢視窗戶
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "窗戶")
            {
                string[] wordsText = { "離窗戶太遠無法檢視" };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
            }
            //我爬上窗戶
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "爬上" && Name2.GetComponent<BlockFull>().CurrentText == "窗戶")
            {
                string[] wordsText = { "離窗戶太遠無法爬上" };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
            }
            //我開啟窗戶
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "開啟" && Name2.GetComponent<BlockFull>().CurrentText == "窗戶")
            {
                string[] wordsText = { "離窗戶太遠無法開啟" };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
            }
            //我檢視櫃子
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "櫃子")
            {
                if (downladder == false)
                {
                    string[] wordsText = { "我還在梯子上，要想辦法下去" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    if (WhereIsMe != "櫃子")
                    {
                        string[] wordsText = { "櫃子太遠，我檢視不到" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "看起來就是個簡單的櫃子，櫃子上方和「天花板」之間有個空隙，感覺櫃子裡面似乎會放著什麼" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        if (getceiling == false)
                        {
                            getceiling = true;
                            yield return WaitForSecondsOrKeyPress(4.2f, false);
                            Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                            temp1.Find("字").GetComponent<Text>().text = "天花板";
                            aud.PlayOneShot(GetCardSound);
                        }
                    }
                }           
            }
            //我檢視天花板
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "天花板")
            {      
                if(seekey == false)
                {
                    string[] wordsText = { "我抬頭看著天花板，櫃子上方似乎有個東西在發光…" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    string[] wordsText = { "天花板似乎沒甚麼東西了" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                } 
            }
            //我開啟櫃子
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "開啟" && Name2.GetComponent<BlockFull>().CurrentText == "櫃子")
            {
                if (downladder == false)
                {
                    string[] wordsText = { "我懶得下梯子，想直接把櫃子打開，不料腳滑，我就摔死了..." };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    
                    if (OpenCabinet == false)
                    {
                        if(WhereIsMe == "櫃子")
                        {
                            string[] wordsText = { "我打開了櫃子，裡面沒有任何衣服，但有個「梯子」" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                            aud.PlayOneShot(OpenCabinetSound, 1f);
                            if (getladder == false)
                            {
                                getladder = true;
                                yield return WaitForSecondsOrKeyPress(4.4f, false);
                                Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                                temp1.Find("字").GetComponent<Text>().text = "梯子";
                                aud.PlayOneShot(GetCardSound);
                            }
                        }
                        else
                        {
                            string[] wordsText = { "我離櫃子太遠開啟不了" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                        }
                    }
                    else
                    {
                        string[] wordsText = { "櫃子已經開啟" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                }                    
            }
            //我檢視梯子
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "梯子")
            {
                if (downladder == false)
                {
                    string[] wordsText = { "我剛剛用過了梯子，感覺沒甚麼問題" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    if(Neerladder == false)
                    {
                        string[] wordsText = { "我離梯子太遠了，無法檢視" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        if (seeladder == false)
                        {                           
                            seeladder = true;
                            string[] wordsText = { "我拿起梯子，放在一旁的空地，發現他和櫃子差不多高" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                            aud.PlayOneShot(TakeLadderSound, 5f);
                            WhereIsLadder = "空地";
                            WhereIsMe = "空地";
                        }
                        else
                        {
                            string[] wordsText = { "這個梯子和櫃子差不多高" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                        }
                    }
                }         
            }
            //梯子移動到櫃子
            else if (Name1.GetComponent<BlockFull>().CurrentText == "梯子" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "櫃子")
            {
                if (seeladder == false)
                {
                    string[] wordsText = { "梯子還在櫃子裡面，不能移動" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {              
                    if (WhereIsLadder !="櫃子")
                     {
                          if (downladder ==false)
                           {
                            string[] wordsText = { "我在梯子上，無法移動梯子" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                           }
                          else
                           {
                            if (Neerladder == true)
                            {
                                string[] wordsText = { "我將拿在手上的梯子放在櫃子前，似乎可以「爬上」去" };
                                dialogue.instance.words = wordsText;
                                dialogue.instance.StartEffect();
                                aud.PlayOneShot(TakeLadderSound, 1f);
                                WhereIsLadder = "櫃子";
                                WhereIsMe = "櫃子";
                                if (getclimb == false)
                                {
                                    yield return WaitForSecondsOrKeyPress(4.4f, false);
                                    Transform temp1 = Instantiate(MotioncardObject, contentCard).transform;
                                    temp1.Find("字").GetComponent<Text>().text = "爬上";
                                    aud.PlayOneShot(GetCardSound);
                                    getclimb = true;
                                }
                            }
                            else
                            {
                                string[] wordsText = { "梯子離我太遠，我無法移動" };
                                dialogue.instance.words = wordsText;
                                dialogue.instance.StartEffect();
                            }
                          }                                                                                                                       
                     }
                    else
                    {
                        string[] wordsText = { "梯子已經在櫃子旁邊" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }                
                }
            }
            //梯子移動到門
            else if (Name1.GetComponent<BlockFull>().CurrentText == "梯子" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "門")
            {
                if (seeladder == false)
                {
                    string[] wordsText = { "梯子還在櫃子裡面，不能移動" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {                  
                        if (WhereIsLadder != "門")
                        {
                          if(downladder == false)
                           {
                            string[] wordsText = { "我在梯子上，無法移動梯子" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                           }
                           else
                           {
                            if (Neerladder == true)
                            {
                                string[] wordsText = { "我將梯子移動到門前" };
                                dialogue.instance.words = wordsText;
                                dialogue.instance.StartEffect();
                                aud.PlayOneShot(TakeLadderSound, 1f);
                                WhereIsLadder = "門";
                                WhereIsMe = "門";
                            }
                            else
                             {
                                string[] wordsText = { "梯子太遠，我無法移動" };
                                dialogue.instance.words = wordsText;
                                dialogue.instance.StartEffect();
                             }
                            }
                        }
                        else
                        {
                            string[] wordsText = { "梯子已經在門旁邊" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                        }                                                                               
                }
            }
            //梯子移動到房間
            else if (Name1.GetComponent<BlockFull>().CurrentText == "梯子" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "房間")
            {             
                    string[] wordsText = { "梯子本來就在房間裡面" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();                          
            }
            //我爬上梯子
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "爬上" && Name2.GetComponent<BlockFull>().CurrentText == "梯子")
            {
                if (downladder == false)
                {
                    string[] wordsText = { "我已經在梯子上了" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    if (Neerladder == false)
                    {
                        string[] wordsText = { "梯子太遠，我爬不到" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        if(WhereIsMe =="櫃子")
                        {
                            string[] wordsText = { "我爬上梯子離天花板又更靠近了，櫃子上有個發光的「鑰匙」" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                            aud.PlayOneShot(ClimbLadderSound, 1f);
                            downladder = false;
                            if (getkey == false)
                            {
                                yield return WaitForSecondsOrKeyPress(5.2f, false);
                                Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                                temp1.Find("字").GetComponent<Text>().text = "鑰匙";
                                aud.PlayOneShot(GetCardSound);
                                getkey = true;
                            }
                        }
                        else
                        {
                            downladder = false;
                            string[] wordsText = { "爬上梯子後並未發現甚麼東西" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                        }                     
                    }                   
                }              
            }
            //我爬上櫃子
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "爬上" && Name2.GetComponent<BlockFull>().CurrentText == "櫃子")
            {            
                if(downladder == false)
                {
                    string[] wordsText = { "上面都是蛆，我才不要爬到櫃子上" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    string[] wordsText = { "櫃子太高了爬不上去" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                } 
            }
            //我檢視鑰匙
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "鑰匙")
            {
               if(downladder == false)
                {
                    if (WhereIsLadder =="櫃子")
                    {
                        string[] wordsText = { "我拿到了鑰匙，這把鑰匙似乎可以開啟那扇門" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        if (seekey == false)
                        {
                            seekey = true;
                        }
                    }
                    else
                    {
                        string[] wordsText = { "鑰匙太遠我檢視不到" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                }
               else
                {
                    string[] wordsText = { "鑰匙太遠我檢視不到" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //我下梯子
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "下" && Name2.GetComponent<BlockFull>().CurrentText == "梯子")
            {
                if (downladder == false)
                {        
                    downladder = true;
                    string[] wordsText = { "我慢慢地走下了梯子" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    aud.PlayOneShot(ClimbLadderSound, 1f);
                }
                else
                {
                    string[] wordsText = { "我本來就在梯子下" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }          
            }
            //我下床
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "下" && Name2.GetComponent<BlockFull>().CurrentText == "床")
            {              
                    string[] wordsText = { "我本來就下床了" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();           
            }
            //我檢視床
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "床")
            {
                string[] wordsText = { "床上並沒有發現甚麼東西，除了發臭的棉被和枕頭上的蛆..." };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
            }
            //我移動到床
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "床")
            {
                string[] wordsText = { "我現在不想去那裏，那個床發臭還長蛆" };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
            }
            //梯子移動到床
            else if (Name1.GetComponent<BlockFull>().CurrentText == "梯子" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "床")
            {
                string[] wordsText = { "我現在不想去那裏，那個床發臭還長蛆" };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
            }
            //我移動到梯子
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "梯子")
            {
                    if (seeladder == true)
                    {
                        if (Neerladder == true)
                        {
                            string[] wordsText = { "我現在就在梯子旁邊" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                        }
                        else
                        {
                            string[] wordsText = { "我移動到" + WhereIsLadder + "前的梯子" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                            WhereIsMe = WhereIsLadder;
                        }
                    }
                    else
                    {
                        string[] wordsText = { "梯子還在櫃子裡面，我必須要先拿出來" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }                  
            }
            //其他可能
            else
            {
                if(GameStart ==true)
                {
                    string[] wordsText = { "你不可以這麼做..." };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
        }
    }

    //第二章 遊戲內容
    public IEnumerator Level2()
    {
        if (Name1.GetComponent<BlockFull>().IfBlockfull == true && Motion.GetComponent<BlockFull>().IfBlockfull == true && Name2.GetComponent<BlockFull>().IfBlockfull == true)
        {
            dialogue.instance.end = false;
            dialogue.instance.strindex = 0;
            //我檢視客廳
            if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "客廳")
            {
                if (SeeLivingRoom == false)
                {
                    SeeLivingRoom = true;
                    string[] wordsText = { "客廳中只看到「書桌」、「沙發」和沙發後的一扇[鐵門]，雖然有些距離，但我可以「移動到」這些地方" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    yield return WaitForSecondsOrKeyPress(1.8f, true);
                    Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                    temp1.Find("字").GetComponent<Text>().text = "書桌";
                    aud.PlayOneShot(GetCardSound);
                    yield return WaitForSecondsOrKeyPress(1f, true);
                    Transform temp2 = Instantiate(NamecardObject, contentCard).transform;
                    temp2.Find("字").GetComponent<Text>().text = "沙發";
                    aud.PlayOneShot(GetCardSound);
                    yield return WaitForSecondsOrKeyPress(2.2f, true);
                    Transform temp3 = Instantiate(NamecardObject, contentCard).transform;
                    temp3.Find("字").GetComponent<Text>().text = "鐵門";
                    aud.PlayOneShot(GetCardSound);
                    yield return WaitForSecondsOrKeyPress(3.4f, false);
                    Transform temp4 = Instantiate(MotioncardObject, contentCard).transform;
                    temp4.Find("字").GetComponent<Text>().text = "移動到";
                    aud.PlayOneShot(GetCardSound);
                }
                else
                {
                    string[] wordsText = { "客廳中只看到書桌、沙發和一扇鐵門" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //我移動到客廳
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "客廳")
            {
                    string[] wordsText = { "我本來就在客廳裏面" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();                
            }
            //我移動到書桌
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "書桌")
            {
                if (IfAttack == false && OpenIronDoor == true)
                {
                    string[] wordsText = { "想移動到書桌但是腳不聽使喚" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    if (WhereIsMe == "書桌")
                    {
                        string[] wordsText = { "我已經在書桌前面了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "我走到了書桌前" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(WalkSound, 1f);
                        WhereIsMe = "書桌";
                    }
                }
            }
            //我檢視書桌
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "書桌")
            {
                if (WhereIsMe == "書桌")
                {
                    string[] wordsText = { "書桌下方有個「抽屜」，而桌上則有一些破損的「報紙」" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    if (SeeDesk == false)
                    {         
                        SeeDesk = true;
                        yield return WaitForSecondsOrKeyPress(1.8f, true);
                        Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                        temp1.Find("字").GetComponent<Text>().text = "抽屜";
                        aud.PlayOneShot(GetCardSound);
                        yield return WaitForSecondsOrKeyPress(2.8f, false);
                        Transform temp2 = Instantiate(NamecardObject, contentCard).transform;
                        temp2.Find("字").GetComponent<Text>().text = "報紙";
                        aud.PlayOneShot(GetCardSound);
                    }
                }
                else
                {
                    string[] wordsText = { "書桌太遠，我檢視不到" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //我檢視抽屜
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "抽屜")
            {
                if (WhereIsMe == "書桌")
                {
                    string[] wordsText = { "抽屜似乎可以被「開啟」" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    if (SeeDrawer == false)
                    {
                        SeeDrawer = true;
                        yield return WaitForSecondsOrKeyPress(2f, false);
                        Transform temp1 = Instantiate(MotioncardObject, contentCard).transform;
                        temp1.Find("字").GetComponent<Text>().text = "開啟";
                        aud.PlayOneShot(GetCardSound);
                    }
                }
                else
                {
                    string[] wordsText = { "離抽屜太遠，我檢視不到" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //我開啟抽屜
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "開啟" && Name2.GetComponent<BlockFull>().CurrentText == "抽屜")
            {
                if (WhereIsMe == "書桌")
                {
                    if (OpenDrawer == false)
                    {
                        OpenDrawer = true;
                        string[] wordsText = { "抽屜裡只有一面「白旗」" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(OpenDrawerSound);
                        yield return WaitForSecondsOrKeyPress(2f, false);
                        Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                        temp1.Find("字").GetComponent<Text>().text = "白旗";
                        aud.PlayOneShot(GetCardSound);
                    }
                    else
                    {
                        string[] wordsText = { "抽屜已經開啟了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                }
                else
                {
                    string[] wordsText = { "離抽屜太遠，我開啟不了" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //我移動到沙發
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "沙發")
            {
                if (IfAttack == false && OpenIronDoor == true)
                {
                    string[] wordsText = { "想移動到沙發但是腳不聽使喚" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    if (NeerSofa == true)
                    {
                        string[] wordsText = { "我已經在沙發前面了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "我走到了沙發前" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(WalkSound, 1f);
                        WhereIsMe = WhereIsSofa;
                    }
                }
            }
            //我檢視沙發
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "沙發")
            {
                if (NeerSofa == true)
                {
                    if (GetGun == false)
                    {
                        string[] wordsText = { "我發現坐墊中的縫隙藏著一支「手槍」" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        if (SeeSofa == false)
                        {
                            SeeSofa = true;
                            yield return WaitForSecondsOrKeyPress(3.2f, false);
                            Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                            temp1.Find("字").GetComponent<Text>().text = "手槍";
                            aud.PlayOneShot(GetCardSound);
                        }
                    }
                    else
                    {
                        if (WhereIsSofa == "鐵門")
                        {
                            string[] wordsText = { "沙發上沒有任何東西，只看到沙發擋在鐵門前面，這樣門根本開不了" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                        }
                        else
                        {
                            string[] wordsText = { "沙發上沒有任何東西" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                        }
                    }
                }
                else
                {
                    string[] wordsText = { "沙發太遠，我檢視不到" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //我檢視白旗
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "白旗")
            {
                if (GetWhiteFlag == false)
                {
                    if (WhereIsMe == "書桌")
                    {
                        GetWhiteFlag = true;
                        string[] wordsText = { "有點髒掉的白旗看上去有點奇怪，但以防萬一我還是塞進了口袋" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "手上沒有白旗，無法檢視" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                }
                else
                {
                    string[] wordsText = { "這白旗上沾有一點血跡" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //我檢視報紙
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "報紙")
            {
                if (WhereIsMe == "書桌")
                {
                    string[] wordsText = { "我看了一下報紙，有個標題為<高舉白旗的倖存者>著實令我深感興趣，但內容已經被撕掉了…" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    string[] wordsText = { "報紙太遠，我檢視不到" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //我檢視手槍
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "手槍")
            {
                if (GetGun == false)
                {
                    if (NeerSofa ==true)
                    {
                        GetGun = true;
                        string[] wordsText = { "我拿起了手槍以備不時之需，裡面還剩3發子彈" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "手槍不在我的視野範圍內" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                }
                else
                {
                    string[] wordsText = { "這是一支1997年的手槍，製造者是Tony" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //沙發移動到書桌
            else if (Name1.GetComponent<BlockFull>().CurrentText == "沙發" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "書桌")
            {
                if (NeerSofa == true)
                {
                    if (WhereIsSofa == "書桌")
                    {
                        string[] wordsText = { "沙發已經在書桌旁了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "我將沙發推到書桌旁，發現了門前面有一片[地毯]" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(TakeLadderSound);
                        WhereIsSofa = "書桌";
                        WhereIsMe = "書桌";
                        if (GetCarpet == false)
                        {
                            GetCarpet = true;
                            yield return WaitForSecondsOrKeyPress(4.4f, false);
                            Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                            temp1.Find("字").GetComponent<Text>().text = "地毯";
                            aud.PlayOneShot(GetCardSound);
                        }
                    }
                }
                else
                {
                    string[] wordsText = { "我離沙發太遠沒辦法移動它" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //沙發移動到鐵門
            else if (Name1.GetComponent<BlockFull>().CurrentText == "沙發" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "鐵門")
            {
                if (NeerSofa == true)
                {
                    if (WhereIsSofa == "鐵門")
                    {
                        string[] wordsText = { "沙發已經在鐵門前了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "我將沙發推到鐵門前" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(TakeLadderSound);
                        WhereIsSofa = "鐵門";
                        WhereIsMe = "鐵門";
                    }
                }
                else
                {
                    string[] wordsText = { "我離沙發太遠沒辦法移動它" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //沙發移動到客廳
            else if (Name1.GetComponent<BlockFull>().CurrentText == "沙發" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "客廳")
            {
                string[] wordsText = { "沙發已經在客廳裡了" };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
            }
            //沙發移動到地毯
            else if (Name1.GetComponent<BlockFull>().CurrentText == "沙發" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "地毯")
            {
                if (NeerSofa == true)
                {
                    if (WhereIsSofa == "鐵門")
                    {
                        string[] wordsText = { "沙發已經在地毯上了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "我將沙發推到鐵門前的地毯上" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(TakeLadderSound);
                        WhereIsSofa = "鐵門";
                        WhereIsMe = "鐵門";
                    }
                }
                else
                {
                    string[] wordsText = { "我離沙發太遠沒辦法移動它" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //沙發移動到大門
            else if (Name1.GetComponent<BlockFull>().CurrentText == "沙發" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "大門")
            {           
                        string[] wordsText = { "距離太遠了，我懶得推過去" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();                 
            }
            //我移動到地毯
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "地毯")
            {
                if (WhereIsMe != "鐵門" && WhereIsSofa != "鐵門")
                {
                    string[] wordsText = { "我移動到地毯上" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    WhereIsMe = "鐵門";
                }
                else if (WhereIsMe == "鐵門")
                {
                    if (WhereIsSofa == "鐵門")
                    {
                        string[] wordsText = { "地毯被壓在沙發下，我無法移動到上面" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "我已經在地毯上了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                }
            }
            //我檢視地毯
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "地毯")
            {
                if (WhereIsMe == "鐵門" && WhereIsSofa != "鐵門")
                {
                    string[] wordsText = { "地毯上有那張被撕下的報紙內容: 我們無法從外觀上立刻分辨出是人或是[喪屍]，目前只能用[舉起]白旗來分辨，沒有白旗一律射殺!而要[殺死]喪屍需要攻擊他們的頭部" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    if (SeeCarpet == false)
                    {
                        SeeCarpet = true;
                        yield return WaitForSecondsOrKeyPress(7.2f, true);
                        Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                        temp1.Find("字").GetComponent<Text>().text = "喪屍";
                        aud.PlayOneShot(GetCardSound);
                        yield return WaitForSecondsOrKeyPress(2f, true);
                        Transform temp2 = Instantiate(MotioncardObject, contentCard).transform;
                        temp2.Find("字").GetComponent<Text>().text = "舉起";
                        aud.PlayOneShot(GetCardSound);
                        yield return WaitForSecondsOrKeyPress(4.2f, false);
                        Transform temp3 = Instantiate(MotioncardObject, contentCard).transform;
                        temp3.Find("字").GetComponent<Text>().text = "殺死";
                        aud.PlayOneShot(GetCardSound);
                    }
                }
                else if (WhereIsMe == "鐵門" && WhereIsSofa == "鐵門")
                {        
                        string[] wordsText = { "地毯被壓在沙發下 我無法檢視" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();                   
                }
                else if (WhereIsMe != "鐵門")
                {
                    string[] wordsText = { "我離地毯太遠無法檢視" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //我移動到鐵門
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "鐵門")
            {
                if (IfAttack == false && OpenIronDoor == true)
                {
                    string[] wordsText = { "我已經在鐵門前面了" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    if (WhereIsSofa == "鐵門" && WhereIsMe == "沙發")
                    {
                        string[] wordsText = { "我已經在鐵門前面了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        if (WhereIsMe == "鐵門")
                        {
                            string[] wordsText = { "我已經在鐵門前面了" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                        }
                        else
                        {
                            string[] wordsText = { "我走到了鐵門前" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                            aud.PlayOneShot(WalkSound, 1f);
                            WhereIsMe = "鐵門";
                        }
                    }
                }
            }
            //我檢視鐵門
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "鐵門")
            {
                if (WhereIsSofa == "鐵門" && WhereIsMe == "沙發")
                {
                    string[] wordsText = { "鐵門看起來是已經被打開的，上面有著明顯的撬開痕跡，裡面還有微微的低吼聲!?" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    aud.PlayOneShot(ZombeShutSound);
                }
                else
                {
                    if (WhereIsMe == "鐵門")
                    {
                        string[] wordsText = { "鐵門看起來是已經被打開的，上面有著明顯的撬開痕跡，裡面似乎有動物的低吼聲!?" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(ZombeShutSound);
                    }
                    else
                    {
                        string[] wordsText = { "鐵門太遠，我檢視不到" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                }
            }
            //我開啟鐵門 (倒數)
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "開啟" && Name2.GetComponent<BlockFull>().CurrentText == "鐵門")
            {
                if (OpenIronDoor == false)
                {
                    if (WhereIsSofa == "鐵門")
                    {
                        string[] wordsText = { "沙發卡在門前，我開啟不了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "開門後，看到一個人朝我衝了過來，正想呼救，仔細一看卻是喪屍!!" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(OpenIronDoorSound);
                        yield return WaitForSecondsOrKeyPress(3.8f, true);
                        aud.PlayOneShot(ZombeRunShutSound);
                        defenceText.SetActive(true);
                        defenceText.GetComponent<Text>().text = "離被喪屍抓到還有 " + time + " 秒";
                        yield return WaitForSecondsOrKeyPress(2.2f, false);
                        OpenIronDoor = true;
                    }
                }
                else
                {
                    string[] wordsText = { "鐵門已經開啟" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //我舉起手槍
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "舉起" && Name2.GetComponent<BlockFull>().CurrentText == "手槍")
            {
              if(RaiseGun ==true)
                {
                    string[] wordsText = { "我已經舉起手槍了" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
              else
                {
                    if (GetGun == false)
                    {
                        string[] wordsText = { "我尚未取得手槍" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        RaiseGun = true;
                        string[] wordsText = { "我舉起了手槍" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();

                    }
                }             
            }
            //手槍殺死喪屍
            else if (Name1.GetComponent<BlockFull>().CurrentText == "手槍" && Motion.GetComponent<BlockFull>().CurrentText == "殺死" && Name2.GetComponent<BlockFull>().CurrentText == "喪屍")
            {
              if(GetGun == false)
                {
                    string[] wordsText = { "我尚未取得手槍" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
              else
                {
                    if (RaiseGun == false)
                    {
                        string[] wordsText = { "我要先舉起手槍!" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        if (IfAttack == false && OpenIronDoor == true)
                        {
                            string[] wordsText = { "我拿著槍殺死了喪屍，驚恐中我看了喪屍的長相...是和我一起喝酒的那位朋友，我在他身上找到了一張[地圖]" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                            aud.PlayOneShot(FireSound);
                            IfAttack = true;
                            defenceText.SetActive(false);
                            yield return WaitForSecondsOrKeyPress(1f, true);
                            aud.PlayOneShot(DeadSound);
                            yield return WaitForSecondsOrKeyPress(8.8f, false);
                            Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                            temp1.Find("字").GetComponent<Text>().text = "地圖";
                            aud.PlayOneShot(GetCardSound);
                        }
                        else
                        {
                            string[] wordsText = { "這裡並沒看到喪屍" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                        }
                    }
                }
            }
            //手槍殺死我
            else if (Name1.GetComponent<BlockFull>().CurrentText == "手槍" && Motion.GetComponent<BlockFull>().CurrentText == "殺死" && Name2.GetComponent<BlockFull>().CurrentText == "我")
            {
                if (GetGun == false)
                {
                    string[] wordsText = { "我尚未取得手槍" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    if (RaiseGun == false)
                    {
                        string[] wordsText = { "我要先舉起手槍!" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {                       
                        string[] wordsText = { "我覺得這個世界好難，所以我自殺了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();              
                        aud.PlayOneShot(FireSound);
                        yield return WaitForSecondsOrKeyPress(1f, false);
                        aud.PlayOneShot(DeadSound);
                        GameOver = true;
                    }
                }
            }
            //我殺死喪屍
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "殺死" && Name2.GetComponent<BlockFull>().CurrentText == "喪屍")
            {              
                        if (IfAttack == false && OpenIronDoor == true)
                        {
                            string[] wordsText = { "我要用手槍才能殺死喪屍" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();                        
                        }
                        else
                        {
                            string[] wordsText = { "這裡並沒看到喪屍" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                        }                  
            }
            //我檢視喪屍
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "喪屍")
            {
               if (OpenIronDoor ==false)
                {
                    string[] wordsText = { "這裡並沒有看到喪屍" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
               else
                {
                    if (IfAttack == false)
                    {
                        string[] wordsText = { "它看起來好面熟" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "朋友，請你安息吧" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                }         
            }
            //我檢視地圖
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "地圖")
            {
                string[] wordsText = { "我看了一下地圖，前方似乎有一扇通往外面的[大門]" };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
                if(GetGate == false)
                {
                    GetGate = true;
                    yield return WaitForSecondsOrKeyPress(4.6f, false);
                    Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                    temp1.Find("字").GetComponent<Text>().text = "大門";
                    aud.PlayOneShot(GetCardSound);
                }           
            }
            //我移動到大門
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "大門")
            {          
                    if (WhereIsMe == "大門")
                    {
                        string[] wordsText = { "我已經在大門前面了" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "我走到了大門前" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(WalkSound, 1f);
                        WhereIsMe = "大門";
                    }              
            }
            //我舉起白旗
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "舉起" && Name2.GetComponent<BlockFull>().CurrentText == "白旗")
            {
                if(RaiseWhiteFlag ==true)
                {
                    string[] wordsText = { "我已經舉起白旗了" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    if (GetWhiteFlag == false)
                    {
                        string[] wordsText = { "我尚未拿到白旗" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        RaiseWhiteFlag = true;
                        string[] wordsText = { "我舉起白旗" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                }  
            }
            //我檢視大門
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "大門")
            {
                if (WhereIsMe == "大門")
                {
                    string[] wordsText = { "大門沒上鎖，可以隨時打開" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    string[] wordsText = { "大門太遠，我檢視不到" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //我開啟大門 (結束)
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "開啟" && Name2.GetComponent<BlockFull>().CurrentText == "大門")
            {
                if (WhereIsMe == "大門")
                {
                    if(RaiseWhiteFlag ==false)
                    {
                        string[] wordsText = { "我打開了大門，但是由於我沒有舉白旗，被遠方的狙擊手誤以為是喪屍而射殺了..." };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(OpenGateSound, 1f);
                        yield return WaitForSecondsOrKeyPress(2f, true);
                        aud.PlayOneShot(FireSound, 1f);
                        yield return WaitForSecondsOrKeyPress(3f, false);
                        aud.PlayOneShot(DeadSound, 1f);
                        GameOver = true;
                    }
                    else
                    {
                        string[] wordsText = { "打開門後，幾名特工確認我為倖存者，便接送我去安全區，但一切才剛剛開始..." };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        pass = true;
                        Level.IfFinishLevel2 = true;
                        aud.PlayOneShot(OpenGateSound, 5f);
                        yield return WaitForSecondsOrKeyPress(10f, false);
                        SceneManager.LoadScene("選單");
                    }
                }
                else
                {
                    string[] wordsText = { "大門太遠，我開啟不了" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
            //其他可能
            else
            {
                if (GameStart == true)
                {
                    string[] wordsText = { "你不可以這麼做..." };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
        }
    }

    //進入關卡
    public void GoStart()
    {
        if (Level.WhichLevel == 1) SceneManager.LoadScene("關卡1");
        else if (Level.WhichLevel == 2) SceneManager.LoadScene("關卡2");

        GameStart = true;
    }

    //一開始的文字敘述
    public IEnumerator FristCard()
    {
        //第一關 一開始文字字卡
        if (Level.WhichLevel == 1)
        {     
            if (FirstSantance == 1)
            {
                yield return WaitForSecondsOrKeyPress(0.4f, true);
                Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                temp1.Find("字").GetComponent<Text>().text = "我";
                aud.PlayOneShot(GetCardSound);
                yield return WaitForSecondsOrKeyPress(0.8f,true);
                Transform temp2 = Instantiate(NamecardObject, contentCard).transform;
                temp2.Find("字").GetComponent<Text>().text = "床";
                aud.PlayOneShot(GetCardSound);
                yield return WaitForSecondsOrKeyPress(3.2f, true);
                Transform temp3 = Instantiate(MotioncardObject, contentCard).transform;
                temp3.Find("字").GetComponent<Text>().text = "移動到";
                aud.PlayOneShot(GetCardSound);
                yield return WaitForSecondsOrKeyPress(0.6f, false);
                Transform temp4 = Instantiate(NamecardObject, contentCard).transform;
                temp4.Find("字").GetComponent<Text>().text = "房間";
                aud.PlayOneShot(GetCardSound);
            }
            else if (FirstSantance == 2)
            {
                yield return WaitForSecondsOrKeyPress(0.6f, false);
                Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                temp1.Find("字").GetComponent<Text>().text = "門";
                aud.PlayOneShot(GetCardSound);
            }
            else if (FirstSantance == 3)
            {
                yield return WaitForSecondsOrKeyPress(1.6f, false);
                Transform temp1 = Instantiate(MotioncardObject, contentCard).transform;
                temp1.Find("字").GetComponent<Text>().text = "檢視";
                aud.PlayOneShot(GetCardSound);
            }
        }

        //第二關 一開始文字字卡
        if (Level.WhichLevel == 2)
        {
            if (FirstSantance == 1)
            {
                yield return WaitForSecondsOrKeyPress(0.4f, true);
                Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                temp1.Find("字").GetComponent<Text>().text = "我";
                aud.PlayOneShot(GetCardSound);
                yield return WaitForSecondsOrKeyPress(4f, false);
                Transform temp2 = Instantiate(NamecardObject, contentCard).transform;
                temp2.Find("字").GetComponent<Text>().text = "客廳";
                aud.PlayOneShot(GetCardSound);

            }
            else if (FirstSantance == 2)
            {
                yield return WaitForSecondsOrKeyPress(1.8f, false);
                Transform temp3 = Instantiate(MotioncardObject, contentCard).transform;
                temp3.Find("字").GetComponent<Text>().text = "檢視";
                aud.PlayOneShot(GetCardSound);
            }
        }
    }

    //重新開始
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("選單");
    }

    public void Update()
    {
       if (Level.WhichLevel == 1)
        {
            if (WhereIsLadder == WhereIsMe)
            {
                Neerladder = true;
            }
            else
            {
                Neerladder = false;
            }
        }
       if (Level.WhichLevel == 2)
        {
            if (time <= 0 && GameOver == false)
            {
                string[] wordsText = { "你被喪屍啃食成了碎塊" };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
                aud.PlayOneShot(ZombeEatSound);
                GameOver = true;
            }
            if (time > 0 && IfAttack == false && OpenIronDoor == true)
            {
                time -= Time.deltaTime;
                timer = (int)time;
                defenceText.GetComponent<Text>().text = "離被喪屍抓到還有 " + timer + " 秒";
            }
            if (WhereIsSofa == WhereIsMe)
            {
                NeerSofa = true;
            }
            else
            {
                NeerSofa = false;
            }
        }
    }


    public IEnumerator WaitForSecondsOrKeyPress(float time, bool HaveOtherCard)  //自定 WaitForSecondsOrKeyPress (時間，是否還有字卡)
    {  
        while(StopDia == false && time > 0) 
         {
            time -= Time.deltaTime;                                        //倒數計時
            yield return new WaitForSeconds(0.0001f);                      //每楨執行一次
            if (StopDia == true)                                           //滑鼠傳回的StopDia為true，time設為0
             {
                time = 0;
             }
         }
        if (HaveOtherCard == false)
        {
            StopDia = false;
        }
    }
}
