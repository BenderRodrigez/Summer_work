using System;
using System.Resources;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Summer_work;
using System.Text.RegularExpressions;

namespace Summer_work
{
	public class Screw : Mount
	{
		public ScrewType type;

		public Screw (ScrewType type,
		              float max_avulsion_force,
		              float max_cut_force,
		              bool is_throughwall,
		              bool is_selfdrill,
		              float d,
		              float lenght,
		              Materials[] accepted_material,
		              float max_a,
		              float max_s)
		{
			this.type = type;
			this.max_avulsion_force = max_avulsion_force;
			this.max_cut_force = max_cut_force;
			this.is_throughwall = is_throughwall;
			this.is_selfdrill = is_selfdrill;
			this.d = d;
			this.lenght = lenght;
			this.accepted_material = accepted_material;
			this.max_a = max_a;
			this.max_s = max_s;
			switch (this.type) {
			case ScrewType.Capercaillie:
				this.img_name = "Summer_work.Imgs.Screws.capercaillie.png";
				break;
			case ScrewType.Metal:
				this.img_name = "Summer_work.Imgs.Screws.metal.png";
				break;
			case ScrewType.PO:
				this.img_name = "Summer_work.Imgs.Screws.PO.png";
				break;
			case ScrewType.PS:
				this.img_name = "Summer_work.Imgs.Screws.PS.png";
				break;
			case ScrewType.Roof:
				this.img_name = "Summer_work.Imgs.Screws.roofs.png";
				break;
			case ScrewType.Tree:
				this.img_name = "Summer_work.Imgs.Screws.tree.png";
				break;
			case ScrewType.Uni:
				this.img_name = "Summer_work.Imgs.Screws.uni.png";
				break;
			}
		}

		public override bool CanPassByLenght (float totalLenght)
		{
			return (this.lenght < totalLenght*0.8);
		}

		public override bool CanPassByForce (int vector, float force)
		{
			switch (vector) {
			case -1:
				return true;
			case 0:
				return force < max_cut_force;
			case 1:
				return force < max_avulsion_force;
			}

			return false;
		}

		public override bool CanByMaterial (/*Materials what, */Materials wher)
		{
			//bool answer = false;
			bool answer1 = false;
//			for(int i = 0; i < this.accepted_material.Length; i++)
//				if(accepted_material[i] == what)
//					answer |= true;
			for(int i = 0; i < this.accepted_material.Length; i++)
				if(accepted_material[i] == wher)
					answer1 |= true;
			return /*answer && */answer1;
		}

		public override string ToString ()
		{
			string s = this.type.ToString () + ' ';
			s += this.max_avulsion_force.ToString () + ' ';
			s += this.max_cut_force.ToString () + ' ';
			if (this.is_throughwall)
				s += "1 ";
			else
				s += "0 ";
			if (this.is_selfdrill)
				s += "1 ";
			else
				s += "0 ";
			s += this.d.ToString ()+ ' ';
			s += this.lenght.ToString ()+ ' ';
			for (int i = 0; i < this.accepted_material.Length-1; i++)
				s += this.accepted_material [i].ToString ()+',';
			s += this.accepted_material [this.accepted_material.Length-1].ToString ()+' ';
			s += this.max_a.ToString() + ' ';
			s += this.max_s.ToString();
			return s;
		}

		public override string NameToString ()
		{
			return this.type.ToString();
		}
	}
}

