using UnityEngine;
using UnityEngine.UI;

public class MinimapIcon : MonoBehaviour
{
    //public RectTransform minimapRect; // RawImage 或其父物体的 RectTransform
    public Transform player; // 玩家位置
    public RectTransform arrowIcon; // 箭头图标

    public float mapScale = 1f; // 小地图世界单位与UI单位的缩放比例

    void Update()
    {
        // // 1. 把玩家的世界坐标转换成UI坐标
        // Vector2 pos = new Vector2(player.position.x, player.position.z) * mapScale;

        // // 2. 设置图标的位置（相对于小地图中心）
        // arrowIcon.anchoredPosition = pos;

        // 3. 设置图标旋转（注意是负号，因为 UI y轴朝上，z轴绕转）
        arrowIcon.localRotation = Quaternion.Euler(0f, 0f, -player.eulerAngles.y + 270);
    }
}
