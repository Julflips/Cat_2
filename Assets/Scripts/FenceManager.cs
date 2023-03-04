using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceManager : MonoBehaviour
{

    public List<FencePost> vertices;
    public GameObject PolygonPre;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            foreach (List<FencePost> circle in GetCircles())
            {
                createPolygon(circle);
            }
        }
    }

    public GameObject createPolygon(List<FencePost> circle)
    {
        
        Vector2[] points = new Vector2[circle.Count];
        Vector3[] positions = new Vector3[circle.Count + 1];
        int i = 0;
        foreach (FencePost p in circle)
        {
            points[i] = p.transform.position;
            positions[i] = p.transform.position;
            i++;
        }
        positions[circle.Count] = positions[0];

        GameObject go = Instantiate(PolygonPre, transform, true);
        LineRenderer lr = go.GetComponent<LineRenderer>();
        PolygonCollider2D pc = go.GetComponent<PolygonCollider2D>();
        pc.points = points;
        lr.positionCount = positions.Length;
        lr.SetPositions(positions);


        return go;
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
