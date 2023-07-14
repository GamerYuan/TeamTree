using UnityEngine;

public class Gizmosfs : MonoBehaviour
{
    public static void DrawWireCircle(Vector3 pos, Quaternion rot, float radius, float height = 0f, int detail = 32)
    {
        Vector3[] points3d = new Vector3[detail];
        for (int i = 0; i < detail; i++)
        {
            float t = i / (float)detail;
            float angRad = 2 * t * Mathf.PI;

            Vector3 point3d = (Vector3)Mathfs.UnitVectorFromAngle(angRad) * radius + new Vector3(0, 0, height);

            points3d[i] = pos + rot * point3d;
        }

        //Draw circular points
        for (int i = 0; i < detail - 1; i++)
        {
            Gizmos.DrawLine(points3d[i], points3d[i + 1]);
        }
        Gizmos.DrawLine(points3d[detail - 1], points3d[0]);
    }
}
