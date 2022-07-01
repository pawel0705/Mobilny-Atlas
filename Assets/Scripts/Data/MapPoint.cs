using System;
using UnityEngine;

namespace Data.SceneReference {
    public class MapPoint : MonoBehaviour {

        public PointData data => _data;
        [SerializeField] PointData _data;
        
        GameObject spawned;

        void OnValidate() {
            gameObject.name = data.displayName;
        }

        void OnDestroy() {
            data.onDetected -= EnableModel;
            data.onDetectStopped -= DisableModel;
        }


        public void Init() {
            spawned = Instantiate(data.prefab, transform);
            DisableModel();
            
            data.onDetected += EnableModel;
            data.onDetectStopped += DisableModel;
        }


        public void EnableModel() {
            spawned.SetActive(true);
        }

        public void DisableModel() {
            spawned.SetActive(false);
        }
        
    }
    
}