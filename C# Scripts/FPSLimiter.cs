using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    // Start is called before the first frame update
    private float m_LastUpdateShowTime = 0f;    //上一次更新帧率的时间;

    private float m_UpdateShowDeltaTime = 0.01f;//更新帧率的时间间隔;

    private int m_FrameUpdate = 0;//帧数;

    private float m_FPS = 0;

    private float m_FPS2 = 0;

    private int counter = 0;

    void Start()
    {
        Application.targetFrameRate = 90;
        m_LastUpdateShowTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        m_FrameUpdate++;
        counter++;
        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
        {
            m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);
            m_FrameUpdate = 0;
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
        }
        if (counter == 50)
        {
            m_FPS2 = Mathf.Round(m_FPS);
            counter = 0;
        }

    }
    void OnGUI()
    {
        
        GUI.Label(new Rect(Screen.width / 2, 0, 100, 100), "FPS: " + m_FPS2);
    }
}
