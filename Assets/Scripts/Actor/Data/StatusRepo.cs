using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Status
{
	static Status()
	{
		repo = new Dictionary<string, Status> ();

		put (new Status (
			"Nullified",
			"Can no longer use abilities",
			null,
			DecayType.communal,
			1,
			float.PositiveInfinity,
			new Disable ()));
	}
}
