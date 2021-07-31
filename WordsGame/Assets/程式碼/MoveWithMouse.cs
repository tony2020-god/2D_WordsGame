using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// UGUI拖動圖片,腳本掛在Image上即可
public class MoveWithMouse : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Image img;//實體化后的物件
    Vector3 offPos;//存盤按下滑鼠時的圖片-滑鼠位置差
    Vector3 arragedPos; //保存經過整理后的向量，用于圖片移動
    public bool inblock = false;
    public bool StartinCardBox = true;
    public GameObject Block;
    public GameObject LastBlock;
    public bool inCardBox =true;
    public string CardWords;
    Vector3 originTraslate;
    public GameManager GM;

    public void Start()
    {
        GM = GameObject.Find("遊戲管理器").GetComponent<GameManager>();
        
        CardWords = gameObject.transform.Find("字").GetComponent<Text>().text;
        print(CardWords);
    }
    /// <summary>
    /// 開始拖拽的時候
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.GetComponent<RectTransform>(), Input.mousePosition
     , eventData.enterEventCamera, out arragedPos))
        {
            offPos = transform.position - arragedPos;
            originTraslate = gameObject.transform.position;
        }
    }

    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = offPos + Input.mousePosition;
    }

    /// <summary>
    /// 拖拽結束，圖片停留在結束位置
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = offPos + Input.mousePosition;
        if (inCardBox ==false)
        {
            if(Block.tag == "字卡放置欄")
            {
                transform.position = originTraslate;
            }
            else
            {
                if (Block.GetComponent<BlockFull>().IfBlockfull == false && inblock == true)
                {
                    gameObject.transform.parent = Block.transform;
                    if(LastBlock != null)
                    {
                        if (LastBlock.tag != "字卡放置欄")
                        {
                            LastBlock.GetComponent<BlockFull>().IfBlockfull = false;
                        }
                    }                    
                    LastBlock = Block;
                    Block.GetComponent<BlockFull>().IfBlockfull = true;
                    Block.GetComponent<BlockFull>().CurrentText = CardWords;
                    StartinCardBox = false;
                    if (Level.WhichLevel == 0) GM.InMenu();
                    else if(Level.WhichLevel == 1) StartCoroutine(GM.Level1());
                    else if(Level.WhichLevel == 2) StartCoroutine(GM.Level2());
                    print(Level.WhichLevel);
                }
                else
                {
                    transform.position = originTraslate;
                }
            }           
        }
        else
        {
            if(StartinCardBox == true)
            {
                transform.position = originTraslate;
            }
            else
            {
                StartinCardBox = true;
                if (LastBlock != null)
                {
                    if (LastBlock.tag != "字卡放置欄")
                    {
                        LastBlock.GetComponent<BlockFull>().IfBlockfull = false;
                    }
                }
                LastBlock = Block;
                gameObject.transform.parent = Block.transform;
            }
        }       
    }

    public void OnTriggerEnter2D(Collider2D other)
    {  
         if (other.tag == "名詞欄位" && gameObject.tag=="名詞")
         {
              Block = other.gameObject;
              inblock = true;
         }
         if (other.tag == "動詞欄位" && gameObject.tag == "動詞")
         {
            Block = other.gameObject;
            inblock = true;
        }
        if (other.tag == "字卡放置欄")
        {
            Block = other.gameObject;
            inCardBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "名詞欄位" && gameObject.tag == "名詞")
        {
            inblock = false;
        }
        if (other.tag == "動詞欄位" && gameObject.tag == "動詞")
        {
            inblock = false;
        }
        if (other.tag == "字卡放置欄")
        {
           inCardBox = false;
        }
    }
}