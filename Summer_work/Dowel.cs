using System;

namespace Summer_work
{
	public class Dowel : Mount
	{
		DowelType type;
		Screw screw;
		public Dowel (DowelType type,
		              float max_avulsion_force,
		              bool is_throughwall,
		              bool is_selfdrill,
		              float d,
		              float lenght,
		              Materials[] accepted_material,
		              float max_a,
		              float max_s)
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

		public override bool CanPassByForce (int vector, float force)
		{
			if(this.max_avulsion_force*vector < force)

		}
	}
}

