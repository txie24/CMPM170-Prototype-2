using UnityEngine;

public class GravityTrigger : MonoBehaviour
{

    [SerializeField] private Direction direction;
    
    private enum Direction
    {
        Right, Left, Up, Down
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        // Check if the object has a Rigidbody (player) and an Animator
        if (other.TryGetComponent<PlayerMovement>(out var pm) )
        {
            Debug.Log(direction);
            switch (direction)
            {
                case Direction.Right:
                    pm.ChangeGravityDirection(0);
                    // change animator code too
                    break;
                case Direction.Left:
                    pm.ChangeGravityDirection(1);
                    break;
                case Direction.Up:
                    pm.ChangeGravityDirection(2);
                    break;
                case Direction.Down:
                    pm.ChangeGravityDirection(3);
                    break;
            }
        }
    }
}