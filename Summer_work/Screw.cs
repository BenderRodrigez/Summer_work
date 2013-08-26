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
		public Dowel dwl;

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
			case ScrewType.Concerete:
				this.img_name = "Summer_work.Imgs.Screws.concrete.png";
				break;
			}
		}

		public override bool CanPassByLenght (float wallLenght, float objLenght)
		{
			if(!this.is_doweled)
				return (this.lenght - objLenght < wallLenght) && (this.lenght - objLenght > this.lenght * 0.4);
			else
				return this.dwl.CanPassByLenght(wallLenght, objLenght) && (this.lenght - objLenght > this.lenght * 0.6);
		}

		public override bool CanPassByForce (int vector, float force, float objLenght, Materials what)
		{
			float gamma_F = 1.4f;
			float gamma_M_avulsion = 1.5f * 1.2f * 1.4f;//2.52
			float gamma_M_cut = 1f / 0.8f;//5.8*10 (5.8 - class of...)
			float gamma_F_force = 1.4f;
			float R_r_cut = ((max_cut_force/4f) / gamma_M_cut) / gamma_F;
			float R_r_avulsion = (max_avulsion_force / gamma_M_avulsion) / gamma_F;
			float force_r = force * gamma_F_force;
			float S_cont;
			if(!this.is_doweled)
				S_cont= (float)((this.lenght - objLenght) * Math.PI * this.d)/2.8f;//half of S!!!!
			else
				S_cont = (float)(this.dwl.lenght*Math.PI*this.d)/2.8f;//half of S!!!!
			float F_cont = force * 1.4f / S_cont;// N/mm^2
			switch (what) {
				case Materials.Concrete:
				if (F_cont > 20)//20 N/mm^2
					return false;
				break;
				case Materials.FoamBlock:
				if (F_cont > 3)
					return false;
				break;
				case Materials.Brick:
				if (F_cont > 12.5)
					return false;
				break;
				case Materials.Tree:
				if (F_cont > 1.8)
					return false;
				break;
				case Materials.GKL:
				if (F_cont > 2.1)
					return false;
				break;
			}
			switch (vector) {
			case -1:
				return true;
			case 0:
				if (this.is_doweled)
					return force_r <= R_r_avulsion && force_r <= R_r_cut && (this.dwl.CanPassByForce (vector, force, objLenght, what)) && (R_r_avulsion*0.2 < force_r && R_r_cut*0.2 < force_r);
				return force_r <= R_r_avulsion && force_r <= R_r_cut && (R_r_avulsion*0.2 < force_r && R_r_cut*0.2 < force_r);
			case 1:
				return force_r <= R_r_avulsion;
			}
			return false;
		}

		public override bool CanByMaterial (Materials what, Materials wher)
		{
			//if Dowel || (what && wher)
			if((Array.IndexOf(this.accepted_material, wher)>-1) && (Array.IndexOf(this.accepted_material, what)>-1))
					return true;
			else{
				if((Array.IndexOf(this.accepted_material, Materials.Dowel)>-1) && (Array.IndexOf(this.accepted_material, what)>-1)){
					Dowel dw = Calculator.DowelToScrew(this, what, wher);
					if(dw != null){
						this.dwl = dw;
					this.is_doweled = dw != null;
					}
					return this.is_doweled;
				}
			}
			return false;
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
			switch (this.type) {
			case ScrewType.Capercaillie:
				return "Глухарь";
			case ScrewType.Concerete:
				return "Саморез по бетону";
			case ScrewType.Metal:
				return "Саморез с частой резьбой";
			case ScrewType.PO:
				return "Саморез с прессшайбой острый";
			case ScrewType.PS:
				return "Саморез с перссшайбой со сверлом";
			case ScrewType.Roof:
				return "Кровельный саморез";
			case ScrewType.Tree:
				return "Саморез с крупнлой резьбой";
			case ScrewType.Uni:
				return "Универсальный шуруп";
			}
			return "error(Anchor)";
		}
	}
}

