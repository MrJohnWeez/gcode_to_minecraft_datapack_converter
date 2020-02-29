using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base file that stores Mcodelines
/// </summary>
public class McodeData
{
	public List<McodeLine> data = new List<McodeLine>();
	
	/// <summary>
	/// Calculate the motion velocity that moves the armor stand to the next node
	/// </summary>
	public void CalculateMotionVectors()
	{
		if (data.Count > 1)
		{
			for (int i = 0; i < data.Count; i++)
			{
				// Calculate the motion that is needed to get to the current point
				Vector3 prevVector = i == 0 ? Vector3.zero : data[i-1].pos;
				Vector3 newMotion = data[i].pos - prevVector;
				data[i].motion = newMotion.normalized * data[i].magnitude;
			}
		}
	}

	/// <summary>
	/// Scales up the entire mcode dataset
	/// </summary>
	/// <param name="scaleFactor">The value to scale by</param>
	/// <returns></returns>
	public void ScalePos(float scaleFactor)
	{
		for(int i = 0; i< data.Count; i++)
		{
			data[i].pos *= scaleFactor;
		}
		CalculateMotionVectors();
	}

	/// <summary>
	/// Scales up the entire mcode dataset
	/// </summary>
	/// <param name="scaleFactor">The value to scale by</param>
	/// <returns></returns>
	public void ScaleMoveSpeed(float scaleFactor)
	{
		for (int i = 0; i < data.Count; i++)
		{
			data[i].magnitude *= scaleFactor;
		}
		CalculateMotionVectors();
	}

	/// <summary>
	/// Gets the minimum cordinate space used for print
	/// </summary>
	/// <returns>xyz vector of min space used</returns>
	public Vector3 MinCord()
	{
		float minX = 0;
		float minY = 0;
		float minZ = 0;

		if (data.Count > 0)
		{
			minX = data[0].pos.x;
			minY = data[0].pos.y;
			minZ = data[0].pos.z;

			foreach (McodeLine mcl in data)
			{
				minX = Mathf.Min(minX, mcl.pos.x);
				minY = Mathf.Min(minX, mcl.pos.y);
				minZ = Mathf.Min(minX, mcl.pos.z);
			}
		}

		return new Vector3(minX, minY, minZ);
	}

	/// <summary>
	/// Gets the maximun cordinate space used for print
	/// </summary>
	/// <returns>xyz vector of max space used</returns>
	public Vector3 MaxCord()
	{
		float maxX = 0;
		float maxY = 0;
		float maxZ = 0;

		if (data.Count > 0)
		{
			maxX = data[0].pos.x;
			maxY = data[0].pos.y;
			maxZ = data[0].pos.z;

			foreach (McodeLine mcl in data)
			{
				maxX = Mathf.Max(maxX, mcl.pos.x);
				maxY = Mathf.Max(maxX, mcl.pos.y);
				maxZ = Mathf.Max(maxX, mcl.pos.z);
			}
		}

		return new Vector3(maxX, maxY, maxZ);
	}
}
