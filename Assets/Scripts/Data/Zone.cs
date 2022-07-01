using System;
using UnityEngine;


namespace Data.SceneReference {
    public class Zone : MonoBehaviour {

        public MapPoint[] mapPoints => _mapPoints; 
        [SerializeField] MapPoint[] _mapPoints;
        
        public event Action onMapPointChange;
        public MapPoint activePoint {
            private set {
                MapPoint prev = _activePoint;
                _activePoint = value;
                if (prev != _activePoint) {
                    onMapPointChange?.Invoke();
                }
            }
            get => _activePoint;
        }
        MapPoint _activePoint;
        public float activePointSqDistance { private set; get; }

        Camera cam;
        static Vector3 viewportCentre = new Vector3(0.5f, 0.5f, 0.0f);

        void OnEnable() {
            cam = Camera.main;
            Debug.Log($"Zone {transform.parent.name} on");
        }

        void OnDisable() {
            activePoint = null;
            Debug.Log($"Zone {transform.parent.name} off");
        }

        void Update() {
            Ray ray = cam.ViewportPointToRay(viewportCentre);
            Plane plane = new Plane(transform.up, transform.position);
            if (plane.Raycast(ray, out float dist)) {
                Vector3 point = ClosestMapPointFromRay(ray, dist);
                Debug.DrawLine(ray.origin, point, Color.green);
            }
            else {
                Debug.DrawRay(ray.origin, ray.direction, Color.red);
            }

        }

        private Vector3 ClosestMapPointFromRay(Ray ray, float dist) {
            Vector3 point = ray.GetPoint(dist);
            MapPoint minMp = GetClosestMapPoint(point);
            activePoint = minMp;
            return point;
        }

        MapPoint GetClosestMapPoint(Vector3 point) {
            float minDist = float.MaxValue;
            MapPoint minMp = null;
            foreach (MapPoint mp in _mapPoints) {
                float sqDist = (mp.transform.position - point).sqrMagnitude;
                if (sqDist < minDist) {
                    minMp = mp;
                    minDist = sqDist;
                }
            }

            activePointSqDistance = minDist;
            return minMp;
        }


    }
}