using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combine : MonoBehaviour
{
    private MeshFilter[] meshesToCombine;
    CombineInstance[] combine;

    public Mesh newMesh;

    // Start is called before the first frame update
    void Start()
    {
        meshesToCombine = GetComponentsInChildren<MeshFilter>();
        combine = new CombineInstance[meshesToCombine.Length];

        for (int i = 0; i < meshesToCombine.Length; i++)
        {
            combine[i].mesh = meshesToCombine[i].sharedMesh;
            combine[i].transform = meshesToCombine[i].transform.localToWorldMatrix;
            meshesToCombine[i].gameObject.SetActive(false);
        }


        newMesh = new Mesh();
        newMesh.CombineMeshes(combine);
        GetComponent<MeshFilter>().sharedMesh = newMesh;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
