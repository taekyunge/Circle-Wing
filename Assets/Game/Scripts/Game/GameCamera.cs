using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameCamera : MonoBehaviour
{
    public static GameCamera main;
    public SpriteRenderer mapSprite;
    
    [SerializeField] private Vector3 offset;
    [SerializeField] private float moveSpeed;

    private Camera gameCamera;
    private Transform target;

    public float width;
    public float height;

    private void Awake()
    {
        gameCamera = GetComponent<Camera>();
        main = this;

        width = gameCamera.orthographicSize * gameCamera.aspect;
        height = gameCamera.orthographicSize;
    }

    public void SetTarget(Transform target)
    {
        if(this.target == null)
            transform.position = GetClampPos(target.position);

        this.target = target;
    }

    private Vector3 GetClampPos(Vector3 targetPos)
    {
        var pos = targetPos + offset;

        pos.x = Mathf.Clamp(pos.x, mapSprite.bounds.min.x + width, mapSprite.bounds.max.x - width);
        pos.y = Mathf.Clamp(pos.y, mapSprite.bounds.min.y + height, mapSprite.bounds.max.y - height);

        return pos;
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        transform.position = Vector3.Slerp(transform.position, GetClampPos(target.position), moveSpeed * Time.fixedDeltaTime);
    }
}
