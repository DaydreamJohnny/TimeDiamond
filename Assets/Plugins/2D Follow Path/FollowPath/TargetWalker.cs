using System;
using UnityEngine;
using System.Collections.Generic;
public class TargetWalker : MonoBehaviour
{
	
	public float m_speed = 5.0f;
	[HideInInspector] public float td_speedCoef = 1.0f;
	
	public FollowPath m_pathController;
	
	private bool m_rotate = false;
	public bool m_running = false;
	public bool m_keepRotation = false;
	public bool m_runningOnlyWhenVisible = true;
	public bool m_usePhysics = false;
	private bool m_constantGlobalSpeed = true;
	private bool m_return = false;
//	private int m_pathSegments = 0;
	private int m_currentNode = 0;
	private bool back = false;
	
	private List<Vector3> m_list = new List<Vector3>();
	private Vector3 m_lastStartPoint;
	private Quaternion m_lastStartRotation;
	private float m_currentDistance = 1;
	private float m_time = 0;
	private int manualNextNode = 0;
	public delegate void ChangeNode(int node, Transform pointTransform);
	public ChangeNode OnChangeNode;
	private bool manualMoveNode = false;
//	private bool tmpRunning = false;
	private int actualRealNode = 0;
	private int tmpRealNode = 0;
	private float waitTime = 0;
	private float startTime = 0;

	void Start()
	{
		if(m_pathController != null)
		{
			if(!m_pathController.berzierPath)
			{
				foreach(Transform vec in m_pathController.points)
				{
					m_list.Add(vec.position);
				}
			}
			else
			{
				m_list = m_pathController.GetBerzierPoints(m_pathController.segmentsForCurve);
			}

//			m_pathSegments = m_list.Count;
			m_rotate = m_pathController.loop;
			m_return = m_pathController.pathReturn;

			m_time = 0;
			m_lastStartPoint = this.transform.position;
			m_lastStartRotation = this.transform.rotation;
			m_currentDistance = Vector3.Distance(m_lastStartPoint ,m_list[0]);
			startTime = Time.time;
		}
	}

	void FixedUpdate()
	{	
		if(m_usePhysics)
		{
			if(m_pathController != null)
			{
				m_rotate = m_pathController.loop;
				m_return = m_pathController.pathReturn;
			}

			MoveTarget();
		}
	}

	void Update()
	{
		if(!m_usePhysics)
		{
			MoveTarget();
		}
	}

	void MoveTarget()
	{
		if((this.GetComponent<Renderer>() != null && this.GetComponent<Renderer>().isVisible && m_runningOnlyWhenVisible) || !m_runningOnlyWhenVisible)
		{
			if((Time.time < (startTime + waitTime)) && waitTime != 0)
				return;
			
			if(!m_running)
				return;
			
			if(m_list == null || m_list.Count == 0)
				return;
			
			m_time = Mathf.Clamp01( Time.deltaTime * Mathf.Max(m_speed * td_speedCoef, 0) / m_currentDistance + m_time);

			if(m_time == 1)
			{
				m_time = 0;
				m_lastStartPoint = this.transform.position;
				m_lastStartRotation = this.transform.rotation;
				
				if(manualNextNode == m_currentNode && manualMoveNode)
				{
					manualMoveNode=false;
					this.Stop();
				}
				
				if(m_return && !back && m_currentNode == m_list.Count-1)
				{
					back = true;
				}
				
				if(m_return && back && m_currentNode == 0)
				{
					back = false;
				}
				
				if(m_rotate && back && m_currentNode == 0)
				{
					m_currentNode = m_list.Count;
				}
				
				if(m_pathController.berzierPath)
				{
					if((m_currentNode % m_pathController.segmentsForCurve) == 0)
					{
						actualRealNode = (m_currentNode) / m_pathController.segmentsForCurve;
					}
					
					if(m_list.Count - 1 == m_currentNode && m_pathController.loop)
					{
						actualRealNode = 0;
					}
				}
				else
				{
					actualRealNode = m_currentNode;
				}
				
				waitTime = 0;
				
				NodeBehaviour n = m_pathController.GetNodeBehaviour(actualRealNode);
				if(n != null)
				{
					this.m_speed = n.speed;
					this.waitTime = n.waitTime;
					startTime = Time.time;
				}
				
				
				if(actualRealNode != tmpRealNode)
				{
					Transform trans = m_pathController.gameObject.transform.FindChild("point"+actualRealNode);
					if(trans != null)
					{						 
						if(OnChangeNode != null)
						{	
							OnChangeNode(actualRealNode, trans);
						}
						tmpRealNode = actualRealNode;
						
						if(!m_usePhysics)
						{
							this.transform.position = trans.position;
						}
						else
						{
							this.GetComponent<Rigidbody2D>().MovePosition(new Vector2(trans.position.x, trans.position.y));
						}
					}
				}
				
				
				if(!back)
				{
					m_currentNode++;
				}
				else
				{
					m_currentNode--;
				}
				
				if(m_rotate)
				{
					m_currentNode = m_currentNode % (m_list.Count);
				}
				else
				{
					if((m_currentNode > m_list.Count-1 || m_currentNode < 0) && !m_return)
					{
						m_running = false;
						return;
					}
				}
				if(m_constantGlobalSpeed)
					m_currentDistance = Vector3.Distance(m_lastStartPoint ,m_list[m_currentNode]);
				
			}
			
			if(!m_keepRotation)
			{
				Vector3 direction = (m_list[m_currentNode] - this.transform.position).normalized;
				float angle = Vector2.Angle(direction, Vector2.up);
				if(this.transform.position.x < m_list[m_currentNode].x)
				{
					angle = -1*angle;
				}
				Quaternion qua = new Quaternion();
				qua.eulerAngles = new Vector3(0,0,angle);
				float timeRotation = m_time;
				
				if(!m_pathController.berzierPath)
				{
					timeRotation = m_time * 4;
				}
				
				this.transform.rotation = Quaternion.Lerp(m_lastStartRotation, qua, timeRotation);
				//this.transform.rotation = qua;
			}
			if(!m_usePhysics)
			{
				this.transform.position = Vector3.Lerp(m_lastStartPoint, m_list[m_currentNode],m_time);
			}
			else
			{
				this.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(new Vector2(m_lastStartPoint.x, m_lastStartPoint.y), new Vector2(m_list[m_currentNode].x, m_list[m_currentNode].y), m_time));
			}
		}
	}

	public void setNextNode(int node)
	{
		int tmpNode = node;
		if(m_pathController.berzierPath)
		{
			tmpNode = m_pathController.segmentsForCurve * node;
		}
		
		if(manualNextNode != tmpNode)
		{
			if(node > m_currentNode)
			{
				back = false;
			}

			if(node < m_currentNode)
			{
				back = true;
			}

			if(m_pathController.berzierPath)
			{
				manualNextNode = m_pathController.segmentsForCurve * node;
			}
			else
			{
				manualNextNode = node;
			}

			manualMoveNode = true;
			m_time = 1;
			this.Run();
		}
	}

	public int CurrentNode
	{
		get
		{
			return actualRealNode;
		}
	}

	public void Run()
	{
		m_running = true;
	}

	public void Stop()
	{
		m_running = false;
	}
}

