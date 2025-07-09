using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    CharacterController character; // 玩家角色控制器

    void Awake()
    {
        character = GetComponent<CharacterController>(); // 获取角色控制器组件
    }

    private void Start()
    {
        Time.timeScale = 1; // 设置时间流速为正常
        score = 0; // 初始化分数
    }

    public Slider Collect_slider, BGM_slider; // 控制采集音效和背景音乐的滑动条
    public AudioSource BGM, Collect; // 背景音乐和采集音效的音源
    public static int score; // 当前得分（静态变量）
    public TextMeshProUGUI txtScore, txtEnemy; // 显示分数和敌人距离的文本
    public GameObject Fail, Success; // 失败和成功的UI画布
    public MonoBehaviour playerController;   // 玩家控制脚本（如MyFirstPersonController）
    public MonoBehaviour enemyController;    // 敌人控制脚本（如AIEnemy）
    private bool isPaused = false; // 判断当前是否处于暂停状态

    void Update()
    {
        TXT(); // 刷新文本内容

        // 实时调整音量
        BGM.volume = BGM_slider.value;
        Collect.volume = Collect_slider.value;
    }

    void TXT() // 更新显示的文本内容
    {
        if (GameObject.FindGameObjectsWithTag("coin").Length > 0) // 如果场景中还有金币
        {
            GameObject[] coins = GameObject.FindGameObjectsWithTag("coin"); // 获取所有金币
            float dis = float.MaxValue; // 初始化最小距离为最大值
            foreach (GameObject co in coins) // 遍历每一个金币
            {
                if (Vector3.Distance(transform.position, co.transform.position) < dis) // 如果玩家离金币更近
                {
                    dis = Vector3.Distance(transform.position, co.transform.position); // 更新最小距离
                }
            }
            txtScore.text = "最近金币距离: " + dis.ToString("0") + "m" + "   " + score + "/5"; // 显示最近金币距离与当前分数
        }
        else
        {
            txtScore.text = "5/5"; // 如果没有金币了，直接显示满分
        }

        GameObject[] enemys = GameObject.FindGameObjectsWithTag("enemy"); // 获取所有敌人
        float dis1 = float.MaxValue; // 初始化最小距离
        foreach (GameObject e in enemys) // 遍历每个敌人
        {
            if (Vector3.Distance(transform.position, e.transform.position) < dis1) // 如果该敌人更近
            {
                dis1 = Vector3.Distance(transform.position, e.transform.position); // 更新最小距离
            }
        }
        txtEnemy.text = "敌人距离: " + dis1.ToString("0") + "m"; // 显示离最近敌人的距离
    }

    private void OnTriggerEnter(Collider other) // 碰撞检测
    {
        if (other.tag == "enemy") // 如果与敌人碰撞
            Tosets(Fail); // 显示失败UI

        // 如果分数达到5，说明游戏胜利
        if (score == 5)
            Tosets(Success); // 显示成功UI
    }

    private void Tosets(GameObject UIcanvas)
    {
        if (UIcanvas == null)
        {
            Debug.LogWarning("Settings Canvas未设置!");
            return;
        }

        isPaused = !UIcanvas.activeSelf;
        UIcanvas.SetActive(isPaused);

        // 时间流速控制
        Time.timeScale = isPaused ? 0f : 1f;

        // 启用/禁用玩家控制脚本
        if (playerController != null)
            playerController.enabled = !isPaused;

        // 启用/禁用敌人控制脚本
        if (enemyController != null)
            enemyController.enabled = !isPaused;

        // 鼠标光标状态
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
        if (isPaused)
            BGM.Pause();
        else
            BGM.UnPause();
    }
}
