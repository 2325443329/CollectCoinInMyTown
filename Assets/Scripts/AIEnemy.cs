using Pathfinding;
using UnityEngine;

public class AIEnemy : MonoBehaviour
{
    public Transform player;
    public Seeker seeker;
    public float speed = 2.0f;
    public float turnSpeed = 5f;
    public float nextWaypointDistance = 1;
    public float repathRate = 0.2f; // 路径更新频率

    private Path path;
    private int currentWaypoint = 0;
    private float lastRepathTime = float.NegativeInfinity;

    void Update()
    {
        float dis = Vector3.Distance(transform.position, player.position);
        GetComponent<Animator>().SetBool("isAttack", dis < 2f);

        // 按固定频率请求路径
        if (Time.time > lastRepathTime + repathRate)
        {
            lastRepathTime = Time.time;
            RequestPath();
        }
    }

    void FixedUpdate()
    {
        if (path == null || path.vectorPath == null || currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // 计算当前航点方向（修复索引越界）
        Vector3 nextWaypoint = path.vectorPath[currentWaypoint];
        Vector3 dir = (nextWaypoint - transform.position).normalized;

        // 移动和旋转
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        }

        // 检查是否到达当前航点
        if (Vector3.Distance(transform.position, nextWaypoint) < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    // 单独封装路径请求方法
    private void RequestPath()
    {
        if (Vector3.Distance(transform.position, player.position) > 10f)
        {
            // 仅当玩家距离较远时更新路径
            if (seeker.IsDone()) // 确保寻路器空闲
            {
                seeker.StartPath(transform.position, player.position, OnPathComplete);
            }
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0; // 重置路径点索引
        }
    }
}