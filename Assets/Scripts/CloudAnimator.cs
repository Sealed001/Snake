using UnityEngine;

public class CloudAnimator : MonoBehaviour
{
    private float _size = 1.5f;
    private float _amplitude = 1f;
    private float _timer = 0f;
    private float _y = 0f;
    private void Start()
    {
        _y = Random.Range(-1000f, 1000f);
    }
    void Update()
    {
        _timer += Time.deltaTime;
        float scale = _size + Mathf.PerlinNoise(_y, _timer / 10f) * _amplitude;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
