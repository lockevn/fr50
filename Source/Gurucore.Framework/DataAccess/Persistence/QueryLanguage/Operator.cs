using System;

namespace Gurucore.Framework.DataAccess.Persistence.QueryLanguage
{
	public class Operator
	{
		public static Operator Value = new Operator("Value",string.Empty,true);

		public static Operator Add = new Operator("Add","+",false);
		public static Operator Sub = new Operator("Sub","-",false);
		public static Operator Mul = new Operator("Mul","*",false);
		public static Operator Div = new Operator("Div","/",false);

		public static Operator Neg = new Operator("Neg","-",true);

		public static Operator And = new Operator("And","AND",false);
		public static Operator Or = new Operator("Or","OR",false);
		public static Operator Xor = new Operator("Xor","XOR",false);
		public static Operator Not = new Operator("Not","NOT",true);
		
		public static Operator Like = new Operator("Like","LIKE",false);
		public static Operator In = new Operator("In","IN",false);
		public static Operator NotIn = new Operator("NotIn","NOT IN",false);
		public static Operator Between = new Operator("Between","BETWEEN",false);

		public static Operator Gt = new Operator("Gt",">",false);
		public static Operator Lt = new Operator("Lt","<",false);
		public static Operator Eq = new Operator("Eq","=",false);
		public static Operator Neq = new Operator("Neq","<>",false);
		public static Operator Gte = new Operator("Gte",">=",false);
		public static Operator Lte = new Operator("Lte","<=",false);

		public static Operator IsNull = new Operator("IsNull","IS NULL",false);
		public static Operator IsNotNull = new Operator("IsNotNull","IS NOT NULL",false);

		private string m_sName;
		private string m_sSignature;
		private bool m_bUnary;

		private Operator(string p_sName, string p_sSignature, bool p_bUnary)
		{
			m_sName = p_sName;
			m_sSignature = p_sSignature;
			m_bUnary = p_bUnary;
		}

		public override string ToString()
		{
			return m_sSignature;
		}

		public bool IsUnary()
		{
			return m_bUnary;
		}
	}
}
