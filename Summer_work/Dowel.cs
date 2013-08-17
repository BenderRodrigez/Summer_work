using System;
using System.Resources;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Summer_work;
using System.Text.RegularExpressions;

namespace Summer_work
{
	public class Dowel : Mount
	{
		public DowelType type;
		public float[] accepted_screw_d;
		//Screw screw;//нужен ли он здесь?
		public Dowel (DowelType type,
		              float max_avulsion_force,
		              float max_cut_force,
		              bool is_throughwall,
		              bool is_selfdrill,
		              float d,
		              float lenght,
		              Materials[] accepted_material,
		              float max_a,
		              float max_s,
		              float[] accepted_screw_d)
		{
			this.max_cut_force = max_cut_force;//To screws...
			this.type = type;
			this.max_avulsion_force = max_avulsion_force;
			this.is_throughwall = is_throughwall;
			this.is_selfdrill = is_selfdrill;
			this.d = d;
			this.lenght = lenght;
			this.accepted_material = accepted_material;
			this.max_a = max_a;
			this.max_s = max_s;
			this.accepted_screw_d = accepted_screw_d;
			switch (this.type) {
			case DowelType.Butterfly:
				this.img_name = "Summer_work.Imgs.Dowels.butterfly.png";
				break;
			case DowelType.Nail:
				this.img_name = "Summer_work.Imgs.Dowels.nail.png";
				break;
			case DowelType.Standart:
				this.img_name = "Summer_work.Imgs.Dowels.standart.png";
				break;
			}
		}

//		public void Add_Screw(ScrewType type, float d, float lenght, bool is_throughwall,
//		                      bool is_selfdrill, float max_avulsion_force, float max_cut_force)
//		{
//			this.screw = new Screw(type, max_avulsion_force, max_cut_force, is_throughwall,
//			                       is_selfdrill, d, lenght, this.accepted_material, this.max_a, this.max_s);
//		}

		public override bool CanPassByForce (int vector, float force, float objLenght, Materials wher)
		{
			return (force*(objLenght)/(this.lenght-objLenght)*(vector+1) < max_avulsion_force/2) && (force*(objLenght)/(this.lenght-objLenght) > 0);//+1 - to crrect calcs and make it compact
		}

		public override bool CanByMaterial (Materials what, Materials wher)
		{
			return Array.IndexOf(this.accepted_material, wher)>-1;
		}

		public override bool CanPassByLenght (float wallLenght, float objLenght)
		{
			if(!this.is_throughwall)
				if(this.type != DowelType.Nail)
					return this.lenght < wallLenght;
				else
					return (this.lenght - objLenght < wallLenght) && (this.lenght - objLenght > this.lenght * 0.5);
			else
				return this.lenght -objLenght > wallLenght*1.5;
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
			s += this.d.ToString ()+' ';
			s += this.lenght.ToString ()+' ';
			for (int i = 0; i < this.accepted_material.Length-1; i++)
				s += this.accepted_material [i].ToString ()+',';
			s += this.accepted_material [this.accepted_material.Length-1].ToString ()+' ';
			s += this.max_a.ToString() + ' ';
			s += this.max_s.ToString()+' ';
			s += this.accepted_screw_d [0].ToString ();
			for (int i = 1; i < this.accepted_screw_d.Length; i++)
				s += ';' + this.accepted_screw_d [i].ToString ();
			return s;
		}

		public override string NameToString ()
		{
			switch (this.type) {
			case DowelType.Butterfly:
				return "Дюбель \"бабочка\"";
			case DowelType.Nail:
				return "Дюбель-гвоздь";
			case DowelType.Standart:
				return "Дюбель";
			}
			return "error(Anchor)";
		}
	}
}

