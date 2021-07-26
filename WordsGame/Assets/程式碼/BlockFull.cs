using UnityEngine;
using UnityEngine.UI;
public class BlockFull : MonoBehaviour
{
    public static BlockFull instance; //對戰管理實體物件
    public bool IfBlockfull = false;
    public string CurrentText;
    public void Awake()
    {
        instance = this;
    }

}
