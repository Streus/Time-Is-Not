using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDashEffect : MonoBehaviour 
{
	[Tooltip("How long each image will last")]
	public float _spriteDecayTime;
	[Tooltip("The tint color and starting alpha for the images")]
	public Color _spriteTintColor;
	[Tooltip("How often will the images be created?")]
	public float _spawnDelay;

	private SpriteRenderer _spriteSource;
	private Player _player;
	//list of active images
	private List<GameObject> _goList = new List<GameObject>();
	//list of image lifetimes
	private List<float> _timeList = new List<float>();
	//timer for spawning images
	private float _timer = 0;

	// Use this for initialization
	void Start () 
	{
		_player = gameObject.GetComponent<Player> ();
		_spriteSource = transform.GetChild (0).GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{

		IncrimentTimers ();
		if (_player.dashing ()) 
		{
			_timer -= Time.deltaTime;
			if(_timer <=0)
			{
				_timer = _spawnDelay;
				SpawnSprite ((Vector2)transform.position);
			}
		} 
		else
			_timer = 0.05f;
		
	}



	//spawns a new image
	void SpawnSprite(Vector2 location)
	{
		GameObject obj = new GameObject ();
		obj.AddComponent<SpriteRenderer> ();
		obj.GetComponent<SpriteRenderer> ().sprite = _spriteSource.sprite;
		obj.GetComponent<SpriteRenderer> ().color = _spriteTintColor;
		obj.GetComponent<SpriteRenderer> ().sortingLayerID = _spriteSource.sortingLayerID;
		obj.GetComponent<SpriteRenderer> ().sortingOrder = _spriteSource.sortingOrder-1;
		obj.transform.position = location;
		_goList.Add (obj);
		_timeList.Add (_spriteDecayTime);

	}

	//removes the given sprite from the lists and the scene
	void DestroySprite(int index)
	{
		if(index < _goList.Count)
		{
			GameObject obj = _goList [index];
			_goList.RemoveAt (index);
			_timeList.RemoveAt (index);
			Destroy (obj);
		}
	}

	//update all active image's lifetimes, and change their opacity to match, destroy them if necesary
	void IncrimentTimers()
	{
		for(int i = 0; i < _timeList.Count; i++)
		{
			_timeList [i] -= Time.deltaTime;
			_goList [i].GetComponent<SpriteRenderer> ().color = new Color (_spriteTintColor.r, _spriteTintColor.g, _spriteTintColor.b, (_spriteTintColor.a * (_timeList [i] / _spriteDecayTime)));
			if(_timeList[i] <= 0)
			{
				DestroySprite (i);
			}
		}
	}
}
