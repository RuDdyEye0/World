﻿using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectTool : MonoBehaviour {
	public static Vector3 LogicToWorld(NVector3 vector)
    {
		return new Vector3(vector.X / 100f, vector.Z / 100f, vector.Y / 100f);
    }

	public static Vector3 LogicToWorld(Vector3Int vector)
    {
		return new Vector3(vector.x / 100f, vector.z / 100f, vector.y / 100f);
    }

	public static float LogicToWorld(int val)
    {
		return val / 100f;
    }

	public static int WorldToLogic(float val)
    {
		return Mathf.RoundToInt(val * 100f);
    }

	public static NVector3 WorldToLogicN(Vector3 vector)
    {
		return new NVector3()
		{
			X = Mathf.RoundToInt(vector.x * 100),
			Y = Mathf.RoundToInt(vector.z * 100),
			Z = Mathf.RoundToInt(vector.y * 100)
		};
    }

	public static Vector3Int WorldToLogic(Vector3 vector)
    {
		return new Vector3Int()
		{
			x = Mathf.RoundToInt(vector.x * 100),
			y = Mathf.RoundToInt(vector.z * 100),
			z = Mathf.RoundToInt(vector.y * 100)
		};

	}

	public static bool EntityUpdate(NEntity entity, UnityEngine.Vector3 position,Quaternion rotation, float speed)
    {
		NVector3 pos = WorldToLogicN(position);
		NVector3 dir = WorldToLogicN(rotation.eulerAngles);
		int spd = WorldToLogic(speed);
		bool update = false;
        if (!entity.Position.Equal(pos))
        {
			entity.Position = pos;
			update = true;
        }
        if (!entity.Direction.Equal(dir))
        {
			entity.Direction = dir;
			update = true;
        }
		if(entity.Speed != spd)
        {
			entity.Speed = spd;
			update = true;
        }
		return update;
    }
}
