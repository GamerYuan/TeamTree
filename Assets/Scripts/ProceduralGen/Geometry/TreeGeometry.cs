using System;
using System.Collections.Generic;
using UnityEngine;

/* Converts a List of Units to a set of Vertices and Edges to render a Tree Mesh
 */
public class TreeGeometry
{

    private LSystemConstants constants;

    private List<TreeVert> treeVertices = new List<TreeVert>();
    private List<int> treeEdges = new List<int>();
    private List<List<TreeVert>> treePolygons = new List<List<TreeVert>>();

    private Mesh surfaceMesh;
    private Mesh branchMesh;

    private int crosssecEdges = 4;
    private int segmentsPerEdge = 1;


    private List<Unit> triangleToUnit = new List<Unit>();

    public void SetConstants(LSystemConstants constants)
    {
        this.constants = constants;
    }
    public void CalcTreeSkeleton(OrientedPoint origin, float[] startingParameters, List<Unit> units)
    {
        treeEdges.Clear();
        treeVertices.Clear();
        treePolygons.Clear();
        Stack<TreeVert> turtleVerts = new Stack<TreeVert>();
        TreeVert originVert = new TreeVert(origin, 0, startingParameters);
        turtleVerts.Push(originVert);

        treeVertices.Clear();
        treeEdges.Clear();
        triangleToUnit.Clear();
        List<TreeVert> polygonVerts = new List<TreeVert>();
        bool isPolygon = false;

        TreeVert currVert = originVert.Clone();
        treeVertices.Add(originVert);
        for (int unit = 0; unit < units.Count; unit++)
        {
            Unit currUnit = units[unit];
            currUnit.SetDefaults(constants.GetDefaultParams(currUnit.name));
            Transformation<TreeVert> transformation = constants.GetTransformation(currUnit);
            StackMod<TreeVert> stackMod = LSystem.GetStackMod<TreeVert>(currUnit);
            TreeVert nextVert = transformation.Invoke(currVert);

            if (currUnit.name == '{')
            {
                isPolygon = true;
            }
            else if (currUnit.name == '}' && polygonVerts.Count > 0)
            {
                treePolygons.Add(polygonVerts.ConvertAll(x => x.Clone()));
                polygonVerts.Clear();
                isPolygon = false;
            }
            else if (constants.AddsNode(currUnit) && !isPolygon)
            {
                nextVert.id = treeVertices.Count;
                treeVertices.Add(nextVert);
                treeEdges.Add(currVert.id);
                treeEdges.Add(nextVert.id);

                triangleToUnit.Add(currUnit);
            }
            else if (constants.AddsNode(currUnit))
            {
                nextVert.id = treeVertices.Count;
                polygonVerts.Add(currVert);
            }

            stackMod.Invoke(nextVert, turtleVerts);
            currVert = turtleVerts.Peek().Clone();
        }
    }

    private Mesh GeneratePolygon(List<TreeVert> treeVertices)
    {
        Mesh polygon = new Mesh();
        surfaceMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();


        verts.Add(treeVertices[0].point.pos);

        normals.Add(treeVertices[0].point.rot * Vector3.up);
        colors.Add(constants.GetColor(Convert.ToInt16(treeVertices[0].GetParam(TreeVert.COLOR))));

        for (int i = 1; i < treeVertices.Count - 1; i++)
        {
            colors.Add(constants.GetColor(Convert.ToInt16(treeVertices[i].GetParam(TreeVert.COLOR))));
            verts.Add(treeVertices[i].point.pos);
            normals.Add(treeVertices[i].point.rot * Vector3.up);
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);

            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i);
        }
        verts.Add(treeVertices[treeVertices.Count - 1].point.pos);

        normals.Add(treeVertices[treeVertices.Count - 1].point.rot * Vector3.up);
        colors.Add(constants.GetColor(Convert.ToInt16(treeVertices[treeVertices.Count - 1].GetParam(TreeVert.COLOR))));


        polygon.SetVertices(verts);
        polygon.SetNormals(normals);
        polygon.SetTriangles(triangles, 0);
        polygon.SetColors(colors);

        return polygon;
    }

    public Unit FindEdge(int triangleIndex)
    {
        int trianglesPerEdge = segmentsPerEdge * 2 * crosssecEdges;
        return triangleToUnit[triangleIndex / trianglesPerEdge];
    }

    public Mesh GenerateSurfaceMesh(Mesh2d crosssec)
    {
        surfaceMesh = new Mesh();
        branchMesh = new Mesh();
        surfaceMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        branchMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        int segments = segmentsPerEdge + 1;
        CombineInstance[] combineBranch = new CombineInstance[treeEdges.Count / 2];
        CombineInstance[] combineAll = new CombineInstance[treeEdges.Count / 2 + treePolygons.Count];
        Mesh segment = new Mesh();
        segment.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        for (int i = 0; i < treeEdges.Count; i += 2)
        {
            TreeVert t1 = treeVertices[treeEdges[i]];
            TreeVert t2 = treeVertices[treeEdges[i + 1]];
            OrientedPoint o1 = t1.point;
            OrientedPoint o2 = t2.point;
            float s1 = t1.GetParam(TreeVert.THICKNESS);
            float s2 = t2.GetParam(TreeVert.THICKNESS);
            Color c1 = constants.GetColor(Convert.ToInt16(t1.GetParam(TreeVert.COLOR)));
            Color c2 = constants.GetColor(Convert.ToInt16(t2.GetParam(TreeVert.COLOR)));
            segment = ExtrudeEdge(segments, o1, o2, crosssec, s1, s2, c1, c2);
            combineBranch[i / 2].mesh = segment;
            combineBranch[i / 2].transform = Matrix4x4.identity;
            combineAll[i / 2].mesh = segment;
            combineAll[i / 2].transform = Matrix4x4.identity;
        }
        branchMesh.CombineMeshes(combineBranch);
        for (int i = 0; i < treePolygons.Count; i++)
        {
            combineAll[treeEdges.Count / 2 + i].mesh = GeneratePolygon(treePolygons[i]);
            combineAll[treeEdges.Count / 2 + i].transform = Matrix4x4.identity;
        }
        surfaceMesh.CombineMeshes(combineAll);
        return surfaceMesh;
    }

    public Mesh GetBranchMesh()
    {
        return branchMesh;
    }

    public Mesh ExtrudeEdge(int segments, OrientedPoint o1, OrientedPoint o2, Mesh2d crosssec, float s1, float s2,
        Color c1, Color c2)
    {
        Mesh prism = new Mesh();
        prism.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Color> colors = new List<Color>();
        for (int s = 0; s < segments; s++)
        {
            float t = s / (float)(segments - 1);
            OrientedPoint op = OrientedPoint.Lerp(o1, o2, t);
            float size = t * (s2 - s1) + s1;
            Color color = Color.Lerp(c1, c2, (float)s / segments);
            for (int i = 0; i < crosssec.VertexCount; i++)
            {
                verts.Add(op.RollToZero().LocalToWorldPos(crosssec.vertices[i].point * size));
                normals.Add(op.RollToZero().LocalToWorldVec(crosssec.vertices[i].normal));
                colors.Add(color);
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
        prism.SetColors(colors);
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
