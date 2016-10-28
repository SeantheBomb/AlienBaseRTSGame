using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public KeyCode up, down, left, right;
    public float moveSpeed;
    public float scrollSpeed;
    public float minZoom=5, maxZoom=30, bounds=50;



    bool isMoving;
    Direction dir;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector3 cameraPosition = transform.position;
        if ((Input.GetKey(up)||(isMoving&&dir==Direction.Up))&&transform.position.z<bounds)
        {
            cameraPosition.z += moveSpeed * Time.deltaTime;
        }
        if ((Input.GetKey(down) || (isMoving && dir == Direction.Down))&& transform.position.z > -bounds)
        {
            cameraPosition.z -= moveSpeed * Time.deltaTime;
        }
        if ((Input.GetKey(left) || (isMoving && dir == Direction.Left))&& transform.position.x > -bounds)
        {
            cameraPosition.x -= moveSpeed * Time.deltaTime;
        }
        if ((Input.GetKey(right) || (isMoving && dir == Direction.Right))&& transform.position.x < bounds)
        {
            cameraPosition.x += moveSpeed * Time.deltaTime;
        }
        cameraPosition.y -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;

        if (cameraPosition.y < minZoom)
            cameraPosition.y = minZoom;
        if (cameraPosition.y > maxZoom)
            cameraPosition.y = maxZoom;

        transform.position = cameraPosition;
    }


    public void startMovingUp(bool isUp)
    {
        if (isUp)
            dir = Direction.Up;
        else
            dir = Direction.Down;
        isMoving = true;
    }

    public void startMovingRight(bool isRight)
    {
        if (isRight)
            dir = Direction.Right;
        else
            dir = Direction.Left;
        isMoving = true;
    }

    public void stopMoving()
    {
        isMoving = false;
        this.dir = Direction.Null;
    }
}

    
