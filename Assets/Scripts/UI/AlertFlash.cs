using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertFlash : Singleton<AlertFlash> 
{
	[SerializeField] Image alertImage; 

	public bool disable; 

	[SerializeField] float defaultSimulationSpeed; 
	[SerializeField] float pulse_intensity; 

	[Tooltip("Perlin noise added to the effect. Set to 0 for none")]
	[SerializeField] float noise_intensity; 
	[SerializeField] float noise_scrollSpeed; 
	public float randomSeed;

	float scrollSpeed; 
	float scrollTheta; 
	bool scrollActive; 
	[Range(0, 1)] float targetAlpha; 

	void Awake()
	{
		alertImage.gameObject.SetActive(true); 
		alertImage.color = new Color (alertImage.color.r, alertImage.color.g, alertImage.color.b, 0);
		//random = Random.Range(0.0f, 65535.0f);
	}

	void Update()
	{
		if (scrollActive)
		{
			scrollTheta += Time.deltaTime * scrollSpeed; 

			if (scrollTheta >= Mathf.PI)
			{
				scrollTheta = 0; 
				scrollActive = false; 
				targetAlpha = 0; 
			}
			else
			{
				// Noise effect
				targetAlpha = 1 - Mathf.PerlinNoise(randomSeed, Mathf.Sin(scrollTheta * noise_scrollSpeed)) * noise_intensity; 

				// Pulse mutliplier
				targetAlpha *= ((Mathf.Sin(scrollTheta) / 2) + 0.5f) * (pulse_intensity);
				//Debug.Log(Mathf.PerlinNoise(random, Mathf.Sin(scrollTheta)) * noise_intensity); 
			}
		}
		else
		{
			targetAlpha = 0; 
		}

		//alertImage.color = new Color (alertImage.color.r, alertImage.color.g, alertImage.color.b, Mathf.Lerp(alertImage.color.a, targetAlpha, 10 * Time.deltaTime)); 
		alertImage.color = new Color (alertImage.color.r, alertImage.color.g, alertImage.color.b, targetAlpha);
	}

	public void PlayAlertFlash()
	{
		StartAlertFlash(defaultSimulationSpeed);
	}

	public void PlayAlertFlash(float simulationSpeed)
	{
		StartAlertFlash(simulationSpeed); 
	}

	void StartAlertFlash(float simulationSpeed)
	{
		if (scrollActive || disable)
			return; 

		scrollSpeed = simulationSpeed;

		scrollActive = true;
		scrollTheta = 0; 
	}

	/*
	ParticleSystem particles; 
	ParticleSystem.MainModule particlesMain; 
	float defaultSimulationSpeed; 

	public bool disable; 

	void Awake()
	{
		particles = GetComponent<ParticleSystem>(); 
		particlesMain = particles.main; 

		defaultSimulationSpeed = particles.main.simulationSpeed; 
	}

	public void PlayAlertFlash()
	{
		if (particles.isPlaying)
			return; 

		if (disable)
			return; 

		particlesMain.simulationSpeed = defaultSimulationSpeed; 
		particles.Play(); 
	}

	public void PlayAlertFlash(float simulationSpeed)
	{
		if (particles.isPlaying)
			return;

		if (disable)
			return; 

		particlesMain.simulationSpeed = simulationSpeed; 
		particles.Play(); 
	}
	*/
}
