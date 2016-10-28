using UnityEngine;
using System.Collections;

[System.Serializable]
public class ResearchNode {
    static int uidCounter;

    public int uid;

    public ResearchNode left=null;
    public ResearchNode right=null;

    public string name, description;
    public bool isAvailable = false;
    public bool isUnlocked = false;

    public ResearchNode()
    {
        uid = uidCounter++;
    }

    public ResearchNode(string name, string desc, ResearchNode left, ResearchNode right)
    {
        this.left = left;
        this.right = right;
        this.name = name;
        this.description = desc;
        uid = uidCounter++;
    }

    public bool isLeaf()
    {
        return left == null && right == null;
    }

    public bool isStem()
    {
        return left == null || right == null;
    }

    public ResearchNode getStem()
    {
        if (right != null)
            return right;
        else
            return left;
    }

    
}
