using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Vuforia;

namespace Data.SceneReference {
    
    [CustomEditor(typeof(Zone))]
    [CanEditMultipleObjects]
    public class ZoneEditor : Editor {

        Tool lastTool = Tool.None;
        Zone[] zones;

        const float gizmosPointSize = 0.005f;
        const float upHeight = 0.05f;

        static int enableCounter = 0;
        static string mapPointsPropName = "_mapPoints";

        void OnEnable() {
            enableCounter++;
            if(lastTool == Tool.None) {
                lastTool = Tools.current;
            }
            Tools.current = Tool.None;
            zones = targets.Select(x => x as Zone).Where(x => x != null).ToArray();
        }

        void OnDisable() {
            enableCounter--;
            if (lastTool != Tool.None) {
                Tools.current = lastTool;
            }
        }
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (GUILayout.Button("Select Map Points")) {
                SelectMapPoints();
            }

            if (GUILayout.Button("Connect Overlapping Points")) {
                SetMapPointsFromBounds(zones);
            }
        }

        public static void SetMapPointsFromBounds(Zone[] zones) {
            MapPointsScene mapRef = zones[0].GetComponentInParent<MapPointsScene>();
            MapPoint[] points = mapRef.GetComponentsInChildren<MapPoint>(true);

            foreach(Zone z in zones) {
                Vector2 boundsSize2D = GetImageTargetBounds(z);
                Vector3 boundsSize = new Vector3(
                    boundsSize2D.x,
                    float.MaxValue,
                    boundsSize2D.y
                );
                Bounds b = new Bounds(z.transform.position, boundsSize);
                MapPoint[] contained = points
                    .Where(p => b.Contains(p.transform.position))
                    .ToArray();

                SerializedObject so = new SerializedObject(z);
                SerializedProperty prop = so.FindProperty(mapPointsPropName);

                SetArray(prop, contained);
                so.ApplyModifiedProperties();
            }


        }

        static void SetArray(SerializedProperty prop, UnityEngine.Object[] array) {
            prop.ClearArray();
            for (int i = 0; i < array.Length; i++) {
                prop.InsertArrayElementAtIndex(i);
                prop.GetArrayElementAtIndex(i).objectReferenceValue = array[i];
            }
        }


        [DrawGizmo(GizmoType.Selected | GizmoType.Active | GizmoType.InSelectionHierarchy)]
        static void DrawGizmos(Zone scr, GizmoType gizmoType) {
            Gizmos.color = new Color(0.6f, 0.0f, 0.8f);
            foreach (MapPoint point in scr.mapPoints.Where(p => p != null)) {
                Gizmos.DrawSphere(point.transform.position, gizmosPointSize);
            }

            DrawZoneBounds(scr);
        }

        static void DrawZoneBounds(Zone scr) {
            Vector2 bounds = GetImageTargetBounds(scr);
            Vector3 halfOffset = new Vector3(bounds.x / 2, 0, bounds.y / 2);
            Vector3 position = scr.transform.position;
            Vector3 topRight = position + halfOffset;
            Vector3 bottomLeft = position - halfOffset;
            Vector3 topLeft = position + new Vector3(halfOffset.x, 0, -halfOffset.z);
            Vector3 bottomRight = position+ new Vector3(-halfOffset.x, 0, halfOffset.z);

            Vector3 upOffset = Vector3.up * upHeight;

            //going up
            Gizmos.DrawLine(topRight, topRight + upOffset);
            Gizmos.DrawLine(bottomLeft, bottomLeft + upOffset);
            Gizmos.DrawLine(topLeft, topLeft + upOffset);
            Gizmos.DrawLine(bottomRight, bottomRight + upOffset);
            //across 1
            Gizmos.DrawLine(topLeft, topRight + upOffset);
            Gizmos.DrawLine(bottomRight, bottomLeft + upOffset);
            Gizmos.DrawLine(bottomLeft, topLeft + upOffset);
            Gizmos.DrawLine(topRight, bottomRight + upOffset);
            //across 2
            Gizmos.DrawLine(bottomRight, topRight + upOffset);
            Gizmos.DrawLine(topLeft, bottomLeft + upOffset);
            Gizmos.DrawLine(topRight, topLeft + upOffset);
            Gizmos.DrawLine(bottomLeft, bottomRight + upOffset);
            //up square
            Gizmos.DrawLine(topLeft + upOffset, topRight + upOffset);
            Gizmos.DrawLine(bottomRight + upOffset, bottomLeft + upOffset);
            Gizmos.DrawLine(bottomLeft + upOffset, topLeft + upOffset);
            Gizmos.DrawLine(topRight + upOffset, bottomRight + upOffset);
        }

        static Vector2 GetImageTargetBounds(Zone z) {
            Vector2? dim = z != null 
                ? z.GetComponentInParent<ImageTargetBehaviour>()?.GetSize() 
                : null;
          return dim.GetValueOrDefault();
        }


        void SelectMapPoints() {
            GameObject[] objArray = GetNotNullPoints().Select(p => p.gameObject).ToArray();
            Selection.objects = objArray;
        }


        IEnumerable<MapPoint> GetNotNullPoints() {
            foreach(Zone zone in zones) {
                foreach(MapPoint p in zone.mapPoints) {
                    if (p != null) {
                        yield return p;
                    }
                }
            }
        }
    }

}