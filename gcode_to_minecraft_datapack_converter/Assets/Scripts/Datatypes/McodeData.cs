using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base file that stores Mcodelines
/// </summary>
public class McodeData
{
	public List<McodeLine> data = new List<McodeLine>();
	
	public void CalculateMotionVectors()
	{
		if (data.Count == 0)
			return;

		if (data.Count > 1)
		{
			for (int i = 0; i < data.Count - 1; i++)
			{
				Vector3 newMotion = data[i + 1].pos - data[i].pos;
				if(i == 0)
				{
					Debug.Log(newMotion);
					Debug.Log(newMotion.normalized);
					Debug.Log(newMotion.normalized * data[i].magnitude);
				}
				data[i].motion = newMotion.normalized * data[i].magnitude;
			}

			data[data.Count - 1].motion = Vector3.zero;
		}
	}
}
