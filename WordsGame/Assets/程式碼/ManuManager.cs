using UnityEngine;

public class ManuManager : MonoBehaviour
{
    public static ManuManager instance; //對戰管理實體物件
    public GameObject blackImage;

    public void Awake()
    {
        instance = this;
    }
}
