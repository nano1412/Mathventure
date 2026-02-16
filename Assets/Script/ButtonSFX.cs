using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSFX : MonoBehaviour
{
    public AudioSource uiAudioSource;
    public AudioClip clickSound;

    [Header("Tags That Should Play Sound")]
    public List<string> clickableTags;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                GameObject clickedUI = GetClickedUIObject();
                
                if (clickedUI != null)
                {
                    Debug.Log(clickedUI);
                    Debug.Log(clickedUI.tag);
                    if (clickableTags.Contains(clickedUI.tag))
                    {
                        
                        PlayClickSound();
                    }
                }
            }
        }
    }

    GameObject GetClickedUIObject()
    {
        
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
            return results[0].gameObject;

        return null;
    }

    void PlayClickSound()
    {
        Debug.Log("play button sound");
        uiAudioSource.PlayOneShot(clickSound);
    }
}
