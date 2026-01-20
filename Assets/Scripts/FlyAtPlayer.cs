using UnityEngine;

public class FlyAtPlayer : MonoBehaviour
{
    [SerializeField] float speed = 0.1f;
    [SerializeField] Transform player;
    Vector3 playerPosition;



    void Start()
    {
        playerPosition = player.transform.position;
    }


    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed);
        DestroyWhenReached();
    }

    void DestroyWhenReached()
    {
        if(transform.position == playerPosition)
        {
            Destroy(gameObject);
        }
    }
}
