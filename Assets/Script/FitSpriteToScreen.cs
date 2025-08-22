using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FitSpriteToScreen : MonoBehaviour
{
    void Start()
    {
        FitToScreen();
    }

    void FitToScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Get the sprite bounds
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        // Get the world screen height & width from the camera
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // Calculate scale
        transform.localScale = new Vector3(
            worldScreenWidth / width,
            worldScreenHeight / height,
            1f);
    }
}
