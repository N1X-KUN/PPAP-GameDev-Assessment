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
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangePlayerAtk();
    }

    private void ChangePlayerAtk()
    {
        // Destroy the current weapon
        if (PlayerAtk.Instance.CurrentPlayerAtk != null)
        {
            Destroy(PlayerAtk.Instance.CurrentPlayerAtk.gameObject);
        }

        // Reset attack state
        PlayerAtk.Instance.ToggleIsAttacking(false);

        // If no weapon is found in the slot, clear the player's attack
        if (!transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>())
        {
            PlayerAtk.Instance.WeaponNull();
            return;
        }

        // Spawn the new weapon
        GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNum).
            GetComponentInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;

        GameObject newWeapon = Instantiate(weaponToSpawn, PlayerAtk.Instance.transform.position, Quaternion.identity);

        PlayerAtk.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        newWeapon.transform.parent = PlayerAtk.Instance.transform;

        PlayerAtk.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}