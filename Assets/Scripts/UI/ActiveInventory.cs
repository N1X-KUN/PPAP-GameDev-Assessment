using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNum = 0;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());

        ToggleActiveHighlight(0);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void ToggleActiveSlot(int numValue)
    {
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
            Debug.LogError("PlayerAtk.Instance is null! Make sure the PlayerAtk script is initialized.");
            return;
        }

        // Destroy the current weapon
        if (PlayerAtk.Instance.CurrentPlayerAtk != null)
        {
            Destroy(PlayerAtk.Instance.CurrentPlayerAtk.gameObject);
        }

        // If no weapon is found in the slot, clear the player's attack
        InventorySlot slot = transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>();
        if (slot == null)
        {
            Debug.LogError($"No InventorySlot found in child {activeSlotIndexNum}!");
            PlayerAtk.Instance.WeaponNull();
            return;
        }

        WeaponInfo weaponInfo = slot.GetWeaponInfo();
        if (weaponInfo == null || weaponInfo.weaponPrefab == null)
        {
            PlayerAtk.Instance.WeaponNull();
            return;
        }

        // Spawn the new weapon
        GameObject newWeapon = Instantiate(weaponInfo.weaponPrefab, PlayerAtk.Instance.transform.position, Quaternion.identity);

        PlayerAtk.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        newWeapon.transform.parent = PlayerAtk.Instance.transform;

        PlayerAtk.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}