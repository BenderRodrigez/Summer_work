using System;
using System.Collections.Generic;

namespace Summer_work
{
	public abstract class Mount
	{
		public float max_avulsion_force;
		public float max_cut_force;
		public bool is_selfdrill;
		public bool is_throughwall;
		public bool is_doweled = false;
		public bool show_dowel = false;
		public float d;
		public float lenght;
		public Materials[] accepted_material;
		public float max_a;
		public float max_s;
		public string img_name;

		public const float infinity = 99999;

		public abstract bool CanPassByLenght(float wallLenght, float objLenght);
		public abstract bool CanPassByForce(int vector, float force, float objLenght, Materials wher);//-1 = floor, 0 = wall, 1 = roof
		public abstract bool CanByMaterial(Materials what, Materials wher);// 0 = what; 1 = where
		public override abstract string ToString ();
		public abstract string NameToString();
		public static Mount FromString (string s)
		{
			string[] attribs = s.Split (' ');
			if (attribs [0] == "Driven" || attribs [0] == "Frame" || attribs [0] == "Sleeve" || attribs [0] == "Wedged") {
				//this is anchor
				AnchorType type;
				float max_avlution_force, max_cut_force, max_a, max_s;
				float d, lenght, bolt_d;
				bool is_trughwall;
				string[] accepted_m = attribs [6].Split (',');
				Materials[] accepted_materials = new Materials[accepted_m.Length];
				Enum.TryParse (attribs [0], out type);
				float.TryParse (attribs [1], out max_avlution_force);
				float.TryParse (attribs [2], out max_cut_force);
				bool.TryParse (attribs [3], out is_trughwall);
				float.TryParse (attribs [4], out d);
				float.TryParse (attribs [5], out lenght);
				float.TryParse (attribs [7], out max_a);
				float.TryParse (attribs [8], out max_s);
				float.TryParse (attribs [9], out bolt_d);
				for (int i = 0; i < accepted_m.Length; i++)
					Enum.TryParse (accepted_m [i], out accepted_materials [i]);
				return new Anchor (type, max_avlution_force, max_cut_force, is_trughwall, d, lenght, accepted_materials, max_a, max_s, bolt_d);
			} else if (attribs [0] == "Standart" || attribs [0] == "Butterfly" || attribs [0] == "Nail") {
				//this is dowel
				DowelType type;
				float max_avlusion_force, max_cut_force, max_a, max_s;
				float[] accepted_d;
				int d, lenght;
				bool is_troughwall, is_selfdrill;
				Materials[] accepted_materials;
				Enum.TryParse (attribs [0], out type);
				float.TryParse (attribs [1], out max_avlusion_force);
				float.TryParse (attribs [2],out  max_cut_force);
				bool.TryParse (attribs [3], out is_troughwall);
				bool.TryParse (attribs [4],out  is_selfdrill);
				int.TryParse (attribs [5], out d);
				int.TryParse (attribs [6], out lenght);
				string[] accepted_m = attribs [7].Split (',');
				accepted_materials = new Materials[accepted_m.Length];
				for (int i = 0; i < accepted_m.Length; i++)
					Enum.TryParse (accepted_m [i], out accepted_materials [i]);
				float.TryParse (attribs [8], out max_a);
				float.TryParse (attribs [9], out max_s);
				accepted_m = attribs [10].Split (';');
				accepted_d = new float[accepted_m.Length];
				for (int i = 0; i < accepted_m.Length; i++)
					float.TryParse (accepted_m [i], out accepted_d [i]);
				return new Dowel (type, max_avlusion_force, max_cut_force, is_troughwall, is_selfdrill, d, lenght, accepted_materials, max_a, max_s, accepted_d);
			} else if (attribs [0] == "Tree" || attribs [0] == "Metal" || attribs [0] == "PO" || attribs [0] == "PS" || attribs [0] == "Roof" || attribs [0] == "Uni" || attribs [0] == "Capercaillie" || attribs [0] == "Concerete") {
				//This is Screw
				ScrewType type;
				float max_avlusion_force, max_cut_force, max_a, max_s;
				float d, lenght;
				bool is_troughwall, is_selfdrill;
				Materials[] accepted_materials;
				Enum.TryParse (attribs [0], out type);
				float.TryParse (attribs [1], out max_avlusion_force);
				float.TryParse(attribs[2], out max_cut_force);
				bool.TryParse (attribs [3], out is_troughwall);
				bool.TryParse (attribs [4], out is_selfdrill);
				float.TryParse (attribs [5], out d);
				float.TryParse (attribs [6], out lenght);
				string[] accepted_m = attribs [7].Split (',');
				accepted_materials = new Materials[accepted_m.Length];
				for (int i = 0; i < accepted_m.Length; i++)
					Enum.TryParse (accepted_m [i], out accepted_materials [i]);
				float.TryParse (attribs [8], out max_a);
				float.TryParse (attribs [9], out max_s);
				return new Screw (type, max_avlusion_force, max_cut_force, is_troughwall, is_selfdrill, d, lenght, accepted_materials, max_a, max_s);
			} else {
				//we have some error oO
				throw new Exception ("Can't parse string!");
			}
		}

        public Mount()
        {
            //
        }
	}
}

