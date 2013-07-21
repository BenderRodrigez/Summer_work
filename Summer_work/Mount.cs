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
		public float d;
		public float lenght;
		public Materials[] accepted_material;
		public float max_a;
		public float max_s;
		public string img_name;

		public const float infinity = 99999;

		public abstract bool CanPassByLenght(float totalLenght);
		//public abstract bool CanPassByBorderLenght(float s, int pcs);
		public abstract bool CanPassByForce(int vector, float force);//-1 = floor, 0 = wall, 1 = roof
		public abstract bool CanByMaterial(Materials what, Materials wher);// 0 = what; 1 = where
		public abstract void ReadDataResourses();
		public Mount ()
		{
		}
	}
}

