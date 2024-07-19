using UnityEngine;
using DG.Tweening;

public class Land : MonoBehaviour, IInteractable, ISelectable, ITimeTracker
{
    public enum LandStatus
    {
        Dry, Plowed, Wet
    }
    
    [Header("Crops")]
    [SerializeField] private GameObject _cropPrefab;
    
    [Header("Land Settings")]
    [SerializeField] private LandStatus _landStatus;
    [SerializeField] private Material _dryMat;
    [SerializeField] private Material _plowedMat;
    [SerializeField] private Material _wetMat;
    
    [Space] [Header("Interact Settings")] 
    [SerializeField] private string _textOnAim;
    [SerializeField] private GameObject _selectedObject;
    
    private new Renderer _renderer;
    private CropBehaviour _cropPlanted = null;
    private GameTimestamp _timeWatered;

    public int id; 
    
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        SwitchLandStatus(LandStatus.Dry);
        Select(false);
        TimeManager.Instance.RegisterTracker(this);
    }

    public void LoadLandData(LandStatus statusToSwitch, GameTimestamp lastWatered)
    {
        _landStatus = statusToSwitch;
        _timeWatered = lastWatered;
        Material materialToSwitch = _dryMat;
        switch (statusToSwitch)
        {
            case LandStatus.Dry:
                materialToSwitch = _dryMat;
                break;
            case LandStatus.Plowed:
                materialToSwitch = _plowedMat;
                break;
            case LandStatus.Wet:
                materialToSwitch = _wetMat;
                break;
        }
        _renderer.material = materialToSwitch;
    }

    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        _landStatus = statusToSwitch;

        Material materialToSwitch = _dryMat; 

        switch (statusToSwitch)
        {
            case LandStatus.Dry:
                materialToSwitch = _dryMat;
                break;
            case LandStatus.Plowed:
                materialToSwitch = _plowedMat;
                break;
            case LandStatus.Wet:
                materialToSwitch = _wetMat;

                _timeWatered = TimeManager.Instance.GetGameTimestamp(); 
                break; 

        }
        
        _renderer.material = materialToSwitch; 

        LandManager.Instance.OnLandStateChange(id, _landStatus, _timeWatered);
    }

    public void Select(bool toggle)
    {
        _selectedObject.SetActive(toggle);
    }

    public void Interact(float interactionTime = 0)
    {
        var toolSlot = InventoryManager.Instance.GetEquippedSlotItem();
        
        if (toolSlot == null)
        {
            return; 
        }
        
        var equipmentTool = toolSlot as EquipmentData; 
        
        if(equipmentTool != null)
        {
            var toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case EquipmentData.ToolType.Hoe:
                    if(_landStatus==LandStatus.Dry){
                        AnimateLand();
                        SwitchLandStatus(LandStatus.Plowed);
                    }
                    break;
                case EquipmentData.ToolType.WateringCan:
                    if(_landStatus==LandStatus.Plowed) {
                        AnimateLand();
                        SwitchLandStatus(LandStatus.Wet);
                    }
                    break;
                case EquipmentData.ToolType.Shovel:
                    if(_cropPlanted != null)
                    {
                        _cropPlanted.RemoveCrop();
                    }
                    break;
            }
            return; 
        }
        
        var seedTool = toolSlot as SeedData; 
        if(seedTool != null && _landStatus != LandStatus.Dry && _cropPlanted == null)
        {
            SpawnCrop();
            _cropPlanted.Plant(id, seedTool);

            InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot());
        }
    }

    public void AnimateLand(){
        transform.DOJump(transform.position,.15f,1,.2f).SetEase(Ease.Linear);
    }

    public CropBehaviour SpawnCrop()
    {
        var cropObject = Instantiate(_cropPrefab, transform);
        cropObject.transform.position = new Vector3(transform.position.x, transform.position.y+.5f, transform.position.z);

        _cropPlanted = cropObject.GetComponent<CropBehaviour>();
        return _cropPlanted; 
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
       
        if(_landStatus == LandStatus.Wet)
        {
           
            int hoursElapsed = GameTimestamp.CompareTimestamps(_timeWatered, timestamp);
            Debug.Log(hoursElapsed + " hours since this was watered");

           
            if(_cropPlanted != null)
            {
                _cropPlanted.Grow();
            }

            if(hoursElapsed > 24)
            {
                SwitchLandStatus(LandStatus.Plowed);
            }
        }
        if(_landStatus != LandStatus.Wet && _cropPlanted != null)
        {
            
            _cropPlanted.Rot();
    
        }
    }
    
    public string Text_OnAim() => _textOnAim;
    
    public float GetInteractionTime()
    {
        var toolSlot = InventoryManager.Instance.GetEquippedSlotItem();
        if (toolSlot == null) return 0f;

        var equipmentTool = toolSlot as EquipmentData;
        if (equipmentTool != null)
        {
            switch (equipmentTool.toolType)
            {
                case EquipmentData.ToolType.Hoe when _landStatus is LandStatus.Plowed or LandStatus.Wet:
                case EquipmentData.ToolType.Shovel when _cropPlanted == null:
                case EquipmentData.ToolType.WateringCan when _landStatus is LandStatus.Wet or not LandStatus.Plowed:
                    return 0;
                default:
                    return equipmentTool.interactionTime;
            }
        }

        return 0;
    }

    public LandStatus GetLandStatus() => _landStatus;
}