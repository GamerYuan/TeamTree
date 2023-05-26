using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/* Converts a List of Units to a set of Vertices and Edges to render a Tree Mesh
 */
public class TreeGeometry
{
   
    private LSystemConstants constants;

    private List<TreeVert> treeVertices = new List<TreeVert>();
    private List<int> treeEdges = new List<int>();

    private Mesh surfaceMesh;

    public void setConstants(LSystemConstants constants)
    {
        this.constants = constants;
    }

    public void CalcTreeSkeleton(OrientedPoint origin, List<Unit> units)
    {
        Stack<TreeVert> turtleVerts = new Stack<TreeVert>();
        TreeVert originVert = new TreeVert(origin, 0);
        turtleVerts.Push(originVert);

        treeVertices.Clear();
        treeEdges.Clear();
        treeVertices.Add(originVert);

        TreeVert currVert = originVert.clone();
        for (int unit = 0; unit < units.Count; unit++)
        {
            Unit currUnit = units[unit];
            Transformation<TreeVert> transformation = constants.GetTransformation(currUnit);
            StackMod<TreeVert> stackMod = constants.GetStackMod(currUnit);

            TreeVert nextVert = transformation.Invoke(currVert);
            
            //Only adds an edge if the next point added is different from the previous point; i.e. no duplicate points on same point in space
            if(!nextVert.point.Equals(currVert.point))
            {
                nextVert.id = treeVertices.Count;
                treeVertices.Add(nextVert);
                treeEdges.Add(currVert.id);
                treeEdges.Add(nextVert.id);
            }
            stackMod.Invoke(nextVert, turtleVerts);
            currVert = turtleVerts.Peek().clone();
        }
    }

    public Mesh GenerateSurfaceMesh(Mesh2d crosssec)
    {
        surfaceMesh = new Mesh();
        surfaceMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        int segments = 5;
        CombineInstance[] combine = new CombineInstance[treeEdges.Count / 2];
        Mesh segment;
        for(int i = 0; i < treeEdges.Count; i+= 2)
        {
            OrientedPoint o1 = treeVertices[treeEdges[i]].point;
            OrientedPoint o2 = treeVertices[treeEdges[i + 1]].point;
            segment = ExtrudeEdge(segments, o1, o2, crosssec);
            combine[i / 2].mesh = segment;
            combine[i / 2].transform = Matrix4x4.identity;
        }

        surfaceMesh.CombineMeshes(combine);
        return surfaceMesh;
    }

    public Mesh ExtrudeEdge(int segments, OrientedPoint o1, OrientedPoint o2, Mesh2d crosssec)
    {
        Mesh prism = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        for (int s = 0; s < segments; s++)
        {
            float t = s / (float)(segments - 1);
            OrientedPoint op = OrientedPoint.Lerp(o1, o2, t);
            for (int i = 0; i < crosssec.VertexCount; i++)
            {
                verts.Add(op.LocalToWorldPos(crosssec.vertices[i].point * 0.1f));
                normals.Add(op.LocalToWorldVec(crosssec.vertices[i].normal));
            }
        }

        List<int> triangles = new List<int>();
        for (int s = 0; s < segments - 1; s++)
        {
            int root = s * crosssec.VertexCount;

            for (int line = 0; line < crosssec.LineCount; line += 2)
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

        prism.SetVertices(verts);
        prism.SetTriangles(triangles, 0);
        prism.SetNormals(normals);
        return prism;
    }
    
    public List<TreeVert> getTreeVertices()
    {
        return treeVertices;
    }

    public List<int> getTreeEdges()
    {
        return treeEdges;
    }
}
