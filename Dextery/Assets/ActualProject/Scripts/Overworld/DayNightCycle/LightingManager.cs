using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField]
    private Light m_directionalLight;
    [SerializeField]
    private LightingPreset m_preset = null;

    [SerializeField, Range(0, 24)]
    private float m_timeOfDay;

    private void Update()
    {
        if(m_preset == null)
        {
            return;
        }

        if(Application.isPlaying)
        {
            m_timeOfDay += Time.deltaTime;
            m_timeOfDay %= 24;
            UpdateLighting(m_timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(m_timeOfDay / 24f);
        }
    }

    private void UpdateLighting(float _timePercent)
    {
        RenderSettings.ambientLight = m_preset.AmbientColor.Evaluate(_timePercent);
        RenderSettings.fogColor = m_preset.FogColor.Evaluate(_timePercent);

        if(m_directionalLight != null)
        {
            m_directionalLight.color = m_preset.DirectionalColor.Evaluate(_timePercent);
            m_directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((_timePercent * 360f) - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        if(m_directionalLight != null)
        {
            return;
        }

        if(RenderSettings.sun != null)
        {
            m_directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if(light.type == LightType.Directional)
                {
                    m_directionalLight = light;
                    return;
                }
            }
        }
    }
}
