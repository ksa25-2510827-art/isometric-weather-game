using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    public enum WeatherState { Clear, Raining, Heavy }

    [SerializeField] private ParticleSystem rainParticles;
    [SerializeField] private Light mainLight;
    [SerializeField] private AudioSource rainAudio;
    
    private WeatherState currentWeather = WeatherState.Clear;
    private float weatherTransitionSpeed = 2f;
    private float rainIntensity = 0f;
    private Color clearSkyColor = new Color(0.5f, 0.7f, 1f);
    private Color rainySkyColor = new Color(0.3f, 0.3f, 0.4f);

    void Start()
    {
        if (mainLight == null)
            mainLight = FindObjectOfType<Light>();
    }

    void Update()
    {
        UpdateWeather();
        UpdateVisualEffects();
    }

    void UpdateWeather()
    {
        // 간단한 날씨 사이클 (데모용)
        float time = Time.time;
        float cycle = Mathf.Sin(time * 0.5f) * 0.5f + 0.5f;

        if (cycle > 0.7f)
            SetWeather(WeatherState.Heavy);
        else if (cycle > 0.4f)
            SetWeather(WeatherState.Raining);
        else
            SetWeather(WeatherState.Clear);
    }

    void UpdateVisualEffects()
    {
        // 비 파티클 강도 조절
        if (rainParticles != null)
        {
            var emission = rainParticles.emission;
            emission.rateOverTime = rainIntensity * 100f;
        }

        // 조명 조절
        if (mainLight != null)
        {
            mainLight.intensity = Mathf.Lerp(1f, 0.5f, rainIntensity);
            RenderSettings.ambientLight = Color.Lerp(clearSkyColor, rainySkyColor, rainIntensity);
        }

        // 오디오 볼륨 조절
        if (rainAudio != null)
        {
            rainAudio.volume = rainIntensity * 0.5f;
        }
    }

    void SetWeather(WeatherState newWeather)
    {
        if (currentWeather != newWeather)
        {
            currentWeather = newWeather;
            
            switch (newWeather)
            {
                case WeatherState.Clear:
                    rainIntensity = Mathf.Lerp(rainIntensity, 0f, Time.deltaTime * weatherTransitionSpeed);
                    break;
                case WeatherState.Raining:
                    rainIntensity = Mathf.Lerp(rainIntensity, 0.5f, Time.deltaTime * weatherTransitionSpeed);
                    break;
                case WeatherState.Heavy:
                    rainIntensity = Mathf.Lerp(rainIntensity, 1f, Time.deltaTime * weatherTransitionSpeed);
                    break;
            }
        }
    }

    public WeatherState GetCurrentWeather()
    {
        return currentWeather;
    }

    public float GetRainIntensity()
    {
        return rainIntensity;
    }
}
