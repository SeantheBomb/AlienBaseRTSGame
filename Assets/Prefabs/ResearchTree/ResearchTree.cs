using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ResearchTree {

    public ResearchNode root;
    int nodesResearched;
    
    public ResearchTree()
    {
        root = null;
    }

    public ResearchTree(ResearchNode node)
    {
        root = node;
    }

    public ResearchTree(List<ResearchNode> ary)
    {
        root = generateResearchNode(ary, 0);
    }

    public ResearchNode generateResearchNode(List<ResearchNode> nodes, int index)
    {
        ResearchNode node;
        int step = index;
        if (step == 0)
            step++;
        else if (step == 1)
            step += 2;
        else
            step = index * 2 + 1;
        if (index < nodes.Count - 2)
            node = new ResearchNode(nodes[index].name, nodes[index].description, generateResearchNode(nodes, step), generateResearchNode(nodes, step + 1));
        else if (index >= nodes.Count)
            return null;
        else
            node = new ResearchNode(nodes[index].name, nodes[index].description, null, null);
        return node;
    }
    

    public ResearchNode Recurse(ResearchNode node, bool moveRight)
    {
        if (moveRight)
            return node.right;
        else
            return node.left;
    }

    public bool isAvailable(string name)
    {
        return Find(root, name).isAvailable;
    }
    public bool isUnlocked(string name)
    {
        return Find(root, name).isUnlocked;
    }
    public void unlock(string name)
    {
        ResearchNode node = Find(root, name);
        node.isUnlocked = true;
        if (node.right != null)
            node.right.isAvailable = true;
        if (node.left != null)
            node.left.isAvailable = true;
    }
    public void available(string name)
    {
        Find(root, name).isAvailable = true;
    }

    public ResearchNode Find(ResearchNode node, string name)
    {
        if (node == null)
            return null;
        if (node.name == name)
            return node;
        node = Find(Recurse(node, true), name);
        if (node != null)
            return node;
        node = Find(Recurse(node, false), name);
        if (node != null)
            return node;

        return null;
    }
}
