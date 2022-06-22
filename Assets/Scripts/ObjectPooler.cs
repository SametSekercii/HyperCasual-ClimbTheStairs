using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    public GameObject LayerPrefab;
    public GameObject SweatingPrefab;
    private List<GameObject> Layers = new List<GameObject>();
    private List<GameObject> SweatingEffects = new List<GameObject>();
    int poolamountlayer = 100;
    int poolamountsweating = 20;

    void Start()
    {
        Instance = this;
        CreateLayer();
        CreateSweatingEffects();
       
    }
    void CreateLayer()
    {
        for(int i = 0; i < poolamountlayer; i++)
        {
            var CreatedLayer=Instantiate(LayerPrefab);
            CreatedLayer.SetActive(false);
            Layers.Add(CreatedLayer);
        }
    }
    void CreateSweatingEffects()
    {
        for (int i = 0; i < poolamountsweating; i++)
        {
            var CreatedEffects = Instantiate(SweatingPrefab);
            CreatedEffects.SetActive(false);
            SweatingEffects.Add(CreatedEffects);
        }
    }
    public GameObject GetSweating()
    {
        for (int i = 0; i < poolamountsweating; i++)
        {
            if (!SweatingEffects[i].activeSelf)
            {
                return SweatingEffects[i];
            }
        }
        return null;

    }

    public GameObject GetLayers()
    {
        for(int i = 0; i <poolamountlayer; i++)
        {
            if (!Layers[i].activeSelf)
            {
                return Layers[i];
            }
        }
        return null;

    }
    
    
}
