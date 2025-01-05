using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    private static ActiveInventory _instance;
    public static ActiveInventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ActiveInventory>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(ActiveInventory).ToString());
                    _instance = singleton.AddComponent<ActiveInventory>();
                }
            }
            return _instance;
        }
    }

    private int activeSlotIndexNum = 0;
    private PlayerControls playerControls;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject); // Make sure this is the root GameObject
        }
        else
        {
            Destroy(gameObject);
        }

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
        if (PlayerAtk.Instance.CurrentPlayerAtk != null)
        {
            Destroy(PlayerAtk.Instance.CurrentPlayerAtk.gameObject);
        }

        if (!transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>())
        {
            PlayerAtk.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNum).
        GetComponentInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;

        GameObject newWeapon = Instantiate(weaponToSpawn, PlayerAtk.Instance.transform.position, Quaternion.identity);

        newWeapon.transform.parent = PlayerAtk.Instance.transform;

        PlayerAtk.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}