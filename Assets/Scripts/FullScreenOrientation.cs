using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenOrientation : MonoBehaviour
{
    public Sprite spriteFullScreen; // Tam ekran modu için sprite
    public Sprite spriteNormalScreen; // Normal mod için sprite

    private bool isFullScreen = false;
    private Button fullscreenButton;
    private Image spriteRenderer;
    private Sprite originalSprite; // Orjinal sprite'ı saklamak için değişken

    private void Start()
    {
        spriteRenderer = GetComponent<Image>();
        if (spriteRenderer != null)
        {
            originalSprite = spriteRenderer.sprite; // Başlangıçta mevcut sprite'ı sakla
        }

        fullscreenButton = GetComponent<Button>();
        if (fullscreenButton != null)
        {
            fullscreenButton.onClick.AddListener(ToggleFullScreen);
        }

        // WebGL platformu için defaultOrientation ayarını yatay yap
#if UNITY_WEBGL
        Screen.orientation = UnityEngine.ScreenOrientation.LandscapeLeft;
#endif
    }

    private void Update()
    {
        // Cihazın yönelimini kontrol et
        UpdateScreenOrientation();
    }

    private void UpdateScreenOrientation()
    {
        // Cihazın yönelimine göre ekran yönelimini ayarla
        if (isFullScreen)
        {
            if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            {
                Screen.orientation = UnityEngine.ScreenOrientation.LandscapeLeft;
            }
            else if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            {
                Screen.orientation = UnityEngine.ScreenOrientation.Portrait;
            }
        }
        else
        {
            // Tam ekran modundan çıkıldığında, ekran yönelimini yatay yap
            Screen.orientation = UnityEngine.ScreenOrientation.LandscapeLeft;
        }
    }

    public void ToggleFullScreen()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;

        // Tam ekran moduna geçildiğinde ekran yönelimini güncelle
        UpdateScreenOrientation();

        // Eğer sprite atanmışsa değiştir
        if (spriteRenderer != null && spriteFullScreen != null && spriteNormalScreen != null)
        {
            spriteRenderer.sprite = isFullScreen ? spriteFullScreen : spriteNormalScreen;
        }
        else
        {
            // Eğer sprite atanmamışsa, orijinal sprite'ı geri yükle
            spriteRenderer.sprite = originalSprite;
        }
    }
}
