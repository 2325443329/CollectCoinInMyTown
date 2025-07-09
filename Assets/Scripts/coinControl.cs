using UnityEngine;
using System.Collections.Generic;

public class coinControl : MonoBehaviour
{
    void Start()
    {
        ActivateRandomCoins(5); // ÿ�ο�ʼ����5��������
    }

    void ActivateRandomCoins(int count)
    {
        // ��ȡ����������
        List<Transform> coins = new List<Transform>();
        foreach (Transform child in transform)
        {
            coins.Add(child);
            child.gameObject.SetActive(false); // ����ȫ������
        }

        // ����������С�����輤�����������ⱨ��
        count = Mathf.Min(count, coins.Count);

        // ������� count �����
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, coins.Count);
            coins[index].gameObject.SetActive(true);
            coins.RemoveAt(index); // �����ظ�����
        }
    }
}
