using UnityEngine;

// Pasang di tombol Quit: tombol otomatis disembunyikan saat game berjalan di build Web (WebGL),
// karena Application.Quit() tidak bisa menutup tab browser.
public class HideOnWebGL : MonoBehaviour
{
    void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        gameObject.SetActive(false);
#endif
    }
}
