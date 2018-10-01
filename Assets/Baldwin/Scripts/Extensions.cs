using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Baldwin
{
	/// <summary>
	///     A static class of extensions and other common functions.
	/// </summary>
	public static class Extensions
	{
		#region Other

		public static string GetApplicationFolder()
		{
			if(Application.platform == RuntimePlatform.OSXPlayer)
			{
				return Application.dataPath + "/../../";
			}

			if(Application.platform == RuntimePlatform.WindowsPlayer)
			{
				return Application.dataPath + "/../";
			}

			return Application.dataPath + "/../";
		}

		public static float ClampAngle(float angle, float min, float max)
		{
			if(angle < -360)
			{
				angle += 360;
			}

			if(angle > 360)
			{
				angle -= 360;
			}

			return Mathf.Clamp(angle, min, max);
		}

		public static Rect ScreenRectFromVectors(Vector2 posA, Vector2 posB)
		{
			//Convert origin to rect space
			posA.y = Screen.height - posA.y;
			posB.y = Screen.height - posB.y;

			//Calculate corners
			Vector2 topLeft = Vector2.Min(posA, posB);
			Vector2 bottomRight = Vector2.Max(posA, posB);

			//Create Rect
			return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
		}

		public static void DrawScreenRectOutline(Rect rect, float thickness, Color color)
		{
			//Top
			DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
			//Left
			DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
			//Right
			DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
			//Bottom
			DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
		}

		public static void DrawScreenRect(Rect rect, Color color)
		{
			GUI.color = color;
			GUI.DrawTexture(rect, Texture2D.whiteTexture);
			GUI.color = Color.white;
		}

		#endregion

		#region GameObject

		/// <summary>
		/// Parents this GameObject to another GameObject.
		/// </summary>
		/// <param name="gameObject">This GameObject.</param>
		/// <param name="parent">The GameObject to parent to.</param>
		/// <param name="worldPositionStays">Should the local position be set when parenting, to keep this GameObject in the same world position?</param>
		public static void SetParent(this GameObject gameObject, GameObject parent, bool worldPositionStays = true)
		{
			gameObject.transform.SetParent(parent.transform, worldPositionStays);
		}

		/// <summary>
		/// Parents this GameObject to a Component.
		/// </summary>
		/// <param name="gameObject">This GameObject.</param>
		/// <param name="component">The Component to parent to.</param>
		/// <param name="worldPositionStays">Should the local position be set when parenting, to keep this GameObject in the same world position?</param>
		public static void SetParent(this GameObject gameObject, Component component, bool worldPositionStays = true)
		{
			gameObject.transform.SetParent(component.transform, worldPositionStays);
		}

		/// <summary>
		///     Enables this GameObject.
		/// </summary>
		/// <param name="gameObject">This GameObject</param>
		public static void Enable(this GameObject gameObject)
		{
			gameObject.SetActive(true);
		}

		/// <summary>
		///     Disables this GameObject.
		/// </summary>
		/// <param name="gameObject">This GameObject</param>
		public static void Disable(this GameObject gameObject)
		{
			gameObject.SetActive(false);
		}

		/// <summary>
		///     Gets or Adds the component to the GameObject.
		/// </summary>
		/// <typeparam name="T">The Component type.</typeparam>
		/// <param name="gameObject">This GameObject.</param>
		/// <returns>The Component.</returns>
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			return gameObject.HasComponent<T>() ? gameObject.GetComponent<T>() : gameObject.AddComponent<T>();
		}

		/// <summary>
		///     Checks if this GameObject has a component of T.
		/// </summary>
		/// <typeparam name="T">The Component type.</typeparam>
		/// <param name="gameObject">This GameObject.</param>
		/// <returns>True if the component is present, false otherwise.</returns>
		public static bool HasComponent<T>(this GameObject gameObject) where T : Component
		{
			return gameObject.GetComponent<T>() != null;
		}

		/// <summary>
		/// Moves this Gameobject by a specified amount.
		/// </summary>
		/// <param name="gameObject">This GameObject to move.</param>
		/// <param name="amount">The amount to translate by.</param>
		/// <param name="relativeTo">What coodinate system should you translate in.</param>
		public static void Translate(this GameObject gameObject, Vector3 amount, Space relativeTo = Space.Self)
		{
			gameObject.transform.Translate(amount, relativeTo);
		}

		#endregion

		#region Component

		/// <summary>
		/// Parents this Component to another Component.
		/// </summary>
		/// <param name="component">This Component.</param>
		/// <param name="parent">The Component to parent to.</param>
		/// <param name="worldPositionStays">Should the local position be set when parenting, to keep this Component in the same world position?</param>
		public static void SetParent(this Component component, Component parent, bool worldPositionStays = true)
		{
			component.transform.SetParent(parent.transform, worldPositionStays);
		}

		/// <summary>
		/// Parents this Component to a GameObject.
		/// </summary>
		/// <param name="component">This Component.</param>
		/// <param name="parent">The GameObject to parent to.</param>
		/// <param name="worldPositionStays">Should the local position be set when parenting, to keep this Component in the same world position?</param>
		public static void SetParent(this Component component, GameObject parent, bool worldPositionStays = true)
		{
			component.transform.SetParent(parent.transform, worldPositionStays);
		}

		/// <summary>
		///     Checks if this components gameobject has a component of T.
		/// </summary>
		/// <typeparam name="T">The Component type.</typeparam>
		/// <param name="component">This component.</param>
		/// <returns>True if the component is present, false otherwise.</returns>
		public static bool HasComponent<T>(this Component component) where T : Component
		{
			return component.gameObject.GetComponent<T>() != null;
		}

		/// <summary>
		///     Gets or Adds the component to this Components GameObject.
		/// </summary>
		/// <typeparam name="T">The Component type.</typeparam>
		/// <param name="component">This Component.</param>
		/// <returns>The Component.</returns>
		public static T GetOrAddComponent<T>(this Component component) where T : Component
		{
			return component.gameObject.GetComponent<T>() != null ? component.gameObject.GetComponent<T>() : component.gameObject.AddComponent<T>();
		}

		/// <summary>
		/// Moves this component by a specified amount.
		/// </summary>
		/// <param name="component">This component to move.</param>
		/// <param name="amount">The amount to translate by.</param>
		/// <param name="relativeTo">What coodinate system should you translate in.</param>
		public static void Translate(this Component component, Vector3 amount, Space relativeTo = Space.Self)
		{
			component.transform.Translate(amount, relativeTo);
		}

		#endregion

		#region Behaviour

		/// <summary>
		///     Enables this Behaviour.
		/// </summary>
		/// <param name="behaviour">This Behaviour</param>
		public static void Enable(this Behaviour behaviour)
		{
			behaviour.enabled = true;
		}

		/// <summary>
		///     Disables this Behaviour.
		/// </summary>
		/// <param name="behaviour">This Behaviour</param>
		public static void Disable(this Behaviour behaviour)
		{
			behaviour.enabled = false;
		}

		#endregion

		#region Texture2D

		/// <summary>
		///     Loads a PNG texture file and retuns it as a Unity Texture2D.
		/// </summary>
		/// <param name="filePath">The filePath, with extension, of the PNG texture file to load.</param>
		/// <param name="mipMaps">Should the loaded texture generate MapMaps.</param>
		/// <param name="compress">Should the loaded texture be compressed into a DXT format.</param>
		/// <returns>A newly created and loaded Texture2D.</returns>
		public static Texture2D LoadPng(string filePath, bool mipMaps = true, bool compress = true)
		{
			Texture2D resultTexture = new Texture2D(16, 16, TextureFormat.RGBA32, mipMaps);
			resultTexture.LoadPNG(filePath);
			resultTexture.name = filePath;

			if(compress)
			{
				resultTexture.Compress(true);
			}

			return resultTexture;
		}

		/// <summary>
		///     Loads a PNG to a Texture2D from a file.
		/// </summary>
		/// <param name="texture">This Texture to load to.</param>
		/// <param name="filePath">The filePath, with extension, of the PNG texture file to load.</param>
		public static void LoadPNG(this Texture2D texture, string filePath)
		{
			if(File.Exists(filePath))
			{
				if(!texture.LoadImage(File.ReadAllBytes(filePath)))
				{
					Debug.LogError("Failed to load texture!");
				}
			}
		}

		#endregion

		#region BoxCollider2D

		public static Vector2 Center(this BoxCollider2D boxCollider2D)
		{
			return boxCollider2D.transform.position + new Vector3(boxCollider2D.offset.x, boxCollider2D.offset.y);
		}

		#endregion

		#region Vector Maths

		public static Vector2 LoadVector2(string data)
		{
			// Remove the parentheses
			if(data.StartsWith("(") && data.EndsWith(")"))
			{
				data = data.Substring(1, data.Length - 2);
			}

			// split the items
			string[] sArray = data.Split(',');

			return new Vector2(float.Parse(sArray[0]), float.Parse(sArray[1]));
		}

		public static Vector3 LoadVector3(string data)
		{
			// Remove the parentheses
			if(data.StartsWith("(") && data.EndsWith(")"))
			{
				data = data.Substring(1, data.Length - 2);
			}

			// split the items
			string[] sArray = data.Split(',');

			return new Vector3(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]));
		}

		public static Quaternion LoadRotationFromString(string data)
		{
			// Remove the parentheses
			if(data.StartsWith("(") && data.EndsWith(")"))
			{
				data = data.Substring(1, data.Length - 2);
			}

			// split the items
			string[] sArray = data.Split(',');

			return new Quaternion(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]), float.Parse(sArray[3]));
		}

		public static Vector2Int Vector2Int(this Vector2 vec)
		{
			return new Vector2Int(CorrectFloor(vec.x), CorrectFloor(vec.y));
		}

		public static Vector2Int Vector2Int(this Vector3 vec)
		{
			return new Vector2Int(CorrectFloor(vec.x), CorrectFloor(vec.y));
		}

		public static Vector3 Vector3(this Vector2Int vec)
		{
			return new Vector3(vec.x, vec.y);
		}

		public static bool Approx(Vector2 vec1, Vector2 vec2)
		{
			return Mathf.Approximately(vec1.x, vec2.x) && Mathf.Approximately(vec1.y, vec2.y);
		}

		#endregion

		#region Reflection

		public static List<FieldInfo> GetFields(this MonoBehaviour monoBehaviour, Type fieldType)
		{
			List<FieldInfo> allFields = monoBehaviour.GetType().GetFields().ToList();
			List<FieldInfo> matchingFields = new List<FieldInfo>();

			foreach(FieldInfo iFieldInfo in allFields)
			{
				if(iFieldInfo.FieldType.IsAssignableFrom(fieldType))
				{
					matchingFields.Add(iFieldInfo);
				}
			}

			return matchingFields;
		}

		public static List<MethodInfo> GetMethods(this MonoBehaviour monoBehaviour, Type returnType, List<Type> paramTypes = null)
		{
			List<MethodInfo> allMethods = monoBehaviour.GetType().GetMethods().ToList();
			List<MethodInfo> matchingMethods = new List<MethodInfo>();

			foreach(MethodInfo iMethodInfo in allMethods)
			{
				if(!iMethodInfo.ReturnType.IsAssignableFrom(returnType))
				{
					continue;
				}

				List<ParameterInfo> parameters = iMethodInfo.GetParameters().ToList();

				if((paramTypes == null || paramTypes.Count == 0) && (parameters == null || parameters.Count == 0))
				{
					matchingMethods.Add(iMethodInfo);
				}
				else if((paramTypes != null && paramTypes.Count > 0) && (parameters != null && parameters.Count == paramTypes.Count))
				{
					bool paramsMatch = true;

					for(int i = 0; i < parameters.Count; i++)
					{
						if(!parameters[i].GetType().IsAssignableFrom(paramTypes[i]))
						{
							paramsMatch = false;
							break;
						}
					}

					if(paramsMatch)
					{
						matchingMethods.Add(iMethodInfo);
					}
				}
			}

			return matchingMethods;
		}

		#endregion

		#region Math

		public static int CorrectFloor(float f)
		{
			bool isNegative = f < 0;
			int result = Mathf.FloorToInt(Mathf.Abs(f));
			return isNegative ? -result : result;
		}

		public static Vector2 OutsideOfUnitBox()
		{
			bool onXAxis = Random.value > 0.5;
			float x = 0.0f;
			float y = 0.0f;
			if(onXAxis)
			{
				x = Random.value;
				y = Random.value > 0.5f ? 0.0f : 1.0f;
			}
			else
			{
				x = Random.value > 0.5f ? 0.0f : 1.0f;
				y = Random.value;
			}

			return new Vector2(x, y);
		}

		public static float Sigmoid(float x)
		{
			return 1.0f / (1.0f + Mathf.Exp(-x));
		}

		#endregion
	}
}
