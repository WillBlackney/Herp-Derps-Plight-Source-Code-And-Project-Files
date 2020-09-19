using UnityEngine;

public class Projectile : MonoBehaviour
{
    // This script is for NON PARTICLE FX based projectiles

    // Properties + Component References
    #region
    [Header("Properties")]
    private Vector3 destination;
    private bool readyToMove;
    private bool destinationReached;
    private float travelSpeed;
    [SerializeField ]private float yOffset;

    public bool DestinationReached
    {
        get { return destinationReached; }
        private set { destinationReached = true; }
    }
    #endregion

    // Initialization 
    #region

    public void InitializeSetup(Vector3 startPos, Vector3 endPos, float speed)
    {
        transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
        destination = endPos;
        travelSpeed = speed;
        FaceDestination();
        readyToMove = true;
    }
    #endregion

    // Movement logic
    #region
    private void Update()
    {
        if (readyToMove)
        {
            MoveTowardsTarget();
        }
    }
    public void MoveTowardsTarget()
    {
        if (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, travelSpeed * Time.deltaTime);
            if (transform.position == destination)
            {
                destinationReached = true;
                DestroySelf();
            }
        }
    }
    #endregion

    // Misc Logic
    #region
    private void FaceDestination()
    {
        Vector2 direction = destination - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10000f);
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
    #endregion

}
