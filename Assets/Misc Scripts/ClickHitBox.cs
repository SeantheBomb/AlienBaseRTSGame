using UnityEngine;
using System.Collections;

public class ClickHitBox : MonoBehaviour {

    public Entity parent;

    public float width = 1;
    public float length = 1;
    public float height = 1;

    BoxCollider coll;

    public void Start()
    {
        coll = this.gameObject.AddComponent<BoxCollider>();
        updateSize();
    }

    public void updateSize()
    {
        if (coll != null)
        {
            Vector3 newSize = new Vector3(width, height, length);
            coll.size = newSize;
        }
        else
        {
            Debug.LogError("Collider is null!");
        }
    }

    public void setSize(float size)
    {
        width = size;
        length = size;
        height = size;
    }

    public void setSize(float width, float length)
    {
        this.width = width;
        this.length = length;
    }

    public void setSize(float width, float length, float height)
    {
        this.width = width;
        this.length = length;
        this.height = height;
    }
}
