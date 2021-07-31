using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level instance; //關卡管理實體物件
    public static bool IfFinishLevel1;
    public static bool IfFinishLevel2;
    public static int WhichLevel;

    public void Awake()
    {
        instance = this;
    }
}
