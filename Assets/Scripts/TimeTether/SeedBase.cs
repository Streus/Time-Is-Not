using UnityEngine;
using System.Runtime.Serialization;
using System;

#pragma warning disable 0168
[Serializable]
public class SeedBase: ISerializable
{
	/* Instance Vars */

	// The destroyed state of the subject
	public bool destroyed;

	// Whether the subject will allow itself to be reset by the SSM
	public bool ignoreReset;

	// Values for filling a transform
	public Vector3 tPosition;
	public Quaternion tRotation;

	// Values for filling a Rigidbody2D
	public Vector2 rbPosition;
	public float rbRotation;
	public Vector2 rbVelocity;
	public float rbAngVelocity;

	// Path for a prefab that this seed should spawn
	public string prefabPath;

	// In the case of prefab saving, the Registered ID is saved here
	public string registeredID;

	// In the case of saving a prefab that is a child GO, the parent's rID is saved here
	public string parentID;

	/* Constructors */

	// Create a new Seed that will pull transform and rigidbody info when serialized
	public SeedBase(GameObject subject)
	{
		destroyed = false;
		tPosition = subject.transform.position;
		tRotation = subject.transform.rotation;
		Rigidbody2D rb2d = subject.GetComponent<Rigidbody2D> ();
		if(rb2d != null)
		{
			rbPosition = rb2d.position;
			rbRotation = rb2d.rotation;
			rbVelocity = rb2d.velocity;
			rbAngVelocity = rb2d.angularVelocity;
		}

		destroyed = false;
		ignoreReset = subject.GetComponent<ISavable> ().shouldIgnoreReset ();

		prefabPath = "";
		registeredID = "";
		parentID = "";
	}

	// Create a Seed from serialized data that contains a transform and rigidbody state
	public SeedBase(SerializationInfo info, StreamingContext context)
	{
		//load destroyed state
		destroyed = info.GetBoolean ("destroyed");

		//load ignoreReset state
		ignoreReset = info.GetBoolean ("ignoreReset");

		//load position
		tPosition = new Vector3 (
			info.GetSingle ("t.p.x"),
			info.GetSingle ("t.p.y"),
			info.GetSingle ("t.p.z"));

		//load rotation
		tRotation = new Quaternion (
			info.GetSingle ("t.r.x"),
			info.GetSingle ("t.r.y"),
			info.GetSingle ("t.r.z"),
			info.GetSingle ("t.r.w"));

		//load rigidbody2d data, ifex
		rbPosition = new Vector2(
			info.GetSingle("rb.p.x"),
			info.GetSingle("rb.p.y"));
		rbRotation = info.GetSingle("rb.r");
		rbVelocity = new Vector2(
			info.GetSingle("rb.v.x"),
			info.GetSingle("rb.v.y"));
		rbAngVelocity = info.GetSingle("rb.av");

		//load misc
		prefabPath = info.GetString("prefabPath");
		registeredID = info.GetString ("rID");
		parentID = info.GetString ("parentID");
	}

	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		//save destroyed state
		info.AddValue("destroyed", destroyed);

		//save ignoreReset state
		info.AddValue("ignoreReset", ignoreReset);

		//save transform values
		info.AddValue ("t.p.x", tPosition.x);
		info.AddValue ("t.p.y", tPosition.y);
		info.AddValue ("t.p.z", tPosition.z);
		info.AddValue ("t.r.x", tRotation.x);
		info.AddValue ("t.r.y", tRotation.y);
		info.AddValue ("t.r.z", tRotation.z);
		info.AddValue ("t.r.w", tRotation.w);

		info.AddValue ("rb.p.x", rbPosition.x);
		info.AddValue ("rb.p.y", rbPosition.y);
		info.AddValue ("rb.r", rbRotation);
		info.AddValue ("rb.v.x", rbVelocity.x);
		info.AddValue ("rb.v.y", rbVelocity.y);
		info.AddValue ("rb.av", rbAngVelocity);

		info.AddValue ("prefabPath", prefabPath);
		info.AddValue ("rID", registeredID);
		info.AddValue ("parentID", parentID);
	}

	public void defaultLoad(GameObject subject)
	{
		if (destroyed)
		{
			//Entity is destroyed
			MonoBehaviour.Destroy (subject);
			return;
		}

		//-SeedBase values-
		subject.transform.position = tPosition;
		subject.transform.rotation = tRotation;

		Rigidbody2D body = subject.GetComponent<Rigidbody2D> ();
		if (body != null)
		{
			body.position = rbPosition;
			body.rotation = rbRotation;
			body.velocity = rbVelocity;
			body.angularVelocity = rbAngVelocity;
		}
	}
}