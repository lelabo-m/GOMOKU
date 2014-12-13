using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;
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
				public List<Vector> ThreeFrees;
				public List<Vector>	DoubleThrees;
				public List<Coord>	BigWeight;

				public PlayerState ()
				{
						this.ThreeFrees = new List<Vector> ();
						this.DoubleThrees = new List<Vector> ();
						this.BigWeight = new List<Coord> ();
				}

				public void Clear ()
				{
						this.ThreeFrees.Clear ();
						this.DoubleThrees.Clear ();
						this.BigWeight.Clear ();
				}
			
				public void Copy (PlayerState player)
				{
						foreach (Vector item in player.ThreeFrees) {
								this.ThreeFrees.Add (new Vector () { begin = item.begin, end = item.end, ori = item.ori });
						}
						foreach (Vector item in player.DoubleThrees) {
								this.DoubleThrees.Add (new Vector () { begin = item.begin, end = item.end, ori = item.ori });
						}
						foreach (Coord item in player.BigWeight) {
								this.BigWeight.Add (new Coord () { x = item.x, y = item.y });
						}
				}
		}
	
		public class PossibleCell
		{
				public int[] Weight = new int[2] { 0, 0 };
				public Coord coord;
				public byte Availability = 0;
		
				public PossibleCell (int x, int y, int weightWhite, int weightBlack)
				{
						this.coord = new Coord ();
						this.coord.x = x;
						this.coord.y = y;
						this.Weight [(int)Gomoku.Color.White] = weightWhite;
						this.Weight [(int)Gomoku.Color.Black] = weightBlack;
				}

				public void SetAvailability (Color color, bool state)
				{
						if (this.IsAvailable (color) != state)
								this.Availability ^= (byte)(1 << (int)color);
				}

				public bool IsAvailable (Color color)
				{	
						return Convert.ToBoolean (this.Availability & (1 << (int)color));
				}
		
				public void Copy (PossibleCell cell)
				{
						this.Weight [0] = cell.Weight [0];
						this.Weight [1] = cell.Weight [1];
						this.Availability = cell.Availability;
						this.coord.x = cell.coord.x;
						this.coord.y = cell.coord.y;
				}
		}

		public class CellsList
		{
				public List<PossibleCell> cells = new List<PossibleCell> ();
				public int[] TotalWeight = new int[2] {0, 0};
				public PlayerState[] Players = new PlayerState[2] {
						new PlayerState (),
						new PlayerState ()
				};
				private System.Random random = new System.Random ();
		
				public void Copy (CellsList list)
				{
						this.TotalWeight [(int)Gomoku.Color.White] = list.TotalWeight [(int)Gomoku.Color.White];
						this.TotalWeight [(int)Gomoku.Color.Black] = list.TotalWeight [(int)Gomoku.Color.Black];

                        this.cells.Clear();
						foreach (PossibleCell item in list.cells) {
								PossibleCell copy = new PossibleCell (item.coord.x, item.coord.y, item.Weight [(int)Gomoku.Color.White], item.Weight [(int)Gomoku.Color.Black]);
								
								this.cells.Add (copy);
						}

						this.Players [0].Copy (list.Players [0]);
						this.Players [1].Copy (list.Players [1]);
						/*	MonoBehaviour.print ("Copy");
					MonoBehaviour.print("totalWeight" + Gomoku.Color.White + " = " + this.TotalWeight[(int) Gomoku.Color.White]);
					MonoBehaviour.print("totalWeight" + Gomoku.Color.Black + " = " + this.TotalWeight[(int) Gomoku.Color.Black]);
					MonoBehaviour.print("CopytotalWeight" + Gomoku.Color.White + " = " + list.TotalWeight[(int) Gomoku.Color.White]);
					MonoBehaviour.print("CopytotalWeight" + Gomoku.Color.Black + " = " + list.TotalWeight[(int) Gomoku.Color.Black]);*/
				}
		 
				public void Update (int x, int y, Gomoku.Map map)
				{				
						Gomoku.Color color = map.GetColor (x, y);
						Gomoku.Color otherColor = (color == Gomoku.Color.White) ? Gomoku.Color.Black : Gomoku.Color.White;
						PossibleCell find = this.cells.Find (item => (item.coord.x == x && item.coord.y == y));
							
						if (find != null && color != Gomoku.Color.Empty) {
								//	MonoBehaviour.print ("RemovePossibleCells : color = " + color + " pos = " + find.Pos);
								this.TotalWeight [(int)color] -= map.GetWeight (x, y, color);
								this.TotalWeight [(int)otherColor] -= map.GetWeight (x, y, otherColor);

								if (this.TotalWeight [(int)color] < 0) {
										this.TotalWeight [(int)color] = 0;
								}
								if (this.TotalWeight [(int)otherColor] < 0) {
										this.TotalWeight [(int)otherColor] = 0;
								} 

								//	MonoBehaviour.print("totalWeight" + color + " = " + this.TotalWeight[(int) color]);
								//	MonoBehaviour.print("totalWeight" + otherColor + " = " + this.TotalWeight[(int) otherColor]);
								this.cells.Remove (find);
						}

						this.cells.RemoveAll (delegate(PossibleCell item) {
								return (map.GetColor (item.coord.x, item.coord.y) != Gomoku.Color.Empty);
						});
				}

				/*public void AddCell (int pos, Gomoku.Map map)
				{
						PossibleCell find = this.cells.Find (item => (item.Ps == pos));

						Gomoku.Color color = Gomoku.Map.IACOLOR;
						Gomoku.Color otherColor = (color == Gomoku.Color.White) ? Gomoku.Color.Black : Gomoku.Color.White;

						if (find == null) {
								this.cells.Add(new PossibleCell (pos, map.GetCell(pos).GetWeight(color), map.GetCell(pos).GetWeight(otherColor)));
								this.TotalWeight[(int) color] += map.GetCell(pos).GetWeight(color);
								this.TotalWeight[(int) otherColor] += map.GetCell(pos).GetWeight(otherColor);
							//	MonoBehaviour.print("totalWeight" + color + " = " + this.TotalWeight[(int) color]);
							//	MonoBehaviour.print("totalWeight" + otherColor + " = " + this.TotalWeight[(int) otherColor]);
						}
						
				}*/
		
				public void AddCell (int x, int y, Gomoku.Map map)
				{
						//		MonoBehaviour.print ("x = " + x + " y = " + y + " weightWhite = " + map.GetWeight(x, y, Gomoku.Color.White) + " weightBlack = " +  map.GetWeight(x, y, Gomoku.Color.Black));
					
						PossibleCell find = this.cells.Find (item => (item.coord.x == x && item.coord.y == y));
			
						Gomoku.Color color = Gomoku.Map.IACOLOR;
						Gomoku.Color otherColor = (color == Gomoku.Color.White) ? Gomoku.Color.Black : Gomoku.Color.White;
			
						if (find == null) {
								//	MonoBehaviour.print ("ADD");
								this.cells.Add (new PossibleCell (x, y, map.GetWeight (x, y, color), map.GetWeight (x, y, otherColor)));
								this.TotalWeight [(int)color] += map.GetWeight (x, y, color);
								this.TotalWeight [(int)otherColor] += map.GetWeight (x, y, otherColor);
								//	MonoBehaviour.print("totalWeight" + color + " = " + this.TotalWeight[(int) color]);
								//	MonoBehaviour.print("totalWeight" + otherColor + " = " + this.TotalWeight[(int) otherColor]);
						}
				}
		
				public void Clear ()
				{
						this.cells.Clear ();
						this.TotalWeight [(int)Gomoku.Color.Black] = 0;
						this.TotalWeight [(int)Gomoku.Color.White] = 0;
						this.Players [0].Clear ();
						this.Players [1].Clear ();
				}

		
		public Coord RandomCell (Gomoku.Map map, Gomoku.Color color)
		{
			this.cells.RemoveAll (delegate(PossibleCell item) {
				return (map.GetColor (item.coord.x, item.coord.y) != Gomoku.Color.Empty);
			});
			int randomNumber;
			Gomoku.Color otherColor = (color == Gomoku.Color.White) ? Gomoku.Color.Black : Gomoku.Color.White;

            if (this.cells.Count == 0)
                return null;
			List<PossibleCell> list;
			list.AddRange(this.cells.FindAll(delegate(PossibleCell item)
			                          {
				return (map.GetWeight(item.coord.x, item.coord.y, color) >= 4);
			}));
			list.AddRange(this.cells.FindAll(delegate(PossibleCell item)
			                          {
				return (map.GetWeight(item.coord.x, item.coord.y, color) > 0 && map.GetWeight(item.coord.x, item.coord.y, color) < 4);
			}));

			this.TotalWeight [(int)color] = 0;
			foreach (PossibleCell item in list) {
				item.Weight[(int) color] = Math.Pow(map.GetWeight(item.coord.x, item.coord.y, otherColor), 2) + map.GetWeight(item.coord.x, item.coord.y, color);
				this.TotalWeight[(int) color] += item.Weight[(int) color];
			}

			randomNumber = this.random.Next(0, randomNumber);
			int i;
			for (i = 0; i < list.Count && randomNumber; ++i, --randomNumber) {
				randomNumber -= list[i].Weight[(int) color];			
			}

			if (list.Count > i)
			{
				return list[i].coord;
			}

						
			randomNumber = this.random.Next (0, this.cells.Count);
			return this.cells[randomNumber].coord;
			
						/*for (int i = 5; i >= 0; --i) {
						list = this.cells.FindAll (delegate (PossibleCell item) {
							return (map.GetWeight (item.coord.x, item.coord.y, color) == i && map.GetColor(item.coord.x, item.coord.y) == Gomoku.Color.Empty);
						});
							
								if (list.Count > 0) {
										int randomNumber = this.random.Next (0, list.Count);
										return list [randomNumber].coord;
								}
						}*/
						//return null;
				}
		}
	
		public class Cell
		{
				public Gomoku.Color Color;
				public char[] Weight;
				public byte Take;
				public bool Takeable;
				public byte Block;
	
				public Cell ()
				{
						this.Weight = new char[2];
						this.Color = Gomoku.Color.Empty;
						this.Weight [0] = (char)0;
						this.Weight [1] = (char)0;
				}
		
				public void Copy (Cell cell)
				{
						this.Color = cell.Color;
						this.Weight [(int)Gomoku.Color.White] = cell.Weight [(int)Gomoku.Color.White];
						this.Weight [(int)Gomoku.Color.Black] = cell.Weight [(int)Gomoku.Color.Black];
						this.Take = cell.Take;
						this.Takeable = cell.Takeable;
						this.Block = cell.Block;
						this.Take = cell.Take;
				}

				// set Weight of a color 
				public void SetWeight (int weight, Gomoku.Color color)
				{
						this.Weight [(int)color] = (char)weight;
				}

				public int GetWeight (Color color)
				{
						int tmp = ((int)this.Weight [(int)color]);
						return tmp;	
				}

				public void SetIsTakeable (bool state)
				{
						this.Takeable = state;
				}

				public bool IsTakeable ()
				{
						return	this.Takeable;
				}

				public void SetIsTaking (Gomoku.Orientation orientation, bool state)
				{
						if (this.IsTaking (orientation) != state)
								this.Take ^= (byte)(1 << (int)orientation);
				}

				public bool IsTaking (Gomoku.Orientation orientation)
				{
						return Convert.ToBoolean (this.Take & (1 << (int)orientation));
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
						if (this.Color != Gomoku.Color.Empty && GetWeight (this.Color) > 0)
								this.Weight [(int)this.Color] = (char)((int)this.Weight [(int)this.Color] - 1);
						this.Color = Gomoku.Color.Empty;
						this.Take = 0;
						this.Block = 0;
						this.Takeable = false;
						return true;
				}

				public void SetBlock (Color color, bool state)
				{
						if (this.IsBlock (color) != state)
								this.Block ^= (byte)(1 << (int)color);
				}

				public bool IsBlock (Color color)
				{
						return Convert.ToBoolean (this.Block & (1 << (int)color));
				}
		}
	
		public class Map
		{
				static public int Size;
				private Cell[] map;
				private CellsList cellsList;
// ReSharper disable once InconsistentNaming
				static public Gomoku.Color IACOLOR = Gomoku.Color.Black;
				public const int MAX_SCORE = 10;
				public int[] scores;
				public int id = 0;

				public Map (int sz)
				{
						Map.Size = sz;
						this.cellsList = new CellsList ();
						this.map = new Cell[GetSizeMap () * GetSizeMap ()];
						for (int i = 0; i < GetSizeMap() * GetSizeMap(); ++i) {
								this.map [i] = new Cell ();
						}
						scores = new int[2];
						scores [(int)Gomoku.Color.White] = 0;
						scores [(int)Gomoku.Color.Black] = 0;
				}
		
				static public int GetSizeMap ()
				{
						return Map.Size;
				}
		
				public void Copy (Map map)
				{
						for (int i = 0; i < GetSizeMap() * GetSizeMap(); ++i) {
								this.GetCell (i).Copy (map.GetCell (i));
						}

						this.cellsList.Copy (map.cellsList);
						this.scores [0] = map.scores [0];
						this.scores [1] = map.scores [1];
				}

				public void AddPossibleCell (int x, int y)
				{
						this.cellsList.AddCell (x, y, this);
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
						return true;
				}

				public bool RemovePawn (int x, int y)
				{
						Gomoku.Color color = GetColor (x, y);
						this.map [x * GetSizeMap () + y].RemovePawn ();
						UpdateBigWeight (color);
						return true;
				}

				public Color GetColor (int x, int y)
				{
						return this.map [x * GetSizeMap () + y].GetColor ();
				}

				public void SetIsBlock (int x, int y, Color color, bool state)
				{
						this.map [x * GetSizeMap () + y].SetBlock (color, state);
				}

				public bool IsBlock (int x, int y, Color color)
				{
						return this.map [x * GetSizeMap () + y].IsBlock (color);
				}

				public void GeneratePossibleCells (int x, int y, int radius)
				{
						//MonoBehaviour.print ("Generate");
						//MonoBehaviour.print ("cellX = " + x + " y = " + y);
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


						if (this.id == 1) {
								MonoBehaviour.print ("---------------------------------------------");
								MonoBehaviour.print ("----------------PossibleCells----------------");
								MonoBehaviour.print ("---------------------------------------------");
								foreach (PossibleCell possible in this.cellsList.cells) {
										MonoBehaviour.print ("x = " + possible.coord.x + " y = " + possible.coord.y + " weight = " + possible.Weight [(int)Gomoku.Color.White]);
								}
								MonoBehaviour.print ("---------------------------------------------");
								MonoBehaviour.print ("---------------------------------------------");
								MonoBehaviour.print ("---------------------------------------------");
						}

				}

				public void UpdateBigWeight (Gomoku.Color color)
				{
						cellsList.Players [(int)color].BigWeight.RemoveAll (item => (GetWeight (item.x, item.y, color) < 5));
				}

				public void SaveWeight (int X, int Y, Gomoku.Color color)
				{
						cellsList.Players [(int)color].BigWeight.Add (new Coord () { x = X, y = Y });
				}

				public List<Coord> GetBigWeight (Gomoku.Color color)
				{
						return cellsList.Players [(int)color].BigWeight;
				}

				public Coord RandomCell (Gomoku.Color color)
				{
						return this.cellsList.RandomCell (this, color);
				}
		}
}
