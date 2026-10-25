using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 2f;

    private Vector2 direction = Vector2.right;

    public void Initialize(Vector2 shootingDirection)
    {
        direction = shootingDirection.normalized;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position +=
            (Vector3)direction * speed * Time.deltaTime;
    }
}
