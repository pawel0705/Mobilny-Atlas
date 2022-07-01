using UnityEditor;
using UnityEngine;

namespace Data.SceneReference {

    [CustomEditor(typeof(MapPointsScene))]
    public class MapPointsSceneEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            MapPointsScene points = (target as MapPointsScene);
            
            if (GUILayout.Button("Connect Overlapping Points")) {
                ZoneEditor.SetMapPointsFromBounds(points.GetComponentsInChildren<Zone>());
            }
        }

    }
}