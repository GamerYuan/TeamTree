using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bonsai : MonoBehaviour
{
    //L-System to use to generate this tree.
    public LSystem lsystem;
    private Camera mainCamera;
    public Mesh2d crossSection;

    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    //Converts Units to Tree Geometry
    private TreeGeometry treeGeometry = new TreeGeometry();
    private OrientedPoint upwards = new OrientedPoint(Vector3.zero, Quaternion.Euler(Vector3.up));

    //Set of Geometric Constants for L-System.
    public LSystemConstants constants;

    //Length of time for pointer to traverse the tree.
    public float period;
    private float t = 0;

    public bool ScissorsMode = true;


    List<TreeVert> treeVertices = new List<TreeVert>();
    List<int> treeEdges = new List<int>();

    private void Awake()
    {
        mainCamera = Camera.main;
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        treeGeometry.SetConstants(constants);
    }

    private void Start()
    {
        lsystem.InitSystem();
        LoadString(SaveData.currString);
        //lsystem.InitSystem();
    }

    private void Update()
    {
        t += Time.deltaTime / period;
        if (t > 1) t = 0;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                MeshCollider meshCollider = hit.collider as MeshCollider;
                if (meshCollider != null && meshCollider.sharedMesh != null)
                {
                    int triangle = hit.triangleIndex;
                    Unit contactUnit = treeGeometry.FindEdge(triangle);
                    Debug.Log("Unit clicked: " + contactUnit + "triangle: " + triangle);
                    lsystem.RemoveUnitSubtree(contactUnit);

                    GenerateSkeleton();
                    GenerateMesh();
                    treeVertices = treeGeometry.getTreeVertices();
                    treeEdges = treeGeometry.getTreeEdges();
                }
            }
        }
    }

    private void GenerateSkeleton()
    {
        treeGeometry.CalcTreeSkeleton(upwards, new float[] { 0, constants.Thickness, 0 }, lsystem.GetUnits().Where(x => constants.ISGEOMETRY.ContainsKey(x.name)).ToList());
    }

    private void GenerateMesh()
    {
        mesh = treeGeometry.GenerateSurfaceMesh(crossSection);
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = treeGeometry.GetBranchMesh();
    }

    public void ToggleScissors(Component sender, object data)
    {
        if (data is bool)
        {
            ScissorsMode = (bool)data;
        }
    }

    public void TreeUpdate()
    {
        mesh.Clear();
        lsystem.ApplyRules();
        GenerateSkeleton();
        GenerateMesh();
        treeVertices = treeGeometry.getTreeVertices();
        treeEdges = treeGeometry.getTreeEdges();
        SaveData.SetString(GetTreeString());
    }

    public void InitTree()
    {
        Awake();
        lsystem.InitSystem();
        GenerateSkeleton();
        GenerateMesh();
    }

    public string GetTreeString()
    {
        return lsystem.ToString();
    }

    public void LoadString(string str)
    {
        Awake();
        lsystem.LoadString(str);
        GenerateSkeleton();
        GenerateMesh();
    }

    public void WaterTree(float f)
    {
        lsystem.ModifyUnit('R', 0, f);
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
        if (treeVertices.Count > 0) testPoint = getPointOnLine(t);
        TreeVert[] verts = treeVertices.ToArray();
        int[] edges = treeEdges.ToArray();
        Gizmos.color = Color.blue;
        foreach (TreeVert treeVert in verts)
        {
            Vector3 vertpos = LocalToWorldPos(treeVert.point.pos);
            Quaternion vertrot = LocalToWorldRot(treeVert.point.rot);
            Gizmos.DrawSphere(vertpos, treeVert.GetParam(3) / 10);
        }
        Gizmos.color = Color.white;
        for (int i = 0; i < edges.Length - 1; i += 2)
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
