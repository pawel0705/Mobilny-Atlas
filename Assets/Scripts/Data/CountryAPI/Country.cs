using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CountryPoller
{
    private CSVLoader.CountryRecord country;

    public CSVLoader.CountryRecord getCountry()
    {
        return country;
    }
    public CountryPoller(string name)
    {
        country = new CSVLoader.CountryRecord();
        var countries = CSVLoader.getCountryList();
        foreach (var t in countries.Where(t => t.name == name))
        {
            country = t;
        }
    }
}
public class Country : MonoBehaviour
{
    private CountryPoller country;
    void Start()
    {
        country = new CountryPoller(gameObject.name);
        Debug.Log("Ciekawostka o kraju: " + country.getCountry().funFact);
    }
    
}
