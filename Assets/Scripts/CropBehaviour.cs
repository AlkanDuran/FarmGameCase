using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{

    int landID;
    
    SeedData seedToGrow;

    [Header("Stages of Life")]
    [SerializeField] private GameObject rotten;

    private GameObject seed;
    private GameObject seedling;
    private GameObject harvestable;


    int growth;
   
    int maxGrowth; 

    int maxHealth = GameTimestamp.HoursToMinutes(48); 

    int health;

    public enum CropState
    {
        Seed, Seedling, Harvestable, Rotten
    }
 
    public CropState cropState;

   
    public void Plant(int landID, SeedData seedToGrow)
    {
        LoadCrop(landID, seedToGrow, CropState.Seed, 0, 0);
        LandManager.Instance.RegisterCrop(landID, seedToGrow, cropState, growth, health); 
    }

    public void LoadCrop(int landID, SeedData seedToGrow, CropState cropState, int growth, int health)
    {
        this.landID = landID;
        this.seedToGrow = seedToGrow;
        seedling = Instantiate(seedToGrow.seedling, transform);
        seed = Instantiate(seedToGrow.seed, transform);
        harvestable = Instantiate(seedToGrow.harvastable, transform);
        int hoursToGrow = GameTimestamp.DaysToHours(seedToGrow.daysToGrow);
        maxGrowth = GameTimestamp.HoursToMinutes(hoursToGrow);
        this.growth = growth;
        this.health = health; 
        SwitchState(cropState);

    }


    public void Grow()
    {

        growth++;

        if(health < maxHealth)
        {
            health++;
        }


        if(growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seedling); 
        }


        if(growth >= maxGrowth && cropState == CropState.Seedling)
        {
            SwitchState(CropState.Harvestable);
        }

        LandManager.Instance.OnCropStateChange(landID, cropState, growth, health);
    }


    public void Rot()
    {
        health--;
 
        if(health <= 0 )
        {
            SwitchState(CropState.Rotten);
        }

        LandManager.Instance.OnCropStateChange(landID, cropState, growth, health);
    }


  
    void SwitchState(CropState stateToSwitch)
    {

        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);
        rotten.SetActive(false);

        switch (stateToSwitch)
        {
            case CropState.Seed:

                seed.SetActive(true);
                health = maxHealth;
                break;
            case CropState.Seedling:
  
                seedling.SetActive(true);
                 
                break;
            case CropState.Harvestable:
             
                harvestable.SetActive(true);
                break;
            case CropState.Rotten:
         
                rotten.SetActive(true);
                break;
        }

        cropState = stateToSwitch; 
    }

    public void RemoveCrop()
    {
        LandManager.Instance.DeregisterCrop(landID);
        Destroy(gameObject);
    }

    
}