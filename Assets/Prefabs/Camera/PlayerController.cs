using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    public enum ControlType
    {
        MouseAndKeyboard,
        Touch,
        JoyStick
    }

    public ControlType controlType;

    public Director director;

    //private int curDay;
    private Entity placing = null;
    private bool isPlacing = false;

    public RectTransform dragSelectUI;

    public StructureController[] strucProto;
    public UnitController[] unitProto;
    UnitController[] SelectedUnits= {};

    Vector3 mouseClickDown, mouseClickUp, mouseClickDownUI;
    //Camera camera;

	// Use this for initialization
	void Start () {
        director = FindObjectOfType<Director>();
	}
	
	// Update is called once per frame
	void LateUpdate () {

        if (controlType == ControlType.MouseAndKeyboard){
            onLeftMouseClick();
            onRightMouseClick();
        }
        
	}

    void onLeftMouseClick()
    {
        //On Left MouseClick
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            mouseClickDown = getMousePosition();
            mouseClickDownUI = Input.mousePosition;
            dragSelectUI.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            showDrag(mouseClickDown, getMousePosition());
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseClickDownUI = Vector3.zero;
            //Mouse is over UI
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            mouseClickUp = getMousePosition();
            dragSelectUI.gameObject.SetActive(false);

            //Select Unit
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!isPlacing)//Selecting Unit
            {
                if (mouseClickUp == mouseClickDown)//Mouse clicked
                {
                    selectUnit(ray);
                }
                else //Mouse dragged
                {
                    selectBulkUnits(mouseClickDown, mouseClickUp);
                }
            }
            else//Placing
            {
                placeStruc(ray);
            }

        }
    }

    void onRightMouseClick()
    {
        //On Right Mouse Click
        if (Input.GetMouseButtonUp(1))
        {
            //Clear previus commands 
            foreach(UnitController u in SelectedUnits)
            {
                u.cancelNav();
            }
            //Set Selected Unit Waypoints
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                ClickHitBox clickHitBox;
                if (Type<ClickHitBox>.isType(hit.collider.gameObject, out clickHitBox))
                {
                    EnemyController enemy;
                    Critter critter;
                    ResourceNode node;
                    if (Type<EnemyController>.isType(clickHitBox.parent.gameObject, out enemy))
                    {
                        attack(enemy);
                    }
                    else if (Type<Critter>.isType(clickHitBox.parent.gameObject, out critter))
                    {
                        attack(critter);
                    }
                    else if (Type<ResourceNode>.isType(clickHitBox.parent.gameObject, out node))
                    {
                        targetResource(node);
                    }
                    
                }
                else
                {
                    moveSelectedUnits(ray);
                    attack((EnemyController)null);
                }
            }
            
        }
    }

    void updateSelectedUnits()
    {
        int unitsFound = 0;
        UnitController[] allUnits = FindObjectsOfType<UnitController>();
        UnitController[] foundUnits = new UnitController[allUnits.Length];
        foreach (UnitController u in allUnits)
        {
            if (u.isSelected)
            {
                foundUnits[unitsFound] = u;
                unitsFound++;
            }
        }
        SelectedUnits = new UnitController[unitsFound];
        for (int i = 0; i < unitsFound; i++)
            SelectedUnits[i] = foundUnits[i];
    }

    public void buildStruc(int struc)
    {
        if (strucProto[struc].buildingMaterialCost <= Resource.BuildingMaterial.amount && strucProto[struc].powerCost+Resource.Power.amount <= Resource.Power.capacity)
        {
            placing = strucProto[struc];
            isPlacing = true;
        }
    }

    public void placeStruc(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<Entity>() == null)
            {
                Instantiate(placing, hit.point, Quaternion.identity);
                if (placing.GetComponent<StructureController>())
                {
                    Resource.addResource(Resource.Type.Power, placing.gameObject.GetComponent<StructureController>().powerCost);
                    Resource.addResource(Resource.Type.BuildingMaterial, -placing.GetComponent<StructureController>().buildingMaterialCost);
                }
                isPlacing = false;
                UILog.AddText("Placing: " + placing.name);
                placing = null;
            }
        }
    }

    public Vector3 getMousePosition()
    {
        Vector3 mousePos;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            mousePos = hit.point;
        }

        else mousePos = Vector3.zero;

        return mousePos;
    }

    public void showDrag(Vector3 startDrag, Vector3 endDrag)
    {
        RayBox box = new RayBox(startDrag, endDrag);
        box.drawBox();
    }

    void OnGUI()
    {
        if (mouseClickDownUI != Vector3.zero)
        {
            Rect rect = RayBox.Utils.GetScreenRect(mouseClickDownUI, Input.mousePosition);
            RayBox.Utils.DrawScreenRect(rect, new Color(0.8f, 0.95f, 0.8f, 0.25f));
            RayBox.Utils.DrawScreenRectBorder(rect, 2, new Color(0, 0.95f, 0));
        }
    }


    public void moveSelectedUnits(Ray ray)
    {
        RaycastHit hit;
        Vector3 mouseToGround = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
            mouseToGround = new Vector3(hit.point.x, 0, hit.point.z);
        if (hit.collider.GetComponent<GuardPostController>())
        {
            hit.collider.GetComponent<GuardPostController>().loadUnits(SelectedUnits);
        }
        else if (hit.collider.GetComponent<EnemyController>())
        {
            if (SelectedUnits.Length >= 1)
                foreach (UnitController u in SelectedUnits)
                {
                    u.target = hit.collider.GetComponent<EnemyController>().transform;
                }
        }
        else
        {
            if (SelectedUnits.Length >= 1)
                foreach (UnitController u in SelectedUnits)
                {
                    if (u.atGuard())
                    {
                        u.guard.unloadSlot();
                    }
                    u.setWaypoint(mouseToGround);
                }
        }
    }

    public void buildUnit(int unit)
    {
        if (director.allUnits.Length < Resource.Food.amount)
        {
            placing = unitProto[unit];
            isPlacing = true;
        }
    }

    public void clearSelectedUnits()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && SelectedUnits.Length >= 1)
            foreach (UnitController u in SelectedUnits)
                u.isSelected = false;
    }

    public void selectUnit(Ray ray)
    {
        RaycastHit hit;
        GameObject selected = null;


        if (Physics.Raycast(ray, out hit))
        {
            selected = hit.collider.gameObject;
        }

        if (selected.GetComponent<UnitController>())
        {
            clearSelectedUnits();
            selected.GetComponent<UnitController>().isSelected = !selected.GetComponent<UnitController>().isSelected;
            updateSelectedUnits();
        }
    }

    public void selectBulkUnits(Vector3 startDrag, Vector3 endDrag)
    {
        clearSelectedUnits();
        RayBox rayBox = new RayBox(startDrag, endDrag);
        rayBox.drawBox();
        foreach (Collider col in rayBox.drawOverlapBox())
        {
            if (col.gameObject.GetComponent<UnitController>())
                col.gameObject.GetComponent<UnitController>().isSelected = true;
        }
        updateSelectedUnits();
    }

    public void attack(EnemyController enemy)
    {
        foreach (UnitController unit in SelectedUnits)
        {
            if (enemy == null)
                unit.target = null;
            else
                unit.target = enemy.transform;
        }
    }

    public void attack(Critter critter)
    {
        foreach (UnitController unit in SelectedUnits)
        {
            if (critter == null)
                unit.target = null;
            else
                unit.target = critter.transform;
        }
    }

    public void targetResource(ResourceNode node)
    {
        foreach (UnitController unit in SelectedUnits)
        {
            unit.setWaypoint(node.transform.position);
            unit.target = node.transform;
            unit.targetNode = node;
            unit.harvesting = true;
            unit.harvestNode(node);
        }
    }

}
