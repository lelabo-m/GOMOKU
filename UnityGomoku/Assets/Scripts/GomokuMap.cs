using System;
using UnityEngine;
using System.Collections.Generic;
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
		public List<ThreeFree> ThreeFrees;
		public List<Segment>	DoubleThrees;
		public List<Coord>	BigWeight;

		public PlayerState()
		{
			this.ThreeFrees = new List<ThreeFree>();
			this.DoubleThrees = new List<Segment>();
			this.BigWeight = new List<Coord>();
		}

		public void Clear()
		{
			this.ThreeFrees.Clear();
			this.DoubleThrees.Clear();
			this.BigWeight.Clear();
		}
			
		public void Copy(PlayerState player)
		{
			foreach (ThreeFree item in this.ThreeFrees) {
				player.ThreeFrees.Add(new ThreeFree() { begin = item.begin, end = item.end, ori = item.ori });
				}
			foreach (Segment item in this.DoubleThrees) {
				player.DoubleThrees.Add(new Segment() { begin = item.begin, end = item.end, ori = item.ori });
			}
			foreach (Coord item in this.BigWeight) {
				player.BigWeight.Add(new Coord() { x = item.x, y = item.y });
			}
        }
    }
	
    public class PossibleCell
	{
		public int Weight;
		public int Pos;

        public byte Availability = 0;
		//public bool[] _disponibility = new bool[2]; //TODO: replace by Availability
		
		public PossibleCell(int p, int w)
		{
			this.Pos = p;
			this.Weight = w;
		}

        //TODO
        public void SetAvailability(Color color, bool state)
        {
            if (this.IsAvailable(color) != state)
                this.Availability ^= (byte)(1 << (int)color);
        }

        //TODO
		public bool IsAvailable(Color color)
		{	
            return Convert.ToBoolean(this.Availability & (1 << (int)color));
		}
		
		public void Copy(PossibleCell cell)
		{
			cell.Weight = this.Weight;
		    cell.Availability = this.Availability;
			cell.Pos = this.Pos;
		}
	}
	
	public class CellsList
	{
		public List<PossibleCell> Cells = new List<PossibleCell>();
		public int TotalWeight;
        public PlayerState[] Players = new PlayerState[2] { new PlayerState(), new PlayerState() };
		
		public void Copy(CellsList list)
		{
			list.TotalWeight = this.TotalWeight;
			
			foreach (PossibleCell item in this.Cells) {
					PossibleCell copy = new PossibleCell (item.Pos, item.Weight);
					//copy.disponibility = item.disponibility;
					list.Cells.Add(copy);
			}

				this.Players[0].Copy(list.Players[0]);
				this.Players[1].Copy(list.Players[1]);
		}

		public void Update(Gomoku.Map map)
		{				
			this.Cells.RemoveAll(item => ((map.IsBlock (item.Pos / Gomoku.Map.GetSizeMap(), item.Pos % Gomoku.Map.GetSizeMap(), 
			                                    map.GetColor(item.Pos / Gomoku.Map.GetSizeMap(), item.Pos % Gomoku.Map.GetSizeMap()))) ||
				                    map.GetColor (item.Pos / Gomoku.Map.GetSizeMap(), item.Pos % Gomoku.Map.GetSizeMap()) == Gomoku.Color.Empty));
		}

		public void Update(int x, int y, int weight)
		{						
			this.Cells.RemoveAll(item => (item.Pos == x * Map.GetSizeMap() + y));
		}
		
		public void AddCell(int pos, int weight)
		{
			PossibleCell find = this.Cells.Find (item => (item.Pos == pos));
			if (find == null) {
					this.Cells.Add(new PossibleCell(pos, weight));
			} else {
					find.Weight = weight;			
			}
			//TODO updateTotalWeight ?
		}
		
		public void AddCell(int x, int y, int weight)
		{
			this.AddCell(x * Map.GetSizeMap() + y, weight);
		}
		
		public void Clear()
		{
			this.Cells.Clear();
			this.TotalWeight = 0;
			this.Players[0].Clear();
			this.Players[1].Clear();
		}
		
		//TODO
		public int RandomCell()
		{
			return 0;
		}
	}
	
	public class Cell
	{
		public Gomoku.Color Color;
		public char[] Weight;

		public byte Take;
		//public bool[] _take = new bool[8]; //TODO: replace by char take

		public bool Takeable;

		public byte Block;
		//public bool[] _block = new bool[2]; //TODO: replace by char block
		
		public Cell()
		{
			this.Weight = new char[2];
			this.Color = Gomoku.Color.Empty;
			this.Weight[0] = (char)0;
			this.Weight[1] = (char)0;
		}
		
		public void Copy(Cell cell)
		{
			cell.Color = this.Color;
			cell.Weight[(int)Gomoku.Color.White] = this.Weight[(int)Gomoku.Color.White];
			cell.Weight[(int)Gomoku.Color.Black] = this.Weight[(int)Gomoku.Color.Black];
			cell.Take = this.Take;
			cell.Takeable = this.Takeable;
			cell.Block = this.Block;
		    cell.Take = this.Take;
		}

		// set Weight of a color 
		public void SetWeight(int weight, Gomoku.Color color)
		{
			this.Weight[(int)color] = (char)weight;
		}

		public int GetWeight(Color color)
		{
			int tmp = ((int)this.Weight[(int)color]);
			return tmp;	
		}

		public void SetIsTakeable(bool state)
		{
			this.Takeable = state;
		}

		public bool IsTakeable()
		{
			return	this.Takeable;
		}

		//TODO
		public void SetIsTaking(Gomoku.Orientation orientation, bool state)
		{
		    if (this.IsTaking(orientation) != state)
		        this.Take ^= (byte)(1 << (int)orientation);
		}

		//TODO
		public bool IsTaking(Gomoku.Orientation orientation)
		{
		    return Convert.ToBoolean(this.Take & (1 << (int)orientation));
		}
		
		public void SetColor(Color type)
		{
			this.Color = type;
		}

		public Color GetColor()
		{
			return this.Color;
		}

		public bool RemovePawn()
		{
			if (this.Color != Gomoku.Color.Empty && GetWeight(this.Color) > 0)
				this.Weight[(int)this.Color] = (char)((int)this.Weight[(int)this.Color] - 1);
			this.Color = Gomoku.Color.Empty;
			this.Take = 0;
            this.Block = 0;
            this.Takeable = false;
			return true;
		}

		//TODO
		public void SetBlock(Color color, bool state)
		{
            if (this.IsBlock(color) != state)
                this.Block ^= (byte)(1 << (int)color);
        }

		//TODO
		public bool IsBlock(Color color)
		{
			MonoBehaviour.print(color);
            return Convert.ToBoolean(this.Block & (1 << (int)color));
		}
	}
	
	public class Map
	{
		public bool Simulation;
		static public int Size;
		private Cell[] map;
		private CellsList cellsList;
// ReSharper disable once InconsistentNaming
		static public Gomoku.Color IACOLOR = Gomoku.Color.Black;

		public Map(int sz, bool simu = false)
		{
			this.Simulation = simu;
			Map.Size = sz;
			this.cellsList = new CellsList();
			this.map = new Cell[GetSizeMap() * GetSizeMap()];
			for (int i = 0; i < GetSizeMap() * GetSizeMap(); ++i) {
					this.map[i] = new Cell();
			}
		}
		
		static public int GetSizeMap()
		{
			return Map.Size;
		}
		
		public void Copy(Map map)
		{
			for (int i = 0; i < GetSizeMap() * GetSizeMap(); ++i) {
					this.GetCell (i).Copy(map.GetCell (i));
			}
			this.cellsList.Copy(map.cellsList);
		}
		
		public Cell GetCell(int x, int y)
		{
			return map[x * GetSizeMap() + y];
		}
		
		public Cell GetCell(int idx)
		{
			return map[idx];
		}

		// set Weight of a color 
		public void SetWeight(int x, int y, int weight, Gomoku.Color color)
		{
			this.map[x * GetSizeMap() + y].SetWeight(weight, color);
		}
		
		public void SetIsTakeable(int x, int y, bool state)
		{
			this.map[x * GetSizeMap() + y].SetIsTakeable(state);
		}
		
		public bool IsTakeable(int x, int y)
		{
			return this.map[x * GetSizeMap() + y].IsTakeable();
		}
		
		public void SetIsTaking(int x, int y, Gomoku.Orientation orientation, bool state)
		{
			this.map[x * GetSizeMap() + y].SetIsTaking(orientation, state);
		}
		
		public bool IsTaking(int x, int y, Gomoku.Orientation orientation)
		{
			return this.map[x * GetSizeMap() + y].IsTaking(orientation);
		}
		
		public int GetWeight(int x, int y, Color color)
		{
			return this.map[x * GetSizeMap() + y].GetWeight(color);
		}
		
		public bool PutPawn(int x, int y, Color type)
		{
			this.map[x * GetSizeMap() + y].SetColor(type);
			GeneratePossibleCells(x, y, 2);

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
		
		public bool RemovePawn(int x, int y)
		{
			this.map[x * GetSizeMap() + y].RemovePawn();
			return true;
		}

		public Color GetColor(int x, int y)
		{
			return this.map[x * GetSizeMap() + y].GetColor();
		}

		public void SetIsBlock(int x, int y, Color color, bool state)
		{
			this.map[x * GetSizeMap() + y].SetBlock(color, state);
		}

		public bool IsBlock(int x, int y, Color color)
		{
			return this.map[x * GetSizeMap() + y].IsBlock(color);
		}

		public void GeneratePossibleCells(int x, int y, int radius)
		{
				foreach (KeyValuePair<Gomoku.Orientation, int[]> entry in MapComponent.ORIENTATION) {
					int tmpX = x + entry.Value[0];
					int tmpY = y + entry.Value[1];

					for (int i = 0; i < radius; ++i, tmpX += entry.Value[0], tmpY += entry.Value[1]) {
						if (tmpX >= 0 && tmpX < GetSizeMap() &&
							tmpY >= 0 && tmpY < GetSizeMap() && 
					    	GetColor(tmpX, tmpY) == Color.Empty &&
				   			!IsBlock(tmpX, tmpY, IACOLOR)) {
								this.cellsList.AddCell(tmpX, tmpY, GetWeight(tmpX, tmpY, IACOLOR));
						}
					}
				}
			this.cellsList.Update(x, y, GetWeight(x, y, IACOLOR));
		}

		public void UpdateBigWeight(Gomoku.Color color)
		{
			cellsList.Players[(int)color].BigWeight.RemoveAll(item => (GetWeight(item.x, item.y, color) < 5));
		}

		public void SaveWeight(int X, int Y, Gomoku.Color color) 
		{
			cellsList.Players[(int)color].BigWeight.Add(new Coord() { x = X, y = Y });
		}

		public List<Coord> GetBigWeight(Gomoku.Color color)
		{
			return cellsList.Players[(int)color].BigWeight;
		}
	}
}