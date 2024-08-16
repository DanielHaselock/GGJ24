using UnityEngine;

public class WindowStuff : MonoBehaviour
{
    int LastScreenWidth = 1024;
    int LastScreenHeight = 768;
    float ScreenRatio = 1024f / 768f;

    void Update()
    {
        if ((LastScreenHeight != Screen.height) || (LastScreenWidth != Screen.width))
        {
            Screen.SetResolution(Mathf.RoundToInt((float)Screen.height * ScreenRatio), Screen.height, Screen.fullScreen);
        }
        LastScreenHeight = Screen.height;
        LastScreenWidth = Mathf.RoundToInt(Screen.height * ScreenRatio);
    }
 }
