using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;
    private PlayerControls playerControls;

    protected override void Awake()
    {
        base.Awake();  // Ensures Singleton behavior

        // Initialize playerControls if it hasn't been initialized yet
        if (playerControls == null)
        {
            playerControls = new PlayerControls();  // Instantiate the PlayerControls if not done already
            Debug.Log("playerControls initialized in Awake.");
        }

        // Check if the instance is initialized before doing anything else
        if (Instance == null)
        {
            Debug.LogError("ActiveInventory Singleton is not initialized!");
        }
    }

    private void Start()
    {
        // Ensure playerControls is not null before using it
        if (playerControls == null)
        {
            Debug.LogError("playerControls is not initialized!");
            return;
        }

        // Ensure that the ActiveInventory.Instance is valid before attempting to toggle
        if (ActiveInventory.Instance == null)
        {
            Debug.LogError("ActiveInventory Singleton is not initialized at Start!");
            return;
        }

        // Bind input action only if playerControls is properly initialized
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
    }

    private void OnEnable()
    {
        // Ensure playerControls is initialized before enabling it
        if (playerControls == null)
        {
            Debug.LogError("playerControls is not initialized in OnEnable!");
            return;
        }

        // Enable the input controls
        playerControls.Enable();
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighlight(0);
    }

    private void ToggleActiveSlot(int numValue)
    {
        // Ensure that the ActiveInventory.Instance is valid before attempting to toggle
        if (ActiveInventory.Instance == null)
        {
            Debug.LogError("ActiveInventory.Instance is null. Cannot toggle active slot.");
            return;
        }
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        if (indexNum < 0 || indexNum >= transform.childCount)
        {
            Debug.LogError($"Index {indexNum} is out of range for inventory slots!");
            return;
        }

        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in transform)
        {
            if (inventorySlot.childCount > 0)
            {
                inventorySlot.GetChild(0).gameObject.SetActive(false);
            }
        }

        Transform targetSlot = transform.GetChild(indexNum);
        if (targetSlot.childCount > 0)
        {
            targetSlot.GetChild(0).gameObject.SetActive(true);
        }

        ChangePlayerAtk();
    }

    private void ChangePlayerAtk()
    {
        // Check if ActiveWeapon.Instance is initialized
        if (PlayerAtk.Instance == null)
        {
            Debug.LogError("ActiveWeapon.Instance is null! Ensure ActiveWeapon is initialized.");
            return;
        }

        // Destroy the current active weapon
        if (PlayerAtk.Instance.CurrentPlayerAtk != null)
        {
            Destroy(PlayerAtk.Instance.CurrentPlayerAtk.gameObject);
        }

        // Get the inventory slot from the active slot index
        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform?.GetComponentInChildren<InventorySlot>();

        if (inventorySlot == null)
        {
            Debug.LogError($"No InventorySlot found in child {activeSlotIndexNum}!");
            PlayerAtk.Instance.WeaponNull(); // Handle no weapon case
            return;
        }

        // Retrieve the weapon info
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();

        if (weaponInfo == null || weaponInfo.weaponPrefab == null)
        {
            Debug.LogWarning("No weapon info or weapon prefab found in the selected inventory slot.");
            PlayerAtk.Instance.WeaponNull(); // Handle no weapon case
            return;
        }

        // Instantiate the new weapon and assign it as the active weapon
        GameObject newWeapon = Instantiate(weaponInfo.weaponPrefab, PlayerAtk.Instance.transform);

        // Optionally reset rotation
        // ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);

        // Pass the new weapon to ActiveWeapon
        PlayerAtk.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
