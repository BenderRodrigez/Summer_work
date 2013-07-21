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
			return (this.lenght < totalLenght);
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
			bool answer = false;
			for(int i = 0; i < this.accepted_material.Length; i++)
				if(accepted_material[i] == what)
					answer = true;
			for(int i = 0; i < this.accepted_material.Length; i++)
				if(accepted_material[i] == wher)
					answer = true;
			return answer;
		}

		public override void ReadDataResourses ()
		{
			ScrewType type;
			float max_avulsion_force;
			float max_cut_force;
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
			str = _assembly.GetManifestResourceStream ("Summer_work.Data.Screw.dat");
			string s;
			StreamReader strr = new StreamReader (str);
			while ((s = strr.ReadLine()) != null) {
				while ((s = strr.ReadLine ()) != null && (s[0] == '#'));
				string t;
				t = s.Substring(0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				Summer_work.ScrewType.TryParse<Summer_work.ScrewType>(t, out type);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_avulsion_force = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_cut_force = float.Parse (t);
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
					string t1 = t.Substring(0, t.IndexOf(","));
					t = t.Remove(0, t.IndexOf(",")+1);
					Materials.TryParse<Summer_work.Materials>(t1, out accepted_material[i]);

				}
				Materials.TryParse<Summer_work.Materials>(t, out accepted_material[sizeOfMat-1]);

				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_a = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_s = float.Parse (t);

				Screw sc = new Screw(type, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s);
				Storage.screwDB.Add(sc);
			}
		}
	}
}

