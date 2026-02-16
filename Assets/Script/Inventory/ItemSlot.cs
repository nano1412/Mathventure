using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;
using static Utils;

public class ItemSlot : MonoBehaviour
{
    public event Action OnItemChanged;
    [field: SerializeField] public ItemType AcceptableItemType { get; private set; }
    [field: SerializeField] public SlotType TypeOfSlot { get; private set; }

    [field: SerializeField] public GameObject ItemInThisSlot { get; private set; }
    [field: SerializeField] public GameObject MiniItemDetail { get; private set; }
    [field: SerializeField] public TMP_Text ItemName { get; private set; }
    [field: SerializeField] public TMP_Text ItemShortDescription { get; private set; }
    [field: SerializeField] public Button UseConsumableBtn { get; private set; }

    private void Start()
    {
        BuffController.current.OnSelectedConsumableUpdate += OnSelectedConsumableUpdate;
        BuffController.current.OnSelectedCharacterUpdate += OnSelectedCharacterUpdate;
        OnTransformChildrenChanged();
    }

    private void OnDestroy()
    {
        BuffController.current.OnSelectedConsumableUpdate -= OnSelectedConsumableUpdate;
        BuffController.current.OnSelectedCharacterUpdate -= OnSelectedCharacterUpdate;
    }


    void OnSelectedConsumableUpdate(GameObject itemGO)
    {
        if (itemGO != null&& ItemInThisSlot == itemGO)
        {

            MiniItemDetail.SetActive(true);
            transform.GetComponentInChildren<ItemData>().gameObject.GetComponent<Outline>().enabled = true;
        } else if(ItemInThisSlot != null)
        {
            MiniItemDetail.SetActive(false);
            ItemInThisSlot.GetComponent<Outline>().enabled = false;
        }     
    }

    void OnSelectedCharacterUpdate(GameObject characterGO)
    {
        if(characterGO != null)
        {
            UseConsumableBtn.interactable = true;
        } else
        {
            UseConsumableBtn.interactable = false;
        }
    }

    public void UseConsumableProxy()
    {
        if (BuffController.current.UseConsumable())
        {
            ItemData itemData = ItemInThisSlot.GetComponent<ItemData>();
            AudioSource sfx = itemData.UseSFX;

            sfx.Play();
            StartCoroutine(DestroyWhenFinishedPlayUseConsumable(sfx, itemData.gameObject));
        }
        
    }

    IEnumerator DestroyWhenFinishedPlayUseConsumable(AudioSource audioSource, GameObject itemObject)
    {
        // Wait until it starts playing (optional safety)
        yield return new WaitUntil(() => audioSource.isPlaying);

        // Wait until it finishes
        yield return new WaitWhile(() => audioSource.isPlaying);

        Destroy(itemObject);
        BuffController.current.SelectedConsumable = null;
    }

    private void OnTransformChildrenChanged()
    {
        if(transform.GetComponentInChildren<ItemData>() != null)
        {
            ItemName.text = transform.GetComponentInChildren<ItemData>().ItemName;
            ItemShortDescription.text = transform.GetComponentInChildren<ItemData>().ShortDescription;
            ItemInThisSlot = transform.GetComponentInChildren<ItemData>().gameObject;
        } else
        {
            ItemName.text = "";
            ItemShortDescription.text = "";
            ItemInThisSlot =null;
        }

        OnItemChanged?.Invoke();
    }
 }
