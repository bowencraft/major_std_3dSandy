using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float speed = 0.1f;  // 控制移动速度
    public Vector3 minBounds = new Vector3(-10, -10, -10);  // 设定运动边界的最小值
    public Vector3 maxBounds = new Vector3(10, 10, 10);  // 设定运动边界的最大值
    public float noiseScale = 1.0f;  // 控制柏林噪声的缩放因子

    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;
    private float time;

    void Start()
    {
        time = Random.Range(0f, 100f);  // 初始化时间，使得每个对象有不同的噪声序列
        GenerateNewTargetPosition();
    }

    void Update()
    {
        time += Time.deltaTime * speed;
        GenerateNewTargetPosition();

        // 使用SmoothDamp来平滑地移动物体
        float smoothTime = 0.3f;  // 调整该值以改变平滑过渡的速度
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    // 使用Perlin Noise生成一个新的随机目标位置
    void GenerateNewTargetPosition()
    {
        float x = Mathf.PerlinNoise(time, 0f) * (maxBounds.x - minBounds.x) + minBounds.x;
        float y = Mathf.PerlinNoise(0f, time) * (maxBounds.y - minBounds.y) + minBounds.y;
        float z = Mathf.PerlinNoise(time, time) * (maxBounds.z - minBounds.z) + minBounds.z;
        targetPosition = new Vector3(x, y, z);
    }
}