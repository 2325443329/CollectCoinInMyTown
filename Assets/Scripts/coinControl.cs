using UnityEngine;
using System.Collections.Generic;

public class coinControl : MonoBehaviour
{
    void Start()
    {
        ActivateRandomCoins(5); // 每次开始激活5个随机金币
    }

    void ActivateRandomCoins(int count)
    {
        // 获取所有子物体
        List<Transform> coins = new List<Transform>();
        foreach (Transform child in transform)
        {
            coins.Add(child);
            child.gameObject.SetActive(false); // 首先全部禁用
        }

        // 如果金币数量小于所需激活数量，避免报错
        count = Mathf.Min(count, coins.Count);

        // 随机激活 count 个金币
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, coins.Count);
            coins[index].gameObject.SetActive(true);
            coins.RemoveAt(index); // 避免重复激活
        }
    }
}
