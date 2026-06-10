using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private LayerMask groundLayer;
    private Unit selectedUnit;

    void Update()
    {
        HandleUnitSelection();
        HandleMovement();
    }

    void HandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
            {
                Unit unit = hit.collider.GetComponent<Unit>();
                if (unit != null)
                {
                    SelectUnit(unit);
                }
            }
        }
    }

    void HandleMovement()
    {
        if (Input.GetMouseButtonDown(1) && selectedUnit != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, groundLayer))
            {
                selectedUnit.MoveTo(hit.point);
            }
        }
    }

    void SelectUnit(Unit unit)
    {
        selectedUnit = unit;
        Debug.Log("Selected unit: " + unit.gameObject.name);
    }
}
