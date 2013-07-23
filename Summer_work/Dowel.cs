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
		DowelType type;
		float[] accepted_screw_d;
		Screw screw;//нужен ли он здесь?
		public Dowel (DowelType type,
		              float max_avulsion_force,
		              bool is_throughwall,
		              bool is_selfdrill,
		              float d,
		              float lenght,
		              Materials[] accepted_material,
		              float max_a,
		              float max_s,
		              float[] accepted_screw_d)
		{
			this.max_cut_force = infinity;
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

		public override bool CanPassByForce (int vector, float force)
		{
			if(this.screw != null)
				return (this.max_avulsion_force*vector < force)&&(this.screw.CanPassByForce(vector,force));
			else
				return (this.max_avulsion_force*vector < force);
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

		public override bool CanPassByLenght (float totalLenght)
		{
			return (this.lenght < totalLenght);
		}
	}
}

