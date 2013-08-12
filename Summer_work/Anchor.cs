using System;
using System.Resources;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Summer_work;
using System.Text.RegularExpressions;

namespace Summer_work
{
	public class Anchor : Mount
	{
		private float bolt_d;
		private float bolt_class;
		private AnchorType type;

		public Anchor (AnchorType type, 
		               float max_avulsion_force,
		               float max_cut_force,
		               bool is_throughwall,
		               float d,
		               float lenght,
		               Materials[] accepted_material,
		               float max_a,
		               float max_s,
		               float bolt_d)
		{
			this.type = type;
			this.max_avulsion_force = max_avulsion_force;
			this.max_cut_force = max_cut_force;
			this.is_throughwall = is_throughwall;
			this.is_selfdrill = false;
			this.d = d;
			this.lenght = lenght;
			this.accepted_material = accepted_material;
			this.max_a = max_a;
			this.max_s = max_s;
			switch (this.type) {
			case AnchorType.Driven:
				this.img_name = "Summer_work.Imgs.Anchors.drivein.png";
				break;
			case AnchorType.Frame:
				this.img_name = "Summer_work.Imgs.Anchors.frame.png";
				break;
			case AnchorType.Sleeve:
				this.img_name = "Summer_work.Imgs.Anchors.sleeve.png";
				break;
			case AnchorType.Wedged:
				this.img_name = "Summer_work.Imgs.Anchors.wedgedt2.png";
				break;
			}
			if (this.type == AnchorType.Driven) {
				this.bolt_d = bolt_d;
				this.bolt_class = this.max_cut_force/100;
				if(this.bolt_class<=4.8)//it's bad...
					this.bolt_class = (float)4.8;
				else if(this.bolt_class<=5.8)
						this.bolt_class = (float)5.8;
				else if(this.bolt_class <= 8.8)
						this.bolt_class = (float)8.8;
				else if(this.bolt_class <=9.8)
					this.bolt_class = (float)9.8;
				else this.bolt_class = (float)12.9;

			} else {
				this.bolt_class = 0;
				this.bolt_d = 0;
			}
		}

		public override bool CanPassByLenght (float wallLenght, float objLenght)
		{
			if (!this.is_throughwall) {
				return (this.lenght - objLenght < wallLenght) && (this.lenght - objLenght > this.lenght * 0.35);
			} else {
				return this.lenght -objLenght > wallLenght*1.5;
			}
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

		public override bool CanByMaterial (Materials what, Materials wher)
		{
			return (what != Materials.GKL) && (Array.IndexOf(this.accepted_material, wher)>-1);
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
			s += this.d.ToString ()+' ';
			s += this.lenght.ToString ()+' ';
			for (int i = 0; i < this.accepted_material.Length-1; i++)
				s += this.accepted_material [i].ToString ()+',';
			s += this.accepted_material [this.accepted_material.Length-1].ToString ()+' ';
			s += this.max_a.ToString() + ' ';
			s += this.max_s.ToString() + ' ';
			s += this.bolt_d.ToString ();
			return s;
		}

		public override string NameToString ()
		{
			switch (this.type) {
			case AnchorType.Driven:
				return "Забивной анкер";
			case AnchorType.Frame:
				return "Рамный анкер (дюбель)";
			case AnchorType.Sleeve:
				return "Анкерный болт с ш/г головой";
			case AnchorType.Wedged:
				return "Анкерный болт с гайкой";
			}
			return "error(Anchor)";
		}
	}
}