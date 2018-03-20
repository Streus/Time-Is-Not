using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] 
[RequireComponent(typeof(LineRenderer))]
public class Laser : Interactable, IActivatable, ISavable
{
	[Tooltip("How far can the laser go? 0-Infinity.")]
	[SerializeField]
	private float _distance = Mathf.Infinity;

	[Tooltip("Layers the laser can collide with.")]
	[SerializeField]
	private LayerMask _layersToHit;

	[Tooltip("The gradient for the laser's color.")]
	[SerializeField]
	private Gradient _laserColor;

	[Tooltip("Distance between each dash in the laser line.")]
	[SerializeField]
	[Range(0,1)]
	private float _dashPadding = 0.5f;

	[Tooltip("Is the laser a death laser or a trigger?")]
	[SerializeField]
	private LaserType _type;

	[Tooltip("List of activatables to affect.")]
	[SerializeField]
	private GameObject[] _activatables;

	private float laserHeight = -0.3f;

	//where the laser is hitting
	private Vector2 currentHitPoint;

	//the line renderer for the laser
	private LineRenderer _laserLine;

	//is the object's default state inverted?
	private bool isInverted = false;

	enum LaserType {Death, Trigger};

	private Animator anim;

    private bool playSound = true;

    AudioClip laserSecurityHum;
    AudioClip laserDeathHum;

    AudioSource source;

	[SerializeField]
	private GameObject particleFolder;
	private ParticleSystem[] laserParticles;

    // Use this for initialization
    void Start()
	{
		laserParticles = GetComponentsInChildren<ParticleSystem>(); 
		if(anim == null)
			anim = gameObject.GetComponentInParent<Animator> ();
		if (!Application.isPlaying)
			return;
		_laserLine = gameObject.GetComponent<LineRenderer> ();
		_laserLine.colorGradient = _laserColor;
		isInverted = !isEnabled ();

        source = this.gameObject.GetComponent<AudioSource>();
        laserSecurityHum = AudioLibrary.inst.laserSecurityHum;
        laserDeathHum = AudioLibrary.inst.laserDeathHum;
        if(_type == LaserType.Trigger)
        {
            source.clip = laserSecurityHum;
            source.Play();
        }
        else
        {
            source.clip = laserDeathHum;
            source.Play();
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
		//control laser sprite direction
		if (anim != null)
			anim.SetFloat ("ZRotation", transform.eulerAngles.z);

		if(laserParticles == null || laserParticles.Length == 0)
			laserParticles = GetComponentsInChildren<ParticleSystem>(); 
		
		if(_laserLine == null)
			_laserLine = gameObject.GetComponent<LineRenderer> ();
		if(isEnabled()) 
		{
			rayCast ();
			if (!_laserLine.enabled)
				_laserLine.enabled = true;
			if(!particleFolder.activeInHierarchy)
				particleFolder.SetActive (true);
		}
		else
		{
			currentHitPoint = transform.position;
			if (_laserLine.enabled)
				_laserLine.enabled = false;
			if(particleFolder.activeInHierarchy)
				particleFolder.SetActive (false);
		}
        if(!GameManager.isPlayerDead())
        {
            playSound = true;
        }
	}

	//Draw lines to all linked activatables
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		for(int i = 0; i < _activatables.Length; i++)
		{
			if(_activatables[i] != null)
				Gizmos.DrawLine (transform.position, _activatables[i].transform.position);
		}

		Gizmos.DrawLine (transform.position + (Vector3.up * laserHeight), (Vector3)currentHitPoint);

	}

	/// <summary>
	/// Raycasts forward and draws the laser line.
	/// </summary>
	void rayCast()
	{
		List<Collider2D> colsToIgnore = new List<Collider2D>();
		if(gameObject.GetComponent<Collider2D>() != null)
			colsToIgnore.Add(gameObject.GetComponent<Collider2D>());
		if(transform.parent != null)
		{
			if (transform.parent.GetComponent<Collider2D> () != null)
				colsToIgnore.Add (transform.parent.GetComponent<Collider2D> ());}
		if(transform.childCount >= 1)
		{
			if (transform.GetChild (0).GetComponent<Collider2D> () != null)
				colsToIgnore.Add (transform.GetChild (0).GetComponent<Collider2D> ());
		}
		
		RaycastHit2D hit = Physics2D.Raycast (transform.position + (Vector3.up * laserHeight), transform.up, _distance, _layersToHit);

		while(colsToIgnore.Contains(hit.collider))
		{
			//ray.point + [the direction your casting] * some small offset.
			hit = Physics2D.Raycast (hit.point +  ((Vector2)transform.up * 0.1f), transform.up, _distance, _layersToHit);
		}

		_laserLine.SetPosition (0, transform.position);

		if (hit.collider == null) 
		{
			_laserLine.SetPosition (1, transform.position + (transform.up * _distance));
			if(particleFolder != null && particleFolder.activeInHierarchy)
				particleFolder.SetActive (false);
			return;
		} else
		{
			_laserLine.SetPosition (1, (Vector3)hit.point - (Vector3.up * laserHeight));
			currentHitPoint = hit.point;
			if(particleFolder != null) 
			{
				if(!particleFolder.activeInHierarchy)
					particleFolder.SetActive (true);
				particleFolder.transform.position = (Vector3)hit.point - (Vector3.up * laserHeight);
				particleFolder.transform.rotation = Quaternion.LookRotation (hit.normal);
			}
		}
		_laserLine.sortingOrder = SpriteOrderer.inst.OrderMe (transform) - 1;
			
		if (!Application.isPlaying)
			return;
		float dist = Vector3.Distance (_laserLine.GetPosition (0), _laserLine.GetPosition (1));
		_laserLine.materials [0].mainTextureScale = new Vector3 (dist, 1, 1);
		if (hit.collider.gameObject.GetComponent<Entity> () != null) 
		{
			Entity entityHit = hit.collider.gameObject.GetComponent<Entity> ();
			if (entityHit.getFaction () == Entity.Faction.player) 
			{
				trigger (entityHit);
			}
		}
	}

	public Vector2 currentHit()
	{
		return currentHitPoint;
	}

	/// <summary>
	/// Triggers Activatables.
	/// </summary>
	public override void onInteract ()
	{
		ToggleSecurityDoors ();
		disable ();
		if (_activatables == null || _activatables.Length == 0)
			return;
		foreach(GameObject activatable in _activatables)
		{
			if (activatable != null) 
			{
				if (activatable.GetComponent<IActivatable> () != null)
					activatable.GetComponent<IActivatable> ().onActivate ();
			}
		}
	}

	/// <summary>
	/// Activates the laser's effect.
	/// </summary>
	public void trigger (Entity entity)
	{
		switch(_type)
		{
		case LaserType.Trigger:
			onInteract ();
                source.Stop();
                AudioLibrary.PlayLaserSecurityCollisionSound();
			break;
		case LaserType.Death:
			entity.onDeath ();
                source.Stop();
                if (playSound)
                {
                    AudioLibrary.PlayLaserDeathCollisionSound();
                    playSound = false;
                }
                break;
		}
	}

    /// <summary>
    /// Toggles the laser's state and returns it.
    /// </summary>
    public bool onActivate()
	{
		if (isEnabled ())
			disable ();
		else
			enable ();
		return isEnabled();
	}

	/// <summary>
	/// Sets the laser's state and returns it.
	/// </summary>
	public bool onActivate(bool state)
	{
		//if inverted: 'true' state enables movement, otherwise it disables it
		if (isInverted) 
		{
			if (state)
				enable ();
			else
				disable ();
		}
		else
		{
			if (state)
				disable();
			else
				enable();
		}
		return isEnabled();
	}

	//****Savable Object Functions****

	/// <summary>
	/// Saves the data into a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public SeedBase saveData()
	{
		Seed seed = new Seed ();

		seed.isOn = isEnabled ();

		return seed;
	}

	/// <summary>
	/// Loads the data from a seed.
	/// </summary>
	/// <returns>The seed.</returns>
	public void loadData(SeedBase s)
	{
		if (s == null)
			return;

		Seed seed = (Seed)s;

		if (seed.isOn)
			enable ();
		else
			disable ();
	}

	/// <summary>
	/// The seed contains all required savable information for the object.
	/// </summary>
	public class Seed : SeedBase
	{
		//is the laser on?
		public bool isOn;
	}

	void ParticlePauseControl()
	{
		if (laserParticles == null || laserParticles.Length <= 0)
			return;
		if (GameManager.isPaused() && !laserParticles[0].isPaused)
		{
			for(int i = 0; i < laserParticles.Length; i++)
				laserParticles[i].Pause();
		}
		else if (!GameManager.isPaused() && laserParticles[0].isPaused)
		{
			for(int i = 0; i < laserParticles.Length; i++)
				laserParticles[i].Play();
		}
	}
}
