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
		Screw screw;//нужен ли он здесь?
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
//				this.screw = new Screw(ScrewType.Uni, this.max_avulsion_force, this.is_throughwall, flase, this.d-1.5, this.lenght, th
				break;
			case DowelType.Nail:
				this.img_name = "Summer_work.Imgs.Dowels.nail.png";
				break;
			case DowelType.Standart:
				this.img_name = "Summer_work.Imgs.Dowels.standart.png";
//				this.screw = new Screw(ScrewType.Uni, this.max_avulsion_force, this.is_throughwall, flase, this.d-1.5, this.lenght, th
				break;
			}
		}

		public void Add_Screw(ScrewType type, float d, float lenght, bool is_throughwall,
		                      bool is_selfdrill, float max_avulsion_force, float max_cut_force)
		{
			this.screw = new Screw(type, max_avulsion_force, max_cut_force, is_throughwall,
			                       is_selfdrill, d, lenght, this.accepted_material, this.max_a, this.max_s);
		}

		public override bool CanPassByForce (int vector, float force)
		{
			return (this.max_avulsion_force*vector < force)&&(this.screw.CanPassByForce(vector,force));
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

		public override bool CanPassByLenght (float totalLenght)
		{
			return (this.lenght < totalLenght);
		}

		public override void ReadDataResourses ()
		{
			DowelType type;
			float max_avulsion_force;
			bool is_throughwall;
			bool is_selfdrill;
			float d;
			float lenght;
			Materials[] accepted_material;
			float max_a;
			float max_s;

			Stream str;
			System.Reflection.Assembly _assembly;
			_assembly = Assembly.GetExecutingAssembly ();
			str = _assembly.GetManifestResourceStream ("Summer_work.Data.Dowels.dat");
			string s;
			StreamReader strr = new StreamReader (str);
			while ((s = strr.ReadLine()) != null) {
				while ((s = strr.ReadLine ()) != null && (s[0] == '#'))
					;
				string t;
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				Summer_work.DowelType.TryParse<Summer_work.DowelType> (t, out type);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_avulsion_force = float.Parse (t);
				if (s [0] == '1')
					is_throughwall = true;
				else
					is_throughwall = false;
				s = s.Remove (0, 2);
				if (s [0] == '1')
					is_selfdrill = true;
				else
					is_selfdrill = false;
				s = s.Remove (0, 2);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				d = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				lenght = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);

				//Используя регулярные выражения (pattern - что ищем, source - где ищем):
				int sizeOfMat = new Regex (",").Matches (t).Count;
				accepted_material = new Materials[sizeOfMat];
				for (int i = 0; i < sizeOfMat-1; i++) {
					string t1 = t.Substring (0, t.IndexOf (","));
					t = t.Remove (0, t.IndexOf (",") + 1);
					Materials.TryParse<Summer_work.Materials> (t1, out accepted_material [i]);

				}
				Materials.TryParse<Summer_work.Materials> (t, out accepted_material [sizeOfMat - 1]);

				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_a = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_s = float.Parse (t);

				Dowel dw = new Dowel (type, max_avulsion_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s);

				Storage.dowelsDB.Add (dw);
			}
		}
	}
}

