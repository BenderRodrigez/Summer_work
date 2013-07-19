using System;

namespace Summer_work
{
	public class Anchor : Mount
	{
		private int bolt_diametr;
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
		               float max_s)
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
			if (this.type = AnchorType.Driven) {
				this.bolt_diametr = this.d;
				this.bolt_class = this.max_cut_force/100+this;
				if(this.bolt_class<=4.8)//it's bad...
					this.bolt_class = 4.8;
				else if(this.bolt_class<=5.8)
						this.bolt_class = 5.8;
				else if(this.bolt_class <= 8.8)
						this.bolt_class = 8.8;
				else if(this.bolt_class <=9.8)
					this.bolt_class = 9.8;
				else this.bolt_class = 12.9;

			} else {
				this.bolt_class = 0;
				this.bolt_diametr = 0;
			}
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
			}
		}

		public override bool CanPassByLenght (float totalLenght)
		{
			return (this.lenght < totalLenght);
		}

		public override bool CanPassByForce (int vector, float force)
		{
			switch (vector) {
			case -1:
				return true;
			break;
			case 0:
				return force < max_cut_force;
			break;
			case 1:
				return force < max_avulsion_force;
			}
		}

		public override bool CanByMaterial (Materials what, Materials wher)
		{
			bool answer = false;
			for(int i = 0; i < this.accepted_material.Length; i++)
				if(accepted_material[i] == what)
					answer = true;
			for(int i = 0; i < this.accepted_material.Length; i++)
				if(accepted_material[i] == wher)
					answer = true;
			return answer;
		}
}

