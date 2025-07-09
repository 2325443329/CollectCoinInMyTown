using UnityEngine;
using UnityEngine.UI;

public class EnemyIconFollow : MonoBehaviour
{
    [Header("敌人的世界坐标目标")]
    public Transform enemy;          // 敌人对象

    [Header("玩家Transform")]
    public Transform player;         // 玩家对象

    [Header("敌人图标")]
    public RectTransform enemyIcon;  // UI中的敌人图标

    [Header("角色图标")]
    public RectTransform playerIcon;  // UI中的角色图标

    [Header("小地图范围容器")]
    public RectTransform minimapRect; // RawImage或其父物体的RectTransform

    [Header("单位换算：世界单位 → 小地图像素")]
    public float mapScale = 5.0f;

    [Header("图标缩放范围")]
    public float minScale = 0.4f;
    public float maxScale = 1.0f;

    void Update()
    {
        // 1. 敌人相对玩家的世界偏移（XZ平面）
        Vector2 worldOffset = new Vector2(
            enemy.position.x - player.position.x,
            enemy.position.z - player.position.z
        );

        // 2. 旋转偏移量，考虑玩家的朝向
        float yaw = player.eulerAngles.y * Mathf.Deg2Rad;
        float cos = Mathf.Cos(-yaw);
        float sin = Mathf.Sin(-yaw);

        Vector2 rotatedOffset = new Vector2(
            worldOffset.x * cos - worldOffset.y * sin,
            worldOffset.x * sin + worldOffset.y * cos
        );

        // 3. 缩放为小地图坐标
        Vector2 minimapOffset = rotatedOffset * mapScale;

        // 4. 敌人图标目标位置 = 玩家图标位置 + 偏移
        Vector2 playerIconPos = playerIcon.anchoredPosition;
        Vector2 targetPos = playerIconPos + minimapOffset;

        // 5. 小地图边界限制
        float halfWidth = minimapRect.rect.width / 2f;
        float halfHeight = minimapRect.rect.height / 2f;

        float distanceRatio = 1f;
        Vector2 centerToTarget = targetPos - playerIconPos;

        if (Mathf.Abs(centerToTarget.x) > halfWidth || Mathf.Abs(centerToTarget.y) > halfHeight)
        {
            // 限制在边界内
            Vector2 clampedOffset = ClampToRect(centerToTarget, halfWidth, halfHeight);
            targetPos = playerIconPos + clampedOffset;
            distanceRatio = 0f;
        }
        else
        {
            float maxDistance = Mathf.Max(halfWidth, halfHeight);
            distanceRatio = 1f - (centerToTarget.magnitude / maxDistance);
        }

        // 6. 应用图标位置
        enemyIcon.anchoredPosition = targetPos;

        // 7. 应用缩放（根据距离）
        float scale = Mathf.Lerp(minScale, maxScale, distanceRatio);
        enemyIcon.localScale = new Vector3(scale, scale, 1f);

        // 8. 敌人图标旋转（指向真实方向）
        Vector3 dir = enemy.position - player.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float relativeAngle = angle - player.eulerAngles.y;
        enemyIcon.localRotation = Quaternion.Euler(0, 0, -relativeAngle);
    }


    // 辅助：将位置限制在矩形边界内
    Vector2 ClampToRect(Vector2 pos, float halfW, float halfH)
    {
        float angle = Mathf.Atan2(pos.y, pos.x);
        float x = Mathf.Cos(angle) * halfW;
        float y = Mathf.Sin(angle) * halfH;

        // 限制在最大矩形范围内
        x = Mathf.Clamp(x, -halfW, halfW);
        y = Mathf.Clamp(y, -halfH, halfH);

        return new Vector2(x, y);
    }
}
