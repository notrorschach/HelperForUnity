using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
	
public static class Helper
{

	/// <summary>
	/// Invokes an action after waiting for a specified number of seconds
	/// </summary>
	/// <param name="waitSeconds">The number of seconds to wait before invoking</param>
	/// <param name="invoke">The action to invoke after the time has passed</param>
	///
	/// Usage:
	///  
	///	 StartCoroutine(InvokeInSeconds(0.5f, () => Destroy(gameObject))); // Destroy the gameObject in 0.5 seconds
	public static IEnumerator InvokeInSeconds(float delay, Action myAction)
	{
		yield return new WaitForSeconds(delay);
		myAction();
	}

	public static void SafeDestroy(this GameObject go)
	{
		if(go!=null)
		{
			MonoBehaviour.Destroy(go);
		}

		else
		{
			Debug.Log("The object that you're trying to destroy doesn't exist!");
		}
	}

	/// <summary>
	/// Returns the color with the desired alpha level
	/// </summary>
	/// <returns>Alpha value</returns>
	/// <param name="color">Color whose alpha is to be changed.</param>
	/// <param name="alpha">Alpha value between 0 and 1.</param>
	public static Color WithAlpha(this Color color, float alpha)
	{
		return new Color(color.r, color.g, color.b, alpha);
	}

	/// <summary>
	/// Truncates the Vector3 (z value is removed)
	/// </summary>
	/// <returns>Vector2.</returns>
	/// <param name="vec3">Vector3 to truncate.</param>
	public static Vector2 ToVec2(this Vector3 vec3)
	{
		return new Vector2(vec3.x, vec3.y);
	}


	/// <summary>
	/// Converts a Vector2 to Vector3 with the required z value
	/// </summary>
	/// <returns>Vector3.</returns>
	/// <param name="vec2">Vector2 to convert to Vector 3.</param>
	/// <param name="zValue">z	 value.</param>
	public static Vector2 ToVec3(this Vector2 vec2, float zValue)
	{
		return new Vector3(vec2.x, vec2.y, zValue);
	}

	/// <summary>
	/// Replaces the z value of the of the Vector3
	/// </summary>
	/// <returns>Vector3 whose z value is to be changed.</returns>
	/// <param name="vec3">Vector3.</param>
	/// <param name="zVal">z value.</param>
	public static Vector3 ToVec3(this Vector3 vec3, float zVal)
	{
		return new Vector3(vec3.x, vec3.y, zVal);
	}


	private static System.Random rng = new System.Random();  
	/// <summary>
	/// Shuffle the specified list.
	/// </summary>
	/// <param name="list">List.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void Shuffle<T>(this IList<T> list)  
	{  
	    int n = list.Count;  
    	while (n > 1) 
    	{  
        	n--;  
	        int k = rng.Next(n + 1);  
	        T value = list[k];  
	        list[k] = list[n];  
	        list[n] = value;  
	    }  
	}	

	/// <summary>
	/// Sorts the number list (int).
	/// </summary>
	/// <param name="list">List to sort.</param>
	/// <param name="ascending">If set to true; sort in ascending order, else sort in descending order.</param>
	public static void SortNumberList(List<int> list, bool ascending = true)
	{
		int temp;
        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
            	if(ascending)
            	{

					if (list[i] > list[j])
					{
						temp = list[i];
						list[i] = list[j];
						list[j] = temp;
					}
				}

				else
				{
					if (list[i] < list[j])
					{
						temp = list[i];
						list[i] = list[j];
						list[j] = temp;
					}
				}
            }
        }
	}

	/// <summary>
	/// Sorts the number list (float).
	/// </summary>
	/// <param name="list">List to sort.</param>
	/// <param name="ascending">If set to true; sort in ascending order, else sort in descending order.</param>
	public static void SortNumberList(List<float> list, bool ascending = true)
	{
		float temp;
        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
            	if(ascending)
            	{

					if (list[i] > list[j])
					{
						temp = list[i];
						list[i] = list[j];
						list[j] = temp;
					}
				}

				else
				{
					if (list[i] < list[j])
					{
						temp = list[i];
						list[i] = list[j];
						list[j] = temp;
					}
				}
            }
        }
	}

	//shake helper
	public enum Axis { X, Y, Z, XY, XZ, YZ, ALL }

	public static IEnumerator LinearShake(this Transform transform, float duration, float magnitude, Axis axis, bool usePause = true)
	{
		for (float time = 0; time < duration; time += usePause ? Time.deltaTime : Time.unscaledDeltaTime)
		{
			if (!usePause || Time.timeScale != 0)
			{
				Vector3 offset = new Vector3(
					axis == Axis.X || axis == Axis.XY || axis == Axis.XZ || axis == Axis.ALL ? UnityEngine.Random.Range(-magnitude, magnitude) : 0,
					axis == Axis.Y || axis == Axis.XY || axis == Axis.YZ || axis == Axis.ALL ? UnityEngine.Random.Range(-magnitude, magnitude) : 0,
					axis == Axis.Z || axis == Axis.XZ || axis == Axis.YZ || axis == Axis.ALL ? UnityEngine.Random.Range(-magnitude, magnitude) : 0);

				transform.position += offset;
				yield return new WaitForEndOfFrame();
				transform.position -= offset;
			}
		}
	}

    /// <summary>
    /// Smooth moves from current position to the target position.
    /// </summary>
    /// <returns>The move to.</returns>
    /// <param name="transform">Transform to be moved.</param>
    /// <param name="fromPos">Initial position of the transform.</param>
    /// <param name="toPos">Final position of the transform.</param>
    /// <param name="duration">Lerp duration.</param>
    /// <param name="myActions">List of actions to be executed at the end of the coroutine. Send a null if no actions are to be sent.</param>
    public static IEnumerator SmoothMove(Transform transform, Vector3 fromPos, Vector3 toPos, float duration, List<Action> myActions)
    {
        float elapsed=0;
        while(elapsed<duration && transform)
        {
            transform.position = Vector3.Lerp(fromPos, toPos, (elapsed/duration));
            elapsed+=Time.deltaTime;        
            yield return null;
        }
        transform.position = toPos;

        if(myActions!=null)
        {
			foreach(var Action in myActions)
			{
				Action();
			}
        }
		yield return null;
    } 
}

