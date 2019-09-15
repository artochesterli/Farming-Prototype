using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public bool Scattered;
    public GameObject Plant;

    public Sprite ScatteredSprite;
    public Sprite PlantedSprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrowPlant()
    {
        Plant.SetActive(true);
        Plant.transform.parent = transform.parent;
        Destroy(gameObject);
    }

    public void Captured()
    {
        Scattered = false;
        GetComponent<SpriteRenderer>().sprite = PlantedSprite;
    }
}
