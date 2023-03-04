using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceManager : MonoBehaviour
{

    public List<FencePost> vertices;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GetCircles().Count);
    }



    public List<List<FencePost>> GetCircles()
    {
        List<FencePost> virginVertices = new List<FencePost>();
        foreach(FencePost v in vertices)
        {
            virginVertices.Add(v);
        }
        List<List<FencePost>> results = new List<List<FencePost>>();
        while (virginVertices.Count > 0)
        {
            FencePost start = virginVertices[0];
            DFS(start, null, new List<FencePost>(), ref virginVertices, ref results);
        }
        return results;
    }

    private void DFS(FencePost v, FencePost parent, List<FencePost> path, ref List<FencePost> virginVertices, ref List<List<FencePost>> results)
    {
        if (!virginVertices.Contains(v))
        {
            path.Add(null);
            return;
        }
        int index = path.IndexOf(v);
        if (index != -1)
        {
            List<FencePost> circle = path.GetRange(index, path.Count - index);
            results.Add(circle);
            path.Add(null);
            return;
        }
        foreach (FencePost neighbour in v.GetNeighbours())
        {
            if (neighbour == parent)
            {
                continue;
            }
            path.Add(v);
            DFS(neighbour, v, path, ref virginVertices, ref results);
            path.RemoveAt(path.Count - 1);
        }
        virginVertices.Remove(v);

    }
}
