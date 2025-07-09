using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIcontrol : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject settingsCanvas; // 拖入Settings画布对象
    public MonoBehaviour playerController;   // 拖入MyFirstPersonController组件
    public MonoBehaviour enemyController;    // 拖入AIEnemy组件
    //public AudioSource[] audioSourcesToPause; // 在Inspector中拖入你想控制的AudioSources

    private bool isPaused = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    /// <summary>
    /// 切换设置菜单激活状态，同时暂停/恢复游戏及相关控制脚本
    /// </summary>
    public void ToggleSettings()
    {
        if (settingsCanvas == null)
        {
            Debug.LogWarning("Settings Canvas未分配!");
            return;
        }

        isPaused = !settingsCanvas.activeSelf;
        settingsCanvas.SetActive(isPaused);

        // 时间缩放
        Time.timeScale = isPaused ? 0f : 1f;

        // 启用/禁用玩家控制脚本
        if (playerController != null)
            playerController.enabled = !isPaused;

        // 启用/禁用敌人控制脚本
        if (enemyController != null)
            enemyController.enabled = !isPaused;

        // 鼠标状态控制
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        // 音效控制
        //foreach (var audioSource in audioSourcesToPause)
        //{
        //    if (isPaused)
        //        audioSource.Pause();
        //    else
        //        audioSource.UnPause();
        //}
    }


    /// <summary>
    /// 场景下标
    /// </summary>
    public static void LoadScene(int index) { SceneManager.LoadScene(index); }

    /// <summary>
    /// 场景名字
    /// </summary>
    public static void LoadScene(string name) { SceneManager.LoadScene(name); }

    public  void NextScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
    public  void ReloadScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    public  void LastScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); }

    /// <summary>
    /// 当前场景上跳转index单位
    /// </summary>
    public void JumpScene(int index) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + index); }

    //退出
    public  void Quit() { Application.Quit(); }
}
