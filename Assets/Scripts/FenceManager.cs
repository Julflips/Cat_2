using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceManager : MonoBehaviour
{

    public List<FencePost> vertices;
    public GameObject PolygonPre;

    public List<GameObject> capturedCats;

    private ContactFilter2D catFilter;
    // Start is called before the first frame update
    void Start()
    {
        catFilter = new ContactFilter2D();
        catFilter.SetLayerMask(LayerMask.GetMask("Cat"));
        capturedCats = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCats()
    {
        capturedCats = GetCapturedCats();
        Debug.Log(capturedCats.Count);
    }

    private List<GameObject> GetCapturedCats() {
        List<Collider2D> cols = new List<Collider2D>();
        foreach (PolygonCollider2D col in createColliders())
        {
            Collider2D[] results = new Collider2D[100];
            col.OverlapCollider(catFilter, results);
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i])
                {
                    cols.Add(results[i]);
                }
                else { break; }
            }
        }
        List<GameObject> cats = new List<GameObject>();
        foreach (Collider2D col in cols)
        {
            if (!cats.Contains(col.gameObject))
            {
                cats.Add(col.gameObject);
            }
        }
        return cats;
    }

    private List<PolygonCollider2D> createColliders()
    {
        List<PolygonCollider2D> results = new List<PolygonCollider2D>();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        foreach (List<FencePost> circle in GetCircles())
        {
            results.Add(createPolygon(circle).GetComponent<PolygonCollider2D>());
        }

        return results;

    }

    private GameObject createPolygon(List<FencePost> circle)
    {
        
        Vector2[] points = new Vector2[circle.Count + 1];
        Vector3[] positions = new Vector3[circle.Count + 1];
        int i = 0;
        foreach (FencePost p in circle)
        {
            points[i] = p.transform.position;
            positions[i] = p.transform.position;
            i++;
        }
        positions[circle.Count] = positions[0];
        points[circle.Count] = points[0];

        GameObject go = Instantiate(PolygonPre, transform, true);
        LineRenderer lr = go.GetComponent<LineRenderer>();
        PolygonCollider2D pc = go.GetComponent<PolygonCollider2D>();
        pc.points = points;
        lr.positionCount = positions.Length;
        lr.SetPositions(positions);


        return go;
    }



    private List<List<FencePost>> GetCircles()
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
