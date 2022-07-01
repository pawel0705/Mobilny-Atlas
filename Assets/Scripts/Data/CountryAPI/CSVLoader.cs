using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CSVLoader : MonoBehaviour
    {
        public TextAsset textAssetData;
        
        [Serializable]
        public class CountryRecord
        {
            public string name;
            public string capital;
            public string language;
            public int totalArea; // km^2
            public int population;
            public int GDPPerCapita; // USD
            public string funFact;
        }

        private static List<CountryRecord> countryList = new List<CountryRecord>();

        public static List<CountryRecord> getCountryList()
        {
            return countryList;
        }
        public void Awake()
        {
            LoadCSV();
        }
        private void LoadCSV()
        {
            var data = textAssetData.text.Split('\n');
            var countrySize = data.Length - 1; // amount of data minus "info" row
            for (var i = 0; i < countrySize; ++i)
            {
                // country record
                var record = data[i + 1].Split(',');
                var country = new CountryRecord
                {
                    name = record[0],
                    capital = record[1],
                    language = record[2],
                    totalArea = int.Parse(record[3]),
                    population = int.Parse(record[4]),
                    GDPPerCapita = int.Parse(record[5]),
                    funFact = record[6].TrimEnd('\r')
                };
                countryList.Add(country);
            }
        }

    }