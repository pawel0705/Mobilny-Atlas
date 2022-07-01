using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class PointData {

    public string displayName => name;
    [SerializeField] string name;
  
    public event Action onDetected;
    public event Action onDetectStopped;

    public GameObject prefab => _prefab;
    [SerializeField] GameObject _prefab;

    public void OnDetect() {
        onDetected?.Invoke();
    }

    public void OnDetectStop() {
        onDetectStopped?.Invoke();
    }

}