using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryTest : MonoBehaviour
{
    Mesh mesh;
    MeshFilter meshFilter;
    TreeGeometry treeGeometry;

    [SerializeField]
    Mesh2d crosssec;
    // Start is called before the first frame update
    void Start() { 
        treeGeometry = new TreeGeometry();
        meshFilter = GetComponent<MeshFilter>();
        OrientedPoint o1 = new OrientedPoint(transform);
        OrientedPoint o2 = new OrientedPoint(transform.position + Vector3.up, transform.rotation);
        mesh = treeGeometry.ExtrudeEdge(10, o1, o2, crosssec);

        meshFilter.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        OrientedPoint o1 = new OrientedPoint(transform);
        OrientedPoint o2 = new OrientedPoint(transform.position + Vector3.up, transform.rotation);
        mesh.Clear();
        mesh = treeGeometry.ExtrudeEdge(10, o1, o2, crosssec);
        meshFilter.mesh = mesh;
    }
}
