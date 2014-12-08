using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using Gomoku;

namespace Gomoku
{
		public enum Color
		{
				White = 0,
				Black = 1,
				Empty
		}

		public class PlayerState
		{
				public List<ThreeFree> threeFrees;
				public List<Segment>	doubleThrees;
				public List<Coord>	bigWeight;

				public PlayerState ()
				{
						this.threeFrees = new List<ThreeFree> ();
						this.doubleThrees = new List<Segment> ();
						this.bigWeight = new List<Coord> ();
				}

				public void Clear ()
				{
						this.threeFrees.Clear ();
						this.doubleThrees.Clear ();
						this.bigWeight.Clear ();
				}
			
				public void Copy(PlayerState player)
				{
					foreach (ThreeFree item in this.threeFrees) {
						player.threeFrees.Add(new ThreeFree() { begin = item.begin, end = item.end, ori = item.ori });
						}
					foreach (Segment item in this.doubleThrees) {
						player.doubleThrees.Add(new Segment() { begin = item.begin, end = item.end, ori = item.ori });
					}
					foreach (Coord item in this.bigWeight) {
						player.bigWeight.Add(new Coord() { x = item.x, y = item.y });
					}
		}
	}
	
	public class PossibleCell
		{
				public int weight;
				public int pos;
				public Color color;

			/*	public char disponibility;
				public bool[] _disponibility = new bool[2]; //TODO: replace by char disponibility*/
		
				public PossibleCell (int p, int w, Gomoku.Color color)
				{
						this.pos = p;
						this.weight = w;
						this.color = color;
						//this.disponibility = (char)0;
				}
		
			/*	//TODO
				public bool IsDisponible (Color color)
				{	
					return this._disponibility [((int)color) % 2];
				}
		
				//TODO
				public void SetIsDisponible (Color color, bool state)
				{
					this._disponibility [((int)color) % 2] = state;
				}*/

				public void Copy (PossibleCell cell)
				{
						cell.weight = this.weight;
						//cell.disponibility = this.disponibility;
						cell.pos = this.pos;
						cell.color = this.color;
						//cell._disponibility = (bool[])this._disponibility.Clone ();
				}
		}
	
		public class CellsList
		{
				public List<PossibleCell> cells = new List<PossibleCell> ();
				public int[] totalWeight = new int[2] {0, 0};
				public PlayerState[] players = new PlayerState[2] { new PlayerState(), new PlayerState() };
		
				public void Copy (CellsList list)
				{
						list.totalWeight = (int[]) this.totalWeight.Clone ();
			
						foreach (PossibleCell item in this.cells) {
								PossibleCell copy = new PossibleCell (item.pos, item.weight, item.color);
								//copy.disponibility = item.disponibility;
								list.cells.Add (copy);
						}

							this.players[0].Copy(list.players[0]);
							this.players[1].Copy(list.players[1]);
				}
		 
				public void Update (int x, int y, Gomoku.Map map)
				{				
					int pos = x * Map.GetSizeMap () + y;
					Gomoku.Color color = map.GetColor (x, y);
					Gomoku.Color otherColor = (color == Gomoku.Color.White) ? Gomoku.Color.Black : Gomoku.Color.White;
					PossibleCell find = this.cells.Find (item => (item.pos == pos));
							
					if (find != null && color != Gomoku.Color.Empty) {
						this.totalWeight[(int) color] -= map.GetCell(pos).GetWeight(color);
						this.totalWeight[(int) otherColor] -= map.GetCell(pos).GetWeight(otherColor);
						this.cells.Remove(find);
					}
			    }

				public void AddCell (int pos, Gomoku.Map map)
				{
						PossibleCell find = this.cells.Find (item => (item.pos == pos));

						Gomoku.Color color = map.GetCell(pos).Color;
						Gomoku.Color otherColor = (color == Gomoku.Color.White) ? Gomoku.Color.Black : Gomoku.Color.White;
						if (find == null) {
								this.cells.Add(new PossibleCell (pos, map.GetCell(pos).GetWeight(color), color));
								this.totalWeight[(int) color] += map.GetCell(pos).GetWeight(color);
								this.totalWeight[(int) otherColor] += map.GetCell(pos).GetWeight(otherColor);
						}
						
				}
		
				public void AddCell (int x, int y, Gomoku.Map map)
				{
						this.AddCell(x * Map.GetSizeMap () + y, map);
				}
		
				public void Clear ()
				{
						this.cells.Clear ();
						this.totalWeight [(int)Gomoku.Color.Black] = 0;
						this.totalWeight [(int)Gomoku.Color.White] = 0;
						this.players [0].Clear ();
						this.players [1].Clear ();
				}

				public int RandomCell (Gomoku.Map map, Gomoku.Color color)
				{
						
						return 0;
				}
		}
	
		public class Cell
		{
				public Gomoku.Color Color;
				public char[] weight;

				public char take;
				public bool[] _take = new bool[8]; //TODO: replace by char take

				public bool takeable;

				public char block;
				public bool[] _block = new bool[2]; //TODO: replace by char block
		
				public Cell ()
				{
						this.weight = new char[2];
						this.Color = Gomoku.Color.Empty;
						this.weight [0] = (char)0;
						this.weight [1] = (char)0;
				}
		
				public void Copy (Cell cell)
				{
						cell.Color = this.Color;
						cell.weight [(int)Gomoku.Color.White] = this.weight [(int)Gomoku.Color.White];
						cell.weight [(int)Gomoku.Color.Black] = this.weight [(int)Gomoku.Color.Black];
						cell.take = this.take;
						cell.takeable = this.takeable;
						cell.block = this.block;
						cell._take = (bool[])this._take.Clone ();
						cell._block = (bool[])this._block.Clone ();
				}

				// set Weight of a color 
				public void SetWeight (int weight, Gomoku.Color color)
				{
						this.weight [(int)color] = (char) weight;
				}

				public int GetWeight (Color color)
				{
					int tmp = ((int)this.weight [(int)color]);
					return tmp;	
				}

				public void SetIsTakeable (bool state)
				{
						this.takeable = state;
				}

				public bool IsTakeable ()
				{
						return	this.takeable;
				}

				//TODO
				public void SetIsTaking (Gomoku.Orientation orientation, bool state)
				{
					this._take [((int)orientation)] = state;
				}

				//TODO
				public bool IsTaking (Gomoku.Orientation orientation)
				{
					return this._take [((int)orientation)];
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
						if (this.Color != Gomoku.Color.Empty && GetWeight(this.Color) > 0)
							this.weight [(int)this.Color] = (char)((int)this.weight [(int)this.Color] - 1);
						this.Color = Gomoku.Color.Empty;
						this.take = (char)0;
						this.takeable = false;
						this.block = (char)0;
						this._block = new bool[2];
						this._take = new bool[8];
						return true;
				}

				//TODO
				public void SetIsBlock (Color color, bool state)
				{
					this._block [((int)color)] = state;
				}

				//TODO
				public bool IsBlock (Color color)
				{
					MonoBehaviour.print (color);
					return this._block [((int)color)];
				}

		}
	
		public class Map
		{
				public bool simulation;
				static public int size;
				private Cell[] map;
				private CellsList cellsList;
				static public Gomoku.Color IACOLOR = Gomoku.Color.Black;

				public Map (int sz, bool simu = false)
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
				public void SetWeight (int x, int y, int weight, Gomoku.Color color)
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
		
				public void SetIsTaking (int x, int y, Gomoku.Orientation orientation, bool state)
				{
						this.map [x * GetSizeMap () + y].SetIsTaking (orientation, state);
				}
		
				public bool IsTaking (int x, int y, Gomoku.Orientation orientation)
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
						GeneratePossibleCells (x, y, 2);

			/*****************
				MonoBehaviour.print ("---------------------------------------------");
				MonoBehaviour.print ("----------------PossibleCells----------------");
				MonoBehaviour.print ("---------------------------------------------");
				foreach (PossibleCell possible in this.cellsList.cells) {
					MonoBehaviour.print("x = " + (possible.pos / GetSizeMap()) + " y = " + (possible.pos % GetSizeMap()));
						}
				MonoBehaviour.print ("---------------------------------------------");
				MonoBehaviour.print ("---------------------------------------------");
				MonoBehaviour.print ("---------------------------------------------");
				***************/
					return true;
				}
		
				public bool RemovePawn (int x, int y)
				{
						this.map [x * GetSizeMap () + y].RemovePawn ();
						this.cellsList.AddCell (x, y, this);
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

				public void GeneratePossibleCells (int x, int y, int radius)
				{
						foreach (KeyValuePair<Gomoku.Orientation, int[]> entry in MapComponent.ORIENTATION) {
								int tmpX = x + entry.Value [0];
								int tmpY = y + entry.Value [1];

								for (int i = 0; i < radius; ++i, tmpX += entry.Value[0], tmpY += entry.Value[1]) {
										if (tmpX >= 0 && tmpX < GetSizeMap () &&
											tmpY >= 0 && tmpY < GetSizeMap () && 
					    					GetColor (tmpX, tmpY) == Color.Empty) {
												this.cellsList.AddCell (tmpX, tmpY, this);
										}
								}
						}
					this.cellsList.Update (x, y, this);
				}

				public void UpdateBigWeight(Gomoku.Color color)
				{
					cellsList.players [(int)color].bigWeight.RemoveAll (item => (GetWeight (item.x, item.y, color) < 5));
				}

				public void SaveWeight(int X, int Y, Gomoku.Color color) 
				{
					cellsList.players [(int)color].bigWeight.Add (new Coord () { x = X, y = Y });
				}

				public List<Coord> GetBigWeight(Gomoku.Color color)
				{
					return cellsList.players [(int)color].bigWeight;
				}

		}
	
}

