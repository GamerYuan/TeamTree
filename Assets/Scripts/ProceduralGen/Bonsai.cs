using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using static MathNet.Symbolics.VisualExpression;
using static UnityEditor.PlayerSettings;
using TreeEditor;

public class Bonsai : MonoBehaviour
{
    //L-System to use to generate this tree.
    [SerializeField]
    private LSystem lsystem;

    [SerializeField]
    private Mesh2d crossSection;

    private Mesh mesh;
    private MeshFilter meshFilter;

    //Converts Units to Tree Geometry
    private TreeGeometry treeGeometry = new TreeGeometry();
    private OrientedPoint upwards = new OrientedPoint(Vector3.zero, Quaternion.Euler(Vector3.up));

    //Set of Geometric Constants for L-System.
    [SerializeField]
    private LSystemConstants constants;

    //Length of time for pointer to traverse the tree.
    [SerializeField]
    private float period;
    private float t = 0;


    List<TreeVert> treeVertices = new List<TreeVert>();
    List<int> treeEdges = new List<int>();

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        lsystem.InitAxiom();
        treeGeometry.setConstants(constants);
    }

    private void Update()
    {
        t += Time.deltaTime / period;
        if (t > 1) t = 0;
        GenerateSkeleton();
        GenerateMesh();
        treeVertices = treeGeometry.getTreeVertices();
        treeEdges = treeGeometry.getTreeEdges();

    }

    private void GenerateSkeleton()
    {
        treeGeometry.CalcTreeSkeleton(upwards, lsystem.GetUnits());
    }

    private void GenerateMesh()
    {
        mesh = treeGeometry.GenerateSurfaceMesh(crossSection);
        meshFilter.sharedMesh = mesh;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 50, 50), "update"))
        {
            lsystem.ApplyRules();
            GenerateSkeleton();
            GenerateMesh();
            treeVertices = treeGeometry.getTreeVertices();
            treeEdges = treeGeometry.getTreeEdges();
        }
    }

    private Vector3 LocalToWorldPos(Vector3 localPos)
    {
        return transform.position + transform.rotation * localPos;
    }

    private Quaternion LocalToWorldRot(Quaternion localRot)
    {
        return (transform.rotation * localRot).normalized;
    }

    private void OnDrawGizmosSelected()
    {
        OrientedPoint testPoint = new OrientedPoint(transform);
        if (treeVertices.Count > 0)  testPoint = getPointOnLine(t);
        TreeVert[] verts = treeVertices.ToArray();
        int[] edges = treeEdges.ToArray();
        Gizmos.color = Color.blue;
        foreach(TreeVert treeVert in verts)
        {
            Vector3 vertpos = LocalToWorldPos(treeVert.point.pos);
            Quaternion vertrot = LocalToWorldRot(treeVert.point.rot);
            Gizmos.DrawSphere(vertpos, 0.05f);
        }
            Gizmos.color = Color.white;
        for (int i = 0; i < edges.Length - 1; i+= 2)
        {
            Vector3 vert1pos = LocalToWorldPos(verts[edges[i]].point.pos);
            Vector3 vert2pos = LocalToWorldPos(verts[edges[i + 1]].point.pos);
            Gizmos.DrawLine(vert1pos, vert2pos);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(testPoint.pos, 0.05f);
    }

    private OrientedPoint getPointOnLine(float t)
    {
        float vertexNo = t *= treeVertices.Count - 1;
        TreeVert vert1 = treeVertices[Mathf.Max(0, Mathf.FloorToInt(vertexNo))];
        TreeVert vert2 = treeVertices[Mathf.Min(treeVertices.Count - 1, Mathf.CeilToInt(vertexNo))];
        Vector3 testPos = Vector3.Lerp(vert1.point.pos, vert2.point.pos, vertexNo - Mathf.Floor(vertexNo));
        Quaternion testRot = Quaternion.Lerp(vert1.point.rot, vert2.point.rot, vertexNo - Mathf.Floor(vertexNo));
        return new OrientedPoint(LocalToWorldPos(testPos), LocalToWorldRot(testRot));
    }
}
