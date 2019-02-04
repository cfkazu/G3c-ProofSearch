using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace G3c
{
   public class Formula
    {
        public List<Formula> Kodomo = new List<Formula>();
        
        public List<string> maestrings = new List<string>();//シークエント計算の前提に当たる
        public List<string> atostrings = new List<string>();//結論に当たる
        public static char[] primaly = new char[] {'￢','∧','∨','⇒','*' };//論理演算子として予約されている記号。最後の*はダミーとして用いる
        public Formula(List<string> maestrings, List<string> atostrings)
        {
            this.maestrings = new List<string>(maestrings);this.atostrings = new List<string>(atostrings);
        }
        public Formula(){}
        public Figure Simplimize()//再帰的に演算子の除去を行う
        {
            string res = MakeStringForOutput();

            for (int k = 0; k < maestrings.Count(); k++)
            {
                int i = maestrings[k].Length - 1;

                if (maestrings[k][i] == '￢')
                {
                   Formula form = DeepCopyArranged(true, k);
                    form.atostrings.Add(maestrings[k].Substring(0, maestrings[k].Length - 1));
                    Kodomo.Add(form);
                    goto end;
                }
                else if (primaly.Contains(maestrings[k][i]))
                {
                    Formula form = DeepCopyArranged(true, k);
                    Formula form2 = DeepCopyArranged(true, k);
                    string[] maeato = Divide(maestrings[k]);
                    string maecoded = Coding(maeato[0]);
                    string atocoded = Coding(maeato[1]);
                    switch (maestrings[k][i])
                    {
                        case '∧':
                            form.maestrings.Add(maecoded);
                            form.maestrings.Add(atocoded);
                            Kodomo.Add(form);
                            goto end;
                        case '∨':
                            form.maestrings.Add(maecoded);
                            form2.maestrings.Add(atocoded);
                            Kodomo.Add(form); Kodomo.Add(form2);
                            goto end;
                        case '⇒':
                            form.atostrings.Add(maecoded);
                            form2.maestrings.Add(atocoded);
                            Kodomo.Add(form); Kodomo.Add(form2);
                            goto end;
                        default:
                            break;
                    }
                }
                else if(maestrings[k][i]== '⊥')
                {
                    Formula form = new Formula();
                    Kodomo.Add(form);
                    goto end;
                }

            }

            for (int k = 0; k < atostrings.Count(); k++)
            {
                int i = atostrings[k].Length - 1;
                if (atostrings[k][i] == '￢')
                {
                    Formula form = DeepCopyArranged(false, k);
                    form.maestrings.Add(atostrings[k].Substring(0, atostrings[k].Length - 1));
                    Kodomo.Add(form);
                    goto end;
                }
                else if(primaly.Contains(atostrings[k][i]))
                {
                    Formula form = DeepCopyArranged(false, k);
                    Formula form2 = DeepCopyArranged(false, k);
                    string[] maeato = Divide(atostrings[k]);
                    string maecoded = Coding(maeato[0]);
                    string atocoded = Coding(maeato[1]);
                    switch (atostrings[k][i])
                    {
                        case '∧':
                            form.atostrings.Add(maecoded);
                            form2.atostrings.Add(atocoded);
                            Kodomo.Add(form); Kodomo.Add(form2);
                            goto end;
                        case '∨':
                            form.atostrings.Add(maecoded);
                            form.atostrings.Add(atocoded);
                            Kodomo.Add(form);
                            goto end;
                        case '⇒':
                            form.maestrings.Add(maecoded);
                            form.atostrings.Add(atocoded);
                            Kodomo.Add(form);
                            goto end;
                        default:
                            break;
                    }
                }
            }


            end:;//一つでも演算子を除去出来たらここにgotoする
            Figure result = new Figure(res);
            foreach (var i in Kodomo)
            {
                result.Addfigure(i.Simplimize());
            }
            return result;
        }
        public Formula DeepCopyArranged(bool mae,int basyo)//basyo番目の式を除いたディープコピーを作成。演算子除去した式自体は削除される
        {
            Formula form = new Formula(maestrings, atostrings);
            if(mae)
            {
                form.maestrings.RemoveAt(basyo);
            }
            else
            {
                form.atostrings.RemoveAt(basyo);
            }
            return form;
        }
        public static string[] Divide(string code)//逆ポーランド化された論理式を受け取り、その前と後ろにある論理を返す:(A∧B)なら{A,B}を返す
        {
            if(code[code.Length-1]=='￢')
            {
                return new string[] { code.Substring(0, code.Length - 1) };
            }
            else
            {
                code = code.Substring(0, code.Length - 1) + '*';//ダミーの*に置き換えることでデコードしても場所が分かる
                string source = Decoding(code);
                string mae = new string(source.TakeWhile(c => c != '*').ToArray());
                string ato = new string(source.SkipWhile(c => c != '*').Skip(1).ToArray());
                return new string[] { mae, ato };
            }

        }
        public static string Coding(string source)//逆ポーランド化
        {
            var stack = new Stack<char>();
            StringBuilder buffa = new StringBuilder();
            foreach (var i in source)
            {

                if (i == '(') { stack.Push(i); }//仮定:すべてで括弧がついている
                else if (primaly.Contains(i))
                {
                    stack.Push(i);//仮定に基づき、演算子ならとにかくスタックにプッシュ
                }
                else if(i==')')
                {
                    while (stack.Peek()!='(')
                    {
                        buffa.Append(stack.Pop());//
                    }
                    stack.Pop();//(を捨てる
                }
                else { buffa.Append(i); }

            }
            while (stack.Any())
            {
                buffa.Append(stack.Pop());
            }
            return buffa.ToString();
        }

        public static string Decoding(string Code)//逆ポーランド化された論理式を元に戻す
        {
            StringBuilder buffa = new StringBuilder();
            var stack = new Stack<string>();
            foreach (var i in Code)
            {
                if (primaly.Contains(i))
                {
                    if (i == '￢') { stack.Push("(" + i + "" + stack.Pop() + ")"); }
                    else
                    {
                        var mae = stack.Pop();
                        var ato = stack.Pop();
                        stack.Push("(" + ato + i + mae + ")");
                    }
                }
                else
                { stack.Push(i.ToString()); }
            }
            string result = stack.Pop();
            if (result.Contains('(')) { result= result.Substring(1, result.Length - 2); }
            return result;
        }

        public string MakeStringForOutput()//出力用に、現在の式を通常の書式で出力
        {

            string res = "";
            foreach (var logic in maestrings)
            {
                res += Decoding(logic);
                res += ",";
            }
            res += "→";
            foreach (var logic in atostrings)
            {
                res += Decoding(logic);
                res += ",";
            }
            return res;
        }
           
    }
}
