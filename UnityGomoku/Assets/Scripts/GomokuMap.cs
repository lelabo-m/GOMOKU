using System.Collections.Generic;
using System.Collections.Specialized;

namespace Gomoku
{
		public enum Color
		{
				White = 0,
				Black,
				Empty
		}

		public class PlayerState
		{
				public List<ThreeFree> threeFrees;
				public List<Segment>	doubleThrees;
				public List<Segment>	bigWeight;

				public PlayerState ()
				{
						this.threeFrees = new List<ThreeFree> ();
						this.doubleThrees = new List<Segment> ();
						this.bigWeight = new List<Segment> ();
				}

				public void Clear ()
				{
						this.threeFrees.Clear ();
						this.doubleThrees.Clear ();
						this.bigWeight.Clear ();
				}
		}
	
		public class PossibleCell
		{
				public int weight;
				public int pos;
				public char disponibility;
		
				public PossibleCell (int p, int w)
				{
						this.pos = p;
						this.weight = w;
						this.disponibility = (char)0;
				}
		
				//TODO
				public bool IsDisponible (Color color)
				{
						return true;
				}
		
				//TODO
				public void SetIsDisponible (Color color, bool state)
				{
				}
		
				public void Copy (PossibleCell cell)
				{
						cell.weight = this.weight;
						cell.disponibility = this.disponibility;
						cell.pos = this.pos;
				}
		}
	
		public class CellsList
		{
				public List<PossibleCell> cells = new List<PossibleCell> ();
				public int totalWeight;
				public PlayerState[] players = new PlayerState[2];
		
				public void Copy (CellsList list)
				{
						list.totalWeight = this.totalWeight;
			
						foreach (PossibleCell item in this.cells) {
								PossibleCell copy = new PossibleCell (item.pos, item.weight);
								copy.disponibility = item.disponibility;
								list.cells.Add (copy);
						}
				}

				public void Update ()
				{
				}

				public void Update (int x, int y, int weight)
				{
						PossibleCell find = this.cells.Find (item => (item.pos == x * Map.GetSizeMap () + y));
						if (find != null) {
								this.cells.Remove (find);	
						} else {
								find.weight = weight;			
						}
				}
		
				public void AddCell (int pos, int weight = 0)
				{
						PossibleCell find = this.cells.Find (item => (item.pos == pos));
						if (find == null) {
								this.cells.Add (new PossibleCell (pos, weight));
						} else {
								find.weight = weight;			
						}
						//TODO updateTotalWeight ?
				}
		
				public void AddCell (int x, int y, int weight = 0)
				{
						this.cells.Add (x * Map.GetSizeMap () + y, weight);
				}
		
				public void Clear ()
				{
						this.cells.Clear ();
						this.totalWeight = 0;
						this.players [0].Clear ();
						this.players [1].Clear ();
				}
		
				//TODO
				public int RandomCell ()
				{
						return 0;
				}
		}
	
		public class Cell
		{
				public Color Color;
				public char[] weight;
				public char take;
				public bool takeable;
				public char block;
		
				public Cell ()
				{
						this.weight = new char[2];
						this.Color = Color.Empty;
						this.weight [(int)Color.White] = (char)0;
						this.weight [(int)Color.Black] = (char)0;
				}
		
				public void Copy (Cell cell)
				{
						cell.Color = this.Color;
						cell.weight [(int)Color.White] = this.weight [(int)Color.White];
						cell.weight [(int)Color.Black] = this.weight [(int)Color.Black];
						cell.take = this.take;
						cell.takeable = this.takeable;
						cell.block = this.block;
				}

				// set Weight of a color 
				public void SetWeight (int weight, Color color)
				{
						this.weight [(int)color] = (char)weight;
				}

				public int GetWeight (Color color)
				{
						return (int)this.weight [(int)color];	
				}

				public void SetIsTakeable (bool state)
				{
						this.takeable = true;
				}

				public bool IsTakeable ()
				{
						return	this.takeable;
				}

				//TODO
				public void SetIsTaking (int orientation, char state)
				{
			
				}

				//TODO
				public bool IsTaking (int orientation)
				{
						return false;
				}
		
				public void SetColor (Color type)
				{
						this.Color = type;
				}

				public Color GetColor ()
				{
						return this.Color;
				}

				public bool RemovePawn ()
				{
						if (this.Color != Color.Empty)
								this.weight [(int)this.Color] -= (char)1;
						this.Color = Color.Empty;
						this.take = (char)0;
						this.takeable = false;
						this.block = (char)0;
						return true;
				}

				//TODO
				public void SetIsBlock (Color color, bool state)
				{

				}

				//TODO
				public bool IsBlock (Color color)
				{
						return false;
				}

		}
	
		public class Map
		{
				public bool simulation;
				static public int size;
				private Cell[] map;
				public CellsList cellsList;
		
				Map (int sz, bool simu = false)
				{

						this.simulation = simu;
						Map.size = sz;
						this.cellsList = new CellsList ();
						this.map = new Cell[GetSizeMap () * GetSizeMap ()];
						for (int i = 0; i < GetSizeMap() * GetSizeMap(); ++i) {
								this.map [i] = new Cell ();
						}
				}
		
				static public int GetSizeMap ()
				{
						return Map.size;
				}
		
				public void Copy (Map map)
				{
						for (int i = 0; i < GetSizeMap() * GetSizeMap(); ++i) {
								this.GetCell (i).Copy (map.GetCell (i));
						}
						this.cellsList.Copy (map.cellsList);
				}
		
				public Cell GetCell (int x, int y)
				{
						return map [x * GetSizeMap () + y];
				}
		
				public Cell GetCell (int idx)
				{
						return map [idx];
				}

				// set Weight of a color 
				public void SetWeight (int x, int y, int weight, Color color)
				{
						this.map [x * GetSizeMap () + y].SetWeight (weight, color);
				}
		
				public void SetIsTakeable (int x, int y, bool state)
				{
						this.map [x * GetSizeMap () + y].SetIsTakeable (state);
				}
		
				public bool IsTakeable (int x, int y)
				{
						return this.map [x * GetSizeMap () + y].IsTakeable ();
				}
		
				public void SetIsTaking (int x, int y, int orientation, char state)
				{
						this.map [x * GetSizeMap () + y].SetIsTaking (orientation, state);
				}
		
				public bool IsTaking (int x, int y, int orientation)
				{
						return this.map [x * GetSizeMap () + y].IsTaking (orientation);
				}
		
				public int GetWeight (int x, int y, Color color)
				{
						return this.map [x * GetSizeMap () + y].GetWeight (color);
				}
		
				public bool PutPawn (int x, int y, Color type)
				{
						this.map [x * GetSizeMap () + y].SetColor (type);
						return true;
				}
		
				public bool RemovePawn (int x, int y)
				{
						this.map [x * GetSizeMap () + y].RemovePawn ();
						foreach (KeyValuePair<int, int[]> entry in MapComponent.ORIENTATION) {
								SetIsTaking (x, y, entry.Key, (char)0);
						}
						return true;
				}

				public Color GetColor (int x, int y)
				{
						return this.map [x * GetSizeMap () + y].GetColor ();
				}

				public void SetIsBlock (int x, int y, Color color, bool state)
				{
						this.map [x * GetSizeMap () + y].SetIsBlock (color, state);
				}

				public bool IsBlock (int x, int y, Color color)
				{
						return this.map [x * GetSizeMap () + y].IsBlock (color);
				}

				public void UpdatePossibleCells (int x, int y, int radius)
				{
						foreach (KeyValuePair<int, int[]> entry in MapComponent.ORIENTATION) {
								int tmpX = x + entry.Value [0];
								int tmpY = y + entry.Value [1];

								for (int i = 0; i < radius; ++i, tmpX += entry.Value[0], tmpY += entry.Value[1]) {
										if (tmpX >= 0 && tmpX < GetSizeMap () &&
												tmpY >= 0 && tmpY < GetSizeMap () && GetColor (tmpX, tmpY) == Color.Empty) {
												this.cellsList.AddCell (tmpX, tmpY, GetWeight (tmpX, tmpY));
										}
								}
						}

						this.cellsList.Update (x, y, etWeight (tmpX, tmpY));
				}


		}
	
}

