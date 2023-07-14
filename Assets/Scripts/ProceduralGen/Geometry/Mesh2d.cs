using UnityEngine;

[CreateAssetMenu]
public class Mesh2d : ScriptableObject
{
    [System.Serializable]
    public class Vertex
    {
        public Vector2 point;
        public Vector2 normal;
        public float u;

        public Vertex(Vector2 point, Vector2 normal)
        {
            this.point = point;
            this.normal = normal;
        }
    }

    public Vertex[] vertices;
    public int[] lineIndices;

    public int VertexCount => vertices.Length;
    public int LineCount => lineIndices.Length;

    public Mesh2d(Vertex[] vertices, int[] lineIndices)
    {
        this.vertices = vertices;
        this.lineIndices = lineIndices;
    }

}
