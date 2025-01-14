using System.Collections;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;
    private PlayerControls playerControls;

    protected override void Awake()
    {
        base.Awake(); // Ensures Singleton behavior

        if (playerControls == null)
        {
            playerControls = new PlayerControls(); // Instantiate PlayerControls
            Debug.Log("playerControls initialized in Awake.");
        }
    }

    private void Start()
    {
        if (playerControls == null)
        {
            Debug.LogError("playerControls is not initialized!");
            return;
        }

        // Ensure Singleton instance is valid before proceeding
        if (Instance == null)
        {
            Debug.LogError("ActiveInventory Singleton instance is null at Start!");
            return;
        }

        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            Debug.LogError("playerControls is not initialized in OnEnable!");
            return;
        }

        playerControls.Enable();
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighlight(0);
    }

    private void ToggleActiveSlot(int numValue)
    {
        if (Instance == null)
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
        if (PlayerAtk.Instance == null)
        {
            Debug.LogError("PlayerAtk.Instance is null! Ensure PlayerAtk is initialized.");
            return;
        }

        if (PlayerAtk.Instance.CurrentPlayerAtk != null)
        {
            Destroy(PlayerAtk.Instance.CurrentPlayerAtk.gameObject);
        }

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform?.GetComponentInChildren<InventorySlot>();

        if (inventorySlot == null)
        {
            Debug.LogError($"No InventorySlot found in child {activeSlotIndexNum}!");
            PlayerAtk.Instance.WeaponNull();
            return;
        }

        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();

        if (weaponInfo == null || weaponInfo.weaponPrefab == null)
        {
            Debug.LogWarning("No weapon info or prefab found in selected inventory slot.");
            PlayerAtk.Instance.WeaponNull();
            return;
        }

        GameObject newWeapon = Instantiate(weaponInfo.weaponPrefab, PlayerAtk.Instance.transform);
        MonoBehaviour newWeaponComponent = newWeapon.GetComponent<MonoBehaviour>();

        if (newWeaponComponent == null)
        {
            Debug.LogError("The weapon prefab does not have a MonoBehaviour attached!");
            return;
        }

        PlayerAtk.Instance.NewWeapon(newWeaponComponent);
    }
}
