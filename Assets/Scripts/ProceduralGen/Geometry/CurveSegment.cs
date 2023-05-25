using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
public class CurveSegment : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float t = 0;

    [Range(1, 64)]
    [SerializeField] int segments = 8;

    [SerializeField] Mesh2d crosssec;

    [SerializeField] Transform[] controlPoints = new Transform[4];

    Vector3 GetPos( int i ) => controlPoints[i].position;

    private Mesh mesh;

    private void Awake()
    {
        mesh = new Mesh();
        mesh.name = "Segment";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void Update() => GenerateMesh();
  
    public void GenerateMesh()
    {
        mesh.Clear();

        //init verts
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        for(int s = 0; s < segments; s++)
        {
            float t = s /(float)(segments - 1);

            OrientedPoint op = GetBezierPoint(t);
            for (int i = 0; i < crosssec.VertexCount; i++)
            {
                verts.Add(op.LocalToWorldPos(crosssec.vertices[i].point));
                normals.Add(op.LocalToWorldVec(crosssec.vertices[i].normal));
            }
        }

        //init triangles
        List<int> triangles = new List<int>();
        for(int s = 0; s < segments - 1; s++)
        {
            int root = s * crosssec.VertexCount;
            
            for(int line = 0; line < crosssec.LineCount; line += 2)
            {
                int lineIndexA = crosssec.lineIndices[line];
                int lineIndexB = crosssec.lineIndices[line + 1];

                int currA = root + lineIndexA;
                int currB = root + lineIndexB;
                int nextA = currA + crosssec.VertexCount;
                int nextB = currB + crosssec.VertexCount;

                triangles.Add(currA);
                triangles.Add(nextA);
                triangles.Add(nextB);
                

                triangles.Add(currA);
                triangles.Add(nextB);
                triangles.Add(currB);
            }
        }

        mesh.SetVertices(verts.ToArray());
        mesh.SetTriangles(triangles.ToArray(), 0);
        mesh.SetNormals(normals);
    }



    public void OnDrawGizmos()
    {
        for(int i = 0; i < 4; i++)
        {
            Gizmos.DrawSphere(GetPos(i), 0.035f);
        }

        Handles.DrawBezier(
            GetPos(0), 
            GetPos(3), 
            GetPos(1), 
            GetPos(2), 
            Color.white,
            EditorGUIUtility.whiteTexture, 1f
        );

        OrientedPoint testPoint = GetBezierPoint(t); 
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(testPoint.pos, 0.05f);
        Handles.PositionHandle(testPoint.pos, testPoint.rot);

        

        Gizmos.color = Color.white;
        for(int i = 0; i < crosssec.VertexCount; i++)
        {
            Gizmos.DrawSphere(testPoint.pos + testPoint.rot * crosssec.vertices[i].point, 0.1f);
        }

        for(int i = 0; i < crosssec.LineCount; i += 2)
        {
            Gizmos.DrawLine(
                testPoint.pos + testPoint.rot * crosssec.vertices[crosssec.lineIndices[i]].point,
                testPoint.pos + testPoint.rot * crosssec.vertices[crosssec.lineIndices[i + 1]].point
                );
        }
    }

    OrientedPoint GetBezierPoint(float t)
    {
        Vector3 p0 = GetPos(0);
        Vector3 p1 = GetPos(1);
        Vector3 p2 = GetPos(2);
        Vector3 p3 = GetPos(3);

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        Vector3 pos =  Vector3.Lerp(d, e, t);
        Vector3 tangent = (e - d).normalized;

        return new OrientedPoint(pos, tangent);
    }
}
