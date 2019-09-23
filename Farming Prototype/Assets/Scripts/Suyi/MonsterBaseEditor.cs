using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// [CustomEditor(typeof(MonsterFlower))]
// public class FlowerEditor : Editor
// {
//     private MonsterBase3D mb;

//     private void OnEnable()
//     {
//         mb = this.target as MonsterBase3D;
//     }
//     private void OnSceneGUI()
//     {
//         Handles.color = Color.red;
//         foreach (Sight s in mb.MonsterData.Sights)
//         {
//             Handles.DrawSolidArc(mb.transform.position, mb.transform.up, mb.transform.forward + Vector3.forward * s.AngleOffset, s.Angle / 2f, s.Radius);
//             Handles.DrawSolidArc(mb.transform.position, -mb.transform.up, mb.transform.forward + Vector3.forward * s.AngleOffset, s.Angle / 2f, s.Radius);
//         }
//     }
// }

[CustomEditor(typeof(MonsterBuffalo))]
public class BuffaloEditor : Editor
{
    private MonsterBase3D mb;

    private void OnEnable()
    {
        mb = this.target as MonsterBase3D;
    }
    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        foreach (Sight s in mb.MonsterData.Sights)
        {
            Handles.DrawSolidArc(mb.transform.position, mb.transform.up, mb.transform.forward + Vector3.forward * s.AngleOffset, s.Angle / 2f, s.Radius);
            Handles.DrawSolidArc(mb.transform.position, -mb.transform.up, mb.transform.forward + Vector3.forward * s.AngleOffset, s.Angle / 2f, s.Radius);
        }
    }
}
