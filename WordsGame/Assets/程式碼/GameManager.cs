using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;//引用系統集合、管理API(協同程式:非同步處理)

// UGUI拖動圖片,腳本掛在Image上即可
public class GameManager : MonoBehaviour
{
    public static GameManager instance; //對戰管理實體物件
    public GameObject Name1;
    public GameObject Motion;
    public GameObject Name2;
    public bool  GameOver = false;
    public bool pass = false;
    public static bool GameStart = false;

    public AudioSource aud;
    public AudioClip WalkSound;
    public AudioClip DoorLockSound;
    public AudioClip DoorOpenSound;
    public AudioClip FireSound;
    public AudioClip DeadSound;
    public AudioClip OpenCabinetSound;
    public AudioClip TakeLadderSound;
    public AudioClip ClimbLadderSound;
    public AudioClip GetCardSound;
    public AudioClip InGameSound;

    [Header("卡片欄")]
    public Transform contentCard;
    [Header("名詞卡片物件")]
    public GameObject NamecardObject;
    [Header("動詞卡片物件")]
    public GameObject MotioncardObject;

    public int FirstSantance = 0;
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
    public string WhereIsLadder;
    public string WhereIsMe;

    public void Awake()
    {   
        instance = this;
    }
    public void Start()
    {
        downladder = true;
    }
    public IEnumerator Story()
    {
        if (Name1.GetComponent<BlockFull>().IfBlockfull == true && Motion.GetComponent<BlockFull>().IfBlockfull == true && Name2.GetComponent<BlockFull>().IfBlockfull == true)
        {
            //我移動到門
            if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "門")
            {
                if(downladder == false)
                {
                    string[] wordsText = { "我一心只想移動到門，忘記我還在梯子上，就不小心摔死了..." };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    aud.PlayOneShot(DeadSound, 5f);
                    GameOver = true;
                }
                else
                {
                    if (WhereIsMe != "門")
                    {
                        string[] wordsText = { "我移動到門前" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(WalkSound,5f);
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
                        yield return new WaitForSeconds(1.6f);
                        Transform temp1 = Instantiate(MotioncardObject, contentCard).transform;
                        temp1.Find("字").GetComponent<Text>().text = "下";
                        aud.PlayOneShot(GetCardSound);
                        yield return new WaitForSeconds(8.2f);
                        Transform temp2 = Instantiate(MotioncardObject, contentCard).transform;
                        temp2.Find("字").GetComponent<Text>().text = "開啟";
                        aud.PlayOneShot(GetCardSound);
                        getopen = true;
                    }
                }
            }
            //我開啟門
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "開啟" && Name2.GetComponent<BlockFull>().CurrentText == "門")
            {          
                if(WhereIsMe == "門")
                {
                    if (seekey == false)
                    {
                        string[] wordsText = { "我嘗試開啟，但發現門是被反鎖的" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(DoorLockSound, 5f);
                    }
                    else
                    {
                        if(WhereIsLadder !="門")
                        {
                            string[] wordsText = { "我試著用鑰匙打開門，果然順利的打開了…但似乎有種不好的預感…" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                            pass = true;
                            aud.PlayOneShot(DoorOpenSound, 5f);
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
                    yield return new WaitForSeconds(4f);
                    Transform temp2 = Instantiate(NamecardObject, contentCard).transform;
                    temp2.Find("字").GetComponent<Text>().text = "櫃子";
                    aud.PlayOneShot(GetCardSound);
                    yield return new WaitForSeconds(3.2f);
                    Transform temp3 = Instantiate(NamecardObject, contentCard).transform;
                    temp3.Find("字").GetComponent<Text>().text = "窗戶";
                    aud.PlayOneShot(GetCardSound);
                    getroom = true;
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
                    aud.PlayOneShot(WalkSound, 5f);
                    yield return new WaitForSeconds(2f);
                    aud.PlayOneShot(FireSound, 5f);
                    yield return new WaitForSeconds(3f);
                    aud.PlayOneShot(DeadSound, 5f);
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
                            yield return new WaitForSeconds(4.2f);
                            Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                            temp1.Find("字").GetComponent<Text>().text = "天花板";
                            aud.PlayOneShot(GetCardSound);
                            getceiling = true;
                        }
                    }
                }           
            }
            //我檢視天花板
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "檢視" && Name2.GetComponent<BlockFull>().CurrentText == "天花板")
            {          
                    string[] wordsText = { "我抬頭看著天花板，櫃子上方似乎有個東西再發光…" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();           
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
                        string[] wordsText = { "我打開了櫃子，裡面沒有任何衣服，但有個「梯子」" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(OpenCabinetSound, 5f);
                        if (getladder == false)
                        {
                            yield return new WaitForSeconds(4.4f);
                            Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                            temp1.Find("字").GetComponent<Text>().text = "梯子";
                            aud.PlayOneShot(GetCardSound);
                            getladder = true;
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
                    if(WhereIsMe != "櫃子")
                    {
                        string[] wordsText = { "我離梯子太遠了，無法檢視" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                    }
                    else
                    {
                        string[] wordsText = { "我拿起梯子，發現他和櫃子差不多高" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        aud.PlayOneShot(TakeLadderSound, 5f);
                        if (seeladder == false)
                        {
                            seeladder = true;
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
                                aud.PlayOneShot(TakeLadderSound, 5f);
                                WhereIsLadder = "櫃子";
                                WhereIsMe = "櫃子";
                                if (getclimb == false)
                                {
                                    yield return new WaitForSeconds(4.4f);
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
                                aud.PlayOneShot(TakeLadderSound, 5f);
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
                            aud.PlayOneShot(ClimbLadderSound, 5f);
                            downladder = false;
                            if (getkey == false)
                            {
                                yield return new WaitForSeconds(5.2f);
                                Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
                                temp1.Find("字").GetComponent<Text>().text = "鑰匙";
                                aud.PlayOneShot(GetCardSound);
                                getkey = true;
                            }
                        }
                        else
                        {
                            string[] wordsText = { "爬上梯子後並未發現甚麼東西" };
                            dialogue.instance.words = wordsText;
                            dialogue.instance.StartEffect();
                            downladder = false;
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
                string[] wordsText = { "我拿到了鑰匙，這把鑰匙似乎可以開啟那扇門" };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
                if (seekey == false)
                {
                    seekey = true;
                }
            }
            //我下梯子
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "下" && Name2.GetComponent<BlockFull>().CurrentText == "梯子")
            {
                if (downladder == false)
                {
                    string[] wordsText = { "我慢慢地走下了梯子" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                    aud.PlayOneShot(ClimbLadderSound, 5f);
                    downladder = true;
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
                string[] wordsText = { "床上並沒有發現甚麼東西，除了發霉的棉被和枕頭上的蛆..." };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
            }
            //我移動到床
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "床")
            {
                string[] wordsText = { "我現在不想去那裏，那個床發霉還長蛆" };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
            }
            //梯子移動到床
            else if (Name1.GetComponent<BlockFull>().CurrentText == "梯子" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "床")
            {
                string[] wordsText = { "我現在不想去那裏，那個床發霉還長蛆" };
                dialogue.instance.words = wordsText;
                dialogue.instance.StartEffect();
            }
            //梯子移動到床
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "開始" && Name2.GetComponent<BlockFull>().CurrentText == "遊戲")
            {
                aud.PlayOneShot(InGameSound);
                ManuManager.instance.blackImage.SetActive(true);
                Invoke("GoStart", 3);
            }
            //我移動到梯子
            else if (Name1.GetComponent<BlockFull>().CurrentText == "我" && Motion.GetComponent<BlockFull>().CurrentText == "移動到" && Name2.GetComponent<BlockFull>().CurrentText == "梯子")
            {
                if(Neerladder==true)
                {
                    string[] wordsText = { "我現在就在梯子旁邊" };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
                else
                {
                    if (Neerladder == true)
                    {
                        string[] wordsText = { "我移動到" + WhereIsLadder + "前的梯子" };
                        dialogue.instance.words = wordsText;
                        dialogue.instance.StartEffect();
                        WhereIsMe = WhereIsLadder;
                    }
                }
            }
            //其他可能
            else
            {
                if(GameStart ==true)
                {
                    string[] wordsText = { "你的常識或文法可能產生了一些問題..." };
                    dialogue.instance.words = wordsText;
                    dialogue.instance.StartEffect();
                }
            }
        }
    }
    public void GoStart()
    {
        SceneManager.LoadScene("關卡1");
        GameStart = true;
    }
    public IEnumerator FristCard()
    {
        if(FirstSantance ==1 )
        {
            yield return new WaitForSeconds(0.4f);
            Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
            temp1.Find("字").GetComponent<Text>().text = "我";
            aud.PlayOneShot(GetCardSound);
            yield return new WaitForSeconds(0.8f);
            Transform temp2 = Instantiate(NamecardObject, contentCard).transform;
            temp2.Find("字").GetComponent<Text>().text = "床";
            aud.PlayOneShot(GetCardSound);
            yield return new WaitForSeconds(3.2f);
            Transform temp3 = Instantiate(MotioncardObject, contentCard).transform;
            temp3.Find("字").GetComponent<Text>().text = "移動到";
            aud.PlayOneShot(GetCardSound);
            yield return new WaitForSeconds(0.6f);
            Transform temp4 = Instantiate(NamecardObject, contentCard).transform;
            temp4.Find("字").GetComponent<Text>().text = "房間";
            aud.PlayOneShot(GetCardSound);
        }
        else if (FirstSantance == 2)
        {
            yield return new WaitForSeconds(1.4f);
            Transform temp1 = Instantiate(NamecardObject, contentCard).transform;
            temp1.Find("字").GetComponent<Text>().text = "門";
            aud.PlayOneShot(GetCardSound);
        }
        else if (FirstSantance == 3)
        {
            yield return new WaitForSeconds(1.6f);
            Transform temp1 = Instantiate(MotioncardObject, contentCard).transform;
            temp1.Find("字").GetComponent<Text>().text = "檢視";
            aud.PlayOneShot(GetCardSound);
        }
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Update()
    {
        if(WhereIsLadder == WhereIsMe)
        {
            Neerladder = true;
        }
    }
}