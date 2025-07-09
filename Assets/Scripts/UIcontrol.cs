using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIcontrol : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject settingsCanvas; // ����Settings��������
    public MonoBehaviour playerController;   // ����MyFirstPersonController���
    public MonoBehaviour enemyController;    // ����AIEnemy���
    //public AudioSource[] audioSourcesToPause; // ��Inspector������������Ƶ�AudioSources

    private bool isPaused = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    /// <summary>
    /// �л����ò˵�����״̬��ͬʱ��ͣ/�ָ���Ϸ����ؿ��ƽű�
    /// </summary>
    public void ToggleSettings()
    {
        if (settingsCanvas == null)
        {
            Debug.LogWarning("Settings Canvasδ����!");
            return;
        }

        isPaused = !settingsCanvas.activeSelf;
        settingsCanvas.SetActive(isPaused);

        // ʱ������
        Time.timeScale = isPaused ? 0f : 1f;

        // ����/������ҿ��ƽű�
        if (playerController != null)
            playerController.enabled = !isPaused;

        // ����/���õ��˿��ƽű�
        if (enemyController != null)
            enemyController.enabled = !isPaused;

        // ���״̬����
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
        // ��Ч����
        //foreach (var audioSource in audioSourcesToPause)
        //{
        //    if (isPaused)
        //        audioSource.Pause();
        //    else
        //        audioSource.UnPause();
        //}
    }


    /// <summary>
    /// �����±�
    /// </summary>
    public static void LoadScene(int index) { SceneManager.LoadScene(index); }

    /// <summary>
    /// ��������
    /// </summary>
    public static void LoadScene(string name) { SceneManager.LoadScene(name); }

    public  void NextScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
    public  void ReloadScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    public  void LastScene() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); }

    /// <summary>
    /// ��ǰ��������תindex��λ
    /// </summary>
    public void JumpScene(int index) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + index); }

    //�˳�
    public  void Quit() { Application.Quit(); }
}
