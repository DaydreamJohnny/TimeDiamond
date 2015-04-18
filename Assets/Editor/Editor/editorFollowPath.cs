using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;

[CustomEditor(typeof(FollowPath))] 
public class editorFollowPath : Editor {

	public static Vector3 offset    = new Vector3(0, 0, -0.0f);

	Vector2 dragStart;
	bool drag = false;
	List<int>  selectedPoints = new List<int>();
	List<int>  selectedHandles = new List<int>();

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		(this.target as FollowPath).loop = GUILayout.Toggle((this.target as FollowPath).loop, "Loop");
		(this.target as FollowPath).pathReturn = GUILayout.Toggle((this.target as FollowPath).pathReturn, "Path Return");

		if (GUILayout.Button("Add"))
		{
			FollowPath path = (this.target as FollowPath);
			Vector3 pos = path.transform.position;
			
			if(path.points.Count > 0)
			{
				pos = path.points[path.points.Count - 1].position;
			}
			addPoint(pos);
		}

		EditorUtility.SetDirty((this.target as FollowPath).gameObject);
	}


	private void OnSceneGUI() {
		if (Event.current.shift) {
			AddHandlePoint();
		}

		DoHandles();
	}

	private void AddHandlePoint()
	{
		FollowPath path = (this.target as FollowPath);

		Vector2  pos = editorUtils.GetMousePos(Event.current.mousePosition, path.transform.position.z) - new Vector2(path.transform.position.x, path.transform.position.y);
		
		Handles.color = new Color(1, 1, 1, 1);
		
		Vector3 handlePos = new Vector3(pos.x, pos.y, 0) + path.transform.position + offset;
		
		if(path.points.Count > 0)
		{
			editorUtils.DrawArrow(path.points[path.points.Count-1].position, handlePos);
			
			if(path.loop)
			{
				editorUtils.DrawArrow(handlePos, path.points[0].position);
			}
		}
		
		GUIStyle     iconStyle = new GUIStyle();
		Handles.color = new Color(1, 1, 1, 0);
		if (editorUtils.IsVisible(handlePos)) {
			if (Handles.Button(handlePos, SceneView.lastActiveSceneView.camera.transform.rotation, editorUtils.HandleScale(handlePos), editorUtils.HandleScale(handlePos), Handles.CircleCap))
			{
				addPoint(handlePos);
				Selection.activeGameObject = path.gameObject;
			}
		}
	}

	private void DoHandles()
	{
		List<Transform> delete = new List<Transform>();
		FollowPath path = (this.target as FollowPath);

		if(path.points.Count == 0)
		{
			Handles.color = Color.white;
			Vector3 p = path.transform.position;
			GUIStyle s = new GUIStyle();
			Handles.Label(p, "Press SHIFT to create point, CONTROL to delete", s);
		}

		Handles.color = new Color(1, 1, 1, 0);
		GUIStyle     iconStyle = new GUIStyle();

		for (int i = 0; i < path.points.Count; i++)
		{
			Vector3 pos        = path.points[i].position;
			Vector3 posOff     =  pos + new Vector3(-0.37f, 0.37f, 0);

			if(!Event.current.control)
			{
				bool isSelected = selectedPoints.Contains(i);

				Vector3 snap   = Event.current.control ? new Vector3(EditorPrefs.GetFloat("MoveSnapX"), EditorPrefs.GetFloat("MoveSnapY"), EditorPrefs.GetFloat("MoveSnapZ")) : Vector3.zero;
				Vector3 result = Handles.FreeMoveHandle(
					pos,
					SceneView.lastActiveSceneView.camera.transform.rotation,
					editorUtils.HandleScale(posOff),
					snap, 
					Handles.CircleCap);

				if (isSelected == false) {
					selectedPoints.Clear();
					selectedPoints.Add(i);
					isSelected = true;
				}

				for (int s = 0; s < selectedPoints.Count; s++) {
					path.points[selectedPoints[s]].position = result;
				}

				if(path.berzierPath)
				{
					for(int z = 0; z <  path.points[i].childCount; z++)
					{
						bool isHandleSelected = selectedHandles.Contains(z);
						
						Vector3 snap2 = Event.current.control ? new Vector3(EditorPrefs.GetFloat("MoveSnapX"), EditorPrefs.GetFloat("MoveSnapY"), EditorPrefs.GetFloat("MoveSnapZ")) : Vector3.zero;
						
						Vector3 resultHandle = Handles.FreeMoveHandle(
							path.points[i].GetChild(z).position,
							SceneView.lastActiveSceneView.camera.transform.rotation,
							editorUtils.HandleScale(path.points[i].GetChild(z).position),
							snap2, 
							Handles.RectangleCap);
						
						if (isHandleSelected == false) {
							selectedHandles.Clear();
							selectedHandles.Add(z);
							isHandleSelected = true;
						}
						
						for (int j = 0; j < selectedHandles.Count; j++) {
							path.points[i].GetChild(selectedHandles[j]).position = resultHandle;

							if(selectedHandles[j] == 0)
							{
								path.points[i].GetChild(1).localPosition = new Vector3(path.points[i].GetChild(selectedHandles[j]).localPosition.x * -1, path.points[i].GetChild(selectedHandles[j]).localPosition.y * -1, path.points[i].GetChild(selectedHandles[j]).localPosition.z * -1);
							}
							else
							{
								path.points[i].GetChild(0).localPosition = new Vector3(path.points[i].GetChild(selectedHandles[j]).localPosition.x * -1, path.points[i].GetChild(selectedHandles[j]).localPosition.y * -1, path.points[i].GetChild(selectedHandles[j]).localPosition.z * -1);
							}
						}
					}
				}
			}
			else
			{
				Handles.color = Color.red;

				if (editorUtils.IsVisible(pos)) {
					if (Handles.Button(pos, SceneView.lastActiveSceneView.camera.transform.rotation, editorUtils.HandleScale(pos), editorUtils.HandleScale(pos), Handles.CircleCap))
					{
						delete.Add(path.points[i]);
					}
				}
			}
		}

		foreach(Transform tran in delete)
		{
			GameObject.DestroyImmediate(tran.gameObject);
			path.points.Remove(tran);
		}
	}

	// Update is called once per frame
	private void addPoint(Vector3 pos)
	{
		FollowPath path = (this.target as FollowPath);

		GameObject point = new GameObject("point"+path.points.Count.ToString());
		point.transform.parent = path.transform;

		//Selection.activeGameObject = point;

		point.transform.position = pos;
		/*point.AddComponent<DrawIcon>();
		point.GetComponent<DrawIcon>().iconName = "circle.png";*/

		GameObject handler1 = new GameObject("handlerPoint"+path.points.Count.ToString()+"1");
		GameObject handler2 = new GameObject("handlerPoint"+path.points.Count.ToString()+"2");

		handler1.transform.parent = point.transform;
		handler1.transform.position = point.transform.position + new Vector3(-1,0,0);

		/*AddComponent<DrawIcon>();
		handler1.GetComponent<DrawIcon>().iconName = "triangle.png";
		handler1.GetComponent<DrawIcon>().DrawIconFuncion = path.IsBerzier;*/

		handler2.transform.parent = point.transform;
		handler2.transform.position = point.transform.position + new Vector3(1,0,0);

		/*handler2.AddComponent<DrawIcon>();
		handler2.GetComponent<DrawIcon>().iconName = "triangle.png";
		handler2.GetComponent<DrawIcon>().DrawIconFuncion = path.IsBerzier;

		handler1.AddComponent<InversePosition>();
		handler1.GetComponent<InversePosition>().targetPosition = handler2.transform;

		handler2.AddComponent<InversePosition>();
		handler2.GetComponent<InversePosition>().targetPosition = handler1.transform;*/

		path.handlerPoints.Add(handler1.transform);
		path.handlerPoints.Add(handler2.transform);

		path.points.Add(point.transform);
	}
}
