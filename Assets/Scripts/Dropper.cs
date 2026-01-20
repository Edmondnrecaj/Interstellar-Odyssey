using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] float timeToWait = 2f;

    Rigidbody2D myRigidBody;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myRigidBody.gravityScale = 0f;
    }

    void Update()
    {
        if (Time.time > timeToWait)
        {
            myRigidBody.gravityScale = 1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // destroy on ANY collision (player, ground, walls ...)
        Destroy(gameObject);
    }
}