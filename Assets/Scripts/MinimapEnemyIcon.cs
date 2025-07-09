using UnityEngine;
using UnityEngine.UI;

public class MinimapEnemyIcon : MonoBehaviour
{
    [Header("敌人的世界坐标目标")]
    public Transform enemy;

    [Header("玩家Transform")]
    public Transform player;

    [Header("敌人图标")]
    public RectTransform enemyIcon;

    [Header("小地图范围容器")]
    public RectTransform minimapRect;

    [Header("单位换算：世界单位 → 小地图像素")]
    public float mapScale;

    [Header("图标缩放范围")]
    public float minScale;
    public float maxScale;

    void Update()
    {
        // 敌人相对玩家的偏移
        Vector2 worldOffset = new Vector2(
            enemy.position.z - player.position.z,
            -(enemy.position.x - player.position.x)
        );


        // 将偏移量缩放成小地图 UI 偏移
        Vector2 minimapOffset = worldOffset * mapScale;

        // 固定角色图标在 UI 上的位置
        Vector2 playerIconPos = new Vector2(-110f, 110f);

        // 敌人图标目标位置 = 固定角色图标 + 偏移
        Vector2 targetPos = playerIconPos + minimapOffset;

        // 小地图边界
        float halfWidth = minimapRect.rect.width / 2f;
        float halfHeight = minimapRect.rect.height / 2f;

        // 限制位置和缩放
        float distanceRatio = 1f;
        Vector2 centerToTarget = targetPos - playerIconPos;

        if (Mathf.Abs(centerToTarget.x) > halfWidth || Mathf.Abs(centerToTarget.y) > halfHeight)
        {
            if (Mathf.Abs(centerToTarget.x) > halfWidth)
            {
                targetPos.x = playerIconPos.x + Mathf.Sign(centerToTarget.x) * halfWidth;
                distanceRatio = 0f;
            }
            if(Mathf.Abs(centerToTarget.y) > halfHeight)
            {
                targetPos.y = playerIconPos.y + Mathf.Sign(centerToTarget.y) * halfHeight;
                distanceRatio = 0f;
            }
        }
        else
        {
            float maxDistance = Mathf.Max(halfWidth, halfHeight);
            distanceRatio = 1f - (centerToTarget.magnitude / maxDistance);
        }

        // 设置敌人图标位置
        enemyIcon.anchoredPosition = targetPos;

        // 设置敌人图标缩放
        float scale = Mathf.Lerp(minScale, maxScale, distanceRatio);
        enemyIcon.localScale = new Vector3(scale, scale, 1f);

    }

    // Vector2 ClampToRect(Vector2 pos, float halfW, float halfH)
    // {
    //     float angle = Mathf.Atan2(pos.y, pos.x);
    //     float x = Mathf.Cos(angle) * halfW;
    //     float y = Mathf.Sin(angle) * halfH;

    //     x = Mathf.Clamp(x, -halfW, halfW);
    //     y = Mathf.Clamp(y, -halfH, halfH);

    //     return new Vector2(x, y);
    // }
}
