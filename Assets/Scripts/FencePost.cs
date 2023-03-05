using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FencePost : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> connectedFences;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<FencePost> GetNeighbours()
    {
        List<FencePost> result = new List<FencePost>();
        foreach (GameObject go in connectedFences)
        {
            FencePost other;
            Fence fence = go.GetComponent<Fence>();
            if(fence.post1 != this)
            {
                other = fence.post1;
            }
            else if(fence.post2 != this)
            {
                other = fence.post2;
            }
            else
            {
                Debug.Log("ERROR Inconsistent graph state");
                continue;
            }
            result.Add(other);
        }
        return result;
    }
}
