using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Data.SceneReference
{
    public class MapPointsScene : MonoBehaviour
    {
        MapPoint[] mapPoints;
        Zone[] zones;

        const string noCountryText = "Hover over a country";

        //CAN BE NULL PointData
        public event Action<PointData> onDataChanged;
        public event Action<PointData> onDataLost;

        private CountryPoller countryPoller;

        [SerializeField]
        private GameObject infoButton;

        [SerializeField]
        private GameObject closeButton;

        [SerializeField]
        private GameObject infoPlane;

        public PointData lastActiveData
        {
            private set
            {
                PointData oldValue = _lastActiveData;
                _lastActiveData = value;
                if (oldValue != _lastActiveData)
                {
                    if (oldValue != null)
                    {
                        oldValue.OnDetectStop();
                    }
                    if (_lastActiveData != null)
                    {
                        _lastActiveData.OnDetect();
                    }
                    onDataLost?.Invoke(oldValue);
                    onDataChanged?.Invoke(_lastActiveData);
                }
            }
            get
            {
                return _lastActiveData;
            }
        }
        PointData _lastActiveData;

        void Awake()
        {
            //TEMP
            onDataChanged += (x) => Debug.Log(x?.displayName);

            this.SetupGUI();

            mapPoints = GetComponentsInChildren<MapPoint>();
            foreach (MapPoint mapPoint in mapPoints)
            {
                mapPoint.Init();
            }

            zones = GetComponentsInChildren<Zone>(true);
            foreach (Zone zone in zones)
            {
                zone.onMapPointChange += ZoneOnMapPointChange;
            }
        }

        void OnDestroy() {
            foreach (Zone zone in zones)
            {
                zone.onMapPointChange -= ZoneOnMapPointChange;
            }
        }

        void ZoneOnMapPointChange()
        {
            IEnumerable<Zone> activePointZones = zones.Where(zone => zone.activePoint != null);
            if (!activePointZones.Any())
            {
                lastActiveData = null;
                return;
            }
            Zone minDistZone = activePointZones.First();
            foreach (Zone zone in activePointZones.Skip(1))
            {
                if (zone.activePointSqDistance < minDistZone.activePointSqDistance)
                {
                    minDistZone = zone;
                }
            }
            lastActiveData = minDistZone.activePoint.data;
        }

        private void SetupGUI()
        {
            onDataChanged += (x) => this.SetupInfoButton(x);
            onDataChanged += (x) => this.SetupInfoPlane(x);

            onDataLost += (x) => this.ClearInfoButton();

            var isInfoButton = this.infoButton.TryGetComponent<Button>(out Button buttonInfoComponent);

            if (isInfoButton == true)
            {
                buttonInfoComponent.onClick.AddListener(this.OpenInfoPlane);
            }

            var isCloseButton = this.closeButton.TryGetComponent<Button>(out Button buttonCloseComponent);

            if (isCloseButton == true)
            {
                buttonCloseComponent.onClick.AddListener(this.ClearInfoPlane);
            }
        }

        private void SetupInfoButton(PointData pointData)
        {
            if (this.infoButton == null)
            {
                return;
            }

            this.infoButton.SetActive(true);

            var isButton = this.infoButton.TryGetComponent<Button>(out Button buttonComponent);

            if (isButton == true)
            {
                var textMesh = buttonComponent.GetComponentInChildren<TextMeshProUGUI>();
                if (pointData != null) {
                    textMesh.SetText(pointData.displayName);
                }
                else {
                    textMesh.text = noCountryText;
                }
            }
        }

        private void ClearInfoButton()
        {
            if (this.infoButton == null)
            {
                return;
            }

            var isButton = this.infoButton.TryGetComponent<Button>(out Button buttonComponent);

            if (isButton == true)
            {
                var textMesh = buttonComponent.GetComponentInChildren<TextMeshProUGUI>();
                textMesh.SetText("<country name>");
            }


            this.infoButton.SetActive(false);
        }

        private void SetupInfoPlane(PointData pointData)
        {
            if (this.infoPlane == null || pointData == null)
            {
                return;
            }

            this.countryPoller = new CountryPoller(pointData.displayName);

            var country = countryPoller.getCountry();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<b>Nazwa: </b>" + country.name + "\n");
            stringBuilder.Append("<b>Stolica: </b>" + country.capital + "\n");
            stringBuilder.Append("<b>Język: </b>" + country.language + "\n");
            stringBuilder.Append("<b>Powierzchnia: </b>" + country.totalArea + " km²" + "\n");
            stringBuilder.Append("<b>Populacja: </b>" + country.population + "\n");
            stringBuilder.Append("<b>PKB na mieszkańca: </b>" + country.GDPPerCapita + " USD" + "\n");
            stringBuilder.Append("<b>Śmieszny fakt: </b>" + country.funFact);

            var textMesh = this.infoPlane.GetComponentInChildren<TextMeshProUGUI>();
            textMesh.SetText(stringBuilder.ToString());
        }

        private void ClearInfoPlane()
        {
            if (this.infoPlane == null)
            {
                return;
            }

            this.infoPlane.SetActive(false);
            this.closeButton.SetActive(false);
        }

        private void OpenInfoPlane()
        {
            var infopaneActive = this.infoPlane.activeSelf;

            if (infopaneActive == true)
            {
                this.infoPlane.SetActive(false);
                this.closeButton.SetActive(false);
            }
            else
            {
                this.infoPlane.SetActive(true);
                this.closeButton.SetActive(true);
            }
        }
    }
}