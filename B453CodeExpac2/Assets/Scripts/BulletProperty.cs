using UnityEngine;

public class BulletProperty : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    void Update()
    {
        gameObject.transform.position +=  Time.deltaTime * speed * direction;
        
        // clean off-screen object
        if (gameObject.transform.position.x > 20 || gameObject.transform.position.x < -20 ||
            gameObject.transform.position.y > 20 || gameObject.transform.position.y < -20)
        {
            Destroy(gameObject);
        }
    }

    public void assignSpeedAndDirection(float speed, Vector3 direction)
    {
        this.direction = direction;
        this.speed = speed;
    }
}
