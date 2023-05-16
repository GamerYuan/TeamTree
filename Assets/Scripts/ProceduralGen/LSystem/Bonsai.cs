using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bonsai : MonoBehaviour
{
    LSystem lsystem;

    [SerializeField]
    private List<Rule> rules;
    [SerializeField]
    private string axiomString;

    List<TreeVert> treeVertices = new List<TreeVert>();
    List<int> treeEdges = new List<int>();

    private void Awake()
    {
        lsystem = new LSystem(rules, axiomString);
        treeVertices = lsystem.generateTreeVerts(new OrientedPoint(transform.position, transform.rotation)); 
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 50, 50), "update"))
        {
            lsystem.ApplyRules();
            treeVertices = lsystem.generateTreeVerts(new OrientedPoint(transform.position, transform.rotation));
            treeEdges = lsystem.generateTreeEdges();
        }
    }

    private void OnDrawGizmosSelected()
    {
        TreeVert[] verts = treeVertices.ToArray();
        int[] edges = treeEdges.ToArray();
        Gizmos.color = Color.blue;
        foreach(TreeVert treeVert in verts)
        {
            Gizmos.DrawSphere(treeVert.point.LocalToWorldPos(transform.position), 0.05f);
        }
        for(int i = 0; i < edges.Length; i+= 2)
        {
            Gizmos.DrawLine(verts[edges[i]].point.pos, verts[edges[i + 1]].point.pos);
        }
    }
}
