using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreadedJob
{
		private bool m_IsDone = false;
		private object m_Handle = new object ();
		private System.Threading.Thread m_Thread = null;

		public bool IsDone {
				get {
						bool tmp;
						lock (m_Handle) {
								tmp = m_IsDone;
						}
						return tmp;
				}
				set {
						lock (m_Handle) {
								m_IsDone = value;
						}
				}
		}
	
		public virtual void Start ()
		{
				m_Thread = new System.Threading.Thread (Run);
				m_Thread.Start ();
		}

		public virtual void Abort ()
		{
				m_Thread.Abort ();
		}
	
		protected virtual void ThreadFunction ()
		{
		}
	
		protected virtual void OnFinished ()
		{
		}
	
		public virtual bool Update ()
		{
				if (IsDone) {
						OnFinished ();
						return true;
				}
				return false;
		}

		private void Run ()
		{
				ThreadFunction ();
				IsDone = true;
		}
}


namespace Gomoku
{
		public class Counter
		{
				private object m_Handle = new object ();
				public int value;

				public Counter ()
				{
						value = 0;
				}

				public int Get ()
				{
						int val;
						lock (m_Handle) {
								val = value;
						}
						return val;
				}

				public void Inc ()
				{
						lock (m_Handle) {
								value++;
						}
				}
		}

		public class Simulation : ThreadedJob
		{
				public MCTS_IA ia;
				public Node current;
				public Map map;
				public GameManager gm;
				public Counter mycount;

				public Simulation (MCTS_IA i, Node c, Map m, GameManager g, Counter val)
				{
						ia = i;
						current = c;
						map = m;
						gm = g;
						mycount = val;
				}

				protected override void ThreadFunction ()
				{
						ia.PlayGame (current, map, gm);
						mycount.Inc ();
				}

				protected override void OnFinished ()
				{
				}
		}
}

