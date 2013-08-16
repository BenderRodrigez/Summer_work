using System;
using Summer_work;
using System.Collections.Generic;
using System.Resources;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Summer_work
{
	public class Calculator
	{
		public enum Concrete{
			M100, M150, M200, M250, M300, M350, M400, M450, M500
		}

		public static void initConcrete()
		{
			for(int i = (int)Concrete.M100; i <= (int)Concrete.M500; i++)
			{
				props[i] = new float[3];
				props[i][0] = 1;
			}
			//gravel								sand
			props[(int)Concrete.M100][1] = 7; props[(int)Concrete.M100][2] = 4.6f;
			props[(int)Concrete.M150][1] = 5.7f; props[(int)Concrete.M150][2] = 3.5f;
			props[(int)Concrete.M200][1] = 4.8f; props[(int)Concrete.M200][2] = 2.8f;
			props[(int)Concrete.M250][1] = 3.9f; props[(int)Concrete.M250][2] = 2.1f;
			props[(int)Concrete.M300][1] = 3.7f; props[(int)Concrete.M300][2] = 1.9f;
			props[(int)Concrete.M350][1] = 2.5f; props[(int)Concrete.M350][2] = 1.8f;
			props[(int)Concrete.M400][1] = 2.7f; props[(int)Concrete.M400][2] = 1.2f;
			props[(int)Concrete.M450][1] = 2.5f; props[(int)Concrete.M450][2] = 1.1f;
			props[(int)Concrete.M500][1] = 3.9f; props[(int)Concrete.M500][2] = 1.5f;
		}

		public static float formwork (float h, float l, float w, float d, float add_P)
		{
			h += 0.2f;
			return (2f*(h*l+h*w)+2f*(h*(l-2f*d)+h*(w-2f*d))+add_P*h*2)*0.025f;
		}

		public static float foundamentVolume (float h, float l, float d, float lJmp, float a)
		{
			return h*(l*a - (l-2*d)*(a-2*d)+lJmp*d);
		}

		public static float armsTotalLenght (float P, float h, float d)
		{
			float K, H = h - 0.05f, R = 2f, T = 0.1f, N = 0.1f, L = 0.1f;
			if (d <= 0.4)
				K = 2;
			else {
				if(d<=0.8)
					K= 4;
				else
					K = 6;
			}
			return P * R * (K + ((K - 1f) * T + 0.05f) / L + ((R - 1f) * H + 0.05f) / N);
		}

		public static float[] concreteMaterials (float foundamentVolume, Concrete type)
		{
			float[] st = new float[3];
			st = props[(int)type];
			float mCon = 1 + st[1] + st[2];
			float K = 2400*foundamentVolume/mCon;
			st[0] *= K;
			st[1] *= K;
			st[2] *= K;
			return st;
		}

		static float[][] props = new float[(int)Concrete.M500+1][];//cement, gravel, sand

		public static Dowel DowelToScrew(Screw scr, Materials what, Materials wher)
		{
			foreach(Dowel dwl in Storage.dowelsDB)
			{
				if(dwl.type != DowelType.Nail){
					if((scr.lenght < (dwl.lenght/0.8)) && (scr.lenght > (dwl.lenght*0.5))){
						if(scr.d >= dwl.accepted_screw_d[0] && scr.d <= dwl.accepted_screw_d[dwl.accepted_screw_d.Length-1])
							return dwl;
					}
				}
			}
			return null;
		}

		public static void GenerateByMaterialList (Materials what, Materials wall)//In Dowel is only Nail in work! Standart, Butterfly in Screw
		{
			if (Storage.passed.Count < 1) {
				foreach (Anchor anch in Storage.anchorsDB) {
					if (anch.CanByMaterial (what, wall))
					{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (anch);
					}
				}
				foreach (Dowel dow in Storage.dowelsDB) {
					if(dow.type != DowelType.Butterfly && dow.type != DowelType.Standart){
						if (dow.CanByMaterial (what,  wall))
						{
							if(Storage.passed.Capacity-1 == Storage.passed.Count)
								Storage.passed.Capacity*=2;
							Storage.passed.Add (dow);
						}
					}
				}
				foreach (Screw scr in Storage.screwDB) {
					if (scr.CanByMaterial (what, wall))
					{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (scr);
					}
				}
			} else {
				List<Mount> toDelete = new List<Mount>();
				foreach(Mount mn in Storage.passed){
					if(!mn.CanByMaterial(what, wall))
						toDelete.Add(mn);
				}
				foreach(Mount mn in toDelete){
						Storage.passed.Remove(mn);
				}
			}
		}

		public static void GenerateByLenght (float wallLenght, float objLenght)
		{
			if (Storage.passed.Count < 1) {
				foreach (Anchor anch in Storage.anchorsDB) {
					if (anch.CanPassByLenght (wallLenght, objLenght))
					{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (anch);
					}
				}
				foreach (Dowel dow in Storage.dowelsDB) {
					if (dow.CanPassByLenght (wallLenght, objLenght))
					{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (dow);
					}				}
				foreach (Screw scr in Storage.screwDB) {
					if (scr.CanPassByLenght (wallLenght, objLenght))
						{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (scr);
					}
				}
			}
			else {
				List<Mount> toDelete = new List<Mount>();
				foreach(Mount mn in Storage.passed){
					if(!mn.CanPassByLenght(wallLenght, objLenght))
						toDelete.Add(mn);
				}
				foreach(Mount mn in toDelete){
						Storage.passed.Remove(mn);
				}
			}
		}

		public static void GenerateByForce (int vector, float force)
		{
			if (Storage.passed.Count < 1) {
				foreach (Anchor anch in Storage.anchorsDB) {
					if (anch.CanPassByForce(vector, force))
						{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (anch);
					}
				}
				foreach (Dowel dow in Storage.dowelsDB) {
					if (dow.CanPassByForce(vector, force))
						{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (dow);
					}
				}
				foreach (Screw scr in Storage.screwDB) {
					if (scr.CanPassByForce(vector, force))
						{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (scr);
					}
				}
			}
			else {
				List<Mount> toDelete = new List<Mount>();
				foreach(Mount mn in Storage.passed){
					if(!mn.CanPassByForce(vector, force))
						toDelete.Add(mn);
				}
				foreach(Mount mn in toDelete){
						Storage.passed.Remove(mn);
				}
			}
		}

		public Calculator ()
		{

		}
	}
}

