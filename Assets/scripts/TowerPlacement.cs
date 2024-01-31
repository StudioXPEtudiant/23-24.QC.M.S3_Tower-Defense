using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    public GameObject towerPrefab;
    public LayerMask placementLayer;

    private bool isPlacingTower = false;

    // Attach this method to the Button's onClick event in the Unity Editor
    public void OnPlaceTowerButtonClick()
    {
        isPlacingTower = !isPlacingTower; // Toggle the placement state
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlacingTower)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayer))
            {
                if (Input.GetMouseButtonDown(0)) // Check if the mouse button is pressed
                {
                    Debug.Log("Mouse button pressed");
                    PlaceTower(hit.point);
                }
            }
        }
    }

    void PlaceTower(Vector3 position)
    {
        Debug.Log("Placing Tower at: " + position);

        GameObject newTower = Instantiate(towerPrefab, position, Quaternion.identity);

        // Optionally, you can add more customization for the tower here
    }
}