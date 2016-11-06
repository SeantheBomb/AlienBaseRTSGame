using UnityEngine;
using System.Collections;

public class ResourceNode : Nature {

    public int count=0;

    public Resource.Type type;

	// Use this for initialization
	void Start () {
        base.Start();
        resourceType = new Resource(type, resource);
        resourceType.amount = resourceType.capacity;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void takeResources(int speed)
    {
        if(resource <= 0)
        {
            Destroy(this.gameObject);
        }
        count++;
        if (count % 60 == 0)
        {
            count = 0;
            resource -= speed;
            Resource.addResource(this.resourceType.type, speed);
        }
    }
}
