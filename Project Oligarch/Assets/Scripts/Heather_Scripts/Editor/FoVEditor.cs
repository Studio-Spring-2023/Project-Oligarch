using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldofView))]
public class FoVEditor : Editor
{
    private void OnSceneGUI ( )
    {
        FieldofView FoV = (FieldofView)target;

        
        Handles.color = Color.white;
        Handles.DrawWireArc (FoV.transform.position , Vector3.up , Vector3.forward , 0 ,FoV.viewRadius );

        Vector3 viewAngleA =FoV.DirFromAngle ( -FoV.viewAngle / 2 , false );
        Vector3 viewAngleB =FoV.DirFromAngle (FoV.viewAngle / 2 , false );
        Handles.DrawLine (FoV.transform.position ,FoV.transform.position + viewAngleA *FoV.viewRadius );
        Handles.DrawLine (FoV.transform.position ,FoV.transform.position + viewAngleB *FoV.viewRadius );
    }

}
