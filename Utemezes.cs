using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Csengo_utemezes
{
    public struct Nap
    {
        public String datumido;
        public Boolean csengo;
        public Nap(String s, Boolean b)
        {
            datumido = s;
            csengo = b;
        }
    }
    public struct Datum
    {
        public int ev;
        public int ho;
        public int nap;
        public Datum(int e, int h, int n)
        {
            ev = e;
            ho = h;
            nap = n;
        }
        public override string ToString()
        {
            String two_dig_ho, two_dig_nap;
            if (ho < 10)
            {
                two_dig_ho = "0" + ho.ToString();
            }
            else
            {
                two_dig_ho = ho.ToString();
            }
            if (nap < 10)
            {
                two_dig_nap = "0" + nap.ToString();
            }
            else
            {
                two_dig_nap = nap.ToString();
            }
            return ev.ToString() + two_dig_ho + two_dig_nap;
        }
    }
    public class Utemezes
    {
        List<Nap> napok;
        String rulesFilePath;
        public Utemezes(String fp)
        {
            napok = new List<Nap>();
            rulesFilePath = fp;
            loadBells();
        }
        public void loadBells()
        {
            List<String> daysOfWeek = new List<string>();
            List<Datum> affectedDays = new List<Datum>();
            Datum kezd, veg;
            StreamReader sr = new StreamReader(rulesFilePath);
            while (sr.Peek() != -1)
            {
                daysOfWeek.Clear();
                Boolean setTo;
                DateTime myDT;
                Calendar myCal = CultureInfo.InvariantCulture.Calendar;
                affectedDays.Clear();
                string line = "";
                do
                {
                    line = sr.ReadLine();
                } while (line.Length == 0 || line.Contains("//"));
                string[] temp = line.Split(' ');
                if (temp[0] == "from" && temp[2] == "to")
                {
                    kezd = new Datum(Int32.Parse(temp[1].Split('/')[0]), Int32.Parse(temp[1].Split('/')[1]), Int32.Parse(temp[1].Split('/')[2]));
                    veg = new Datum(Int32.Parse(temp[3].Split('/')[0]), Int32.Parse(temp[3].Split('/')[1]), Int32.Parse(temp[3].Split('/')[2]));
                    do
                    {
                        line = sr.ReadLine();
                    } while (line.Length == 0 || line.Contains("//"));
                    temp = line.Split(' ');
                    if (temp[0] == "every")
                    {
                        if (temp[1] == "day")
                        {
                            daysOfWeek.Add("monday");
                            daysOfWeek.Add("tuesday");
                            daysOfWeek.Add("wednesday");
                            daysOfWeek.Add("thursday");
                            daysOfWeek.Add("friday");
                            daysOfWeek.Add("saturday");
                            daysOfWeek.Add("sunday");
                        }
                        else if (temp[1] == "weekday")
                        {
                            daysOfWeek.Add("monday");
                            daysOfWeek.Add("tuesday");
                            daysOfWeek.Add("wednesday");
                            daysOfWeek.Add("thursday");
                            daysOfWeek.Add("friday");
                        }
                        else if (temp[1] == "weekend")
                        {
                            daysOfWeek.Add("saturday");
                            daysOfWeek.Add("sunday");
                        }
                        else
                        {
                            for (int i = 1; i < temp.Length; i++)
                            {
                                daysOfWeek.Add(temp[i]);
                            }
                        }
                        myDT = new DateTime(kezd.ev, kezd.ho, kezd.nap, new GregorianCalendar());
                        while (myDT != (new DateTime(veg.ev, veg.ho, veg.nap, new GregorianCalendar())).AddDays(1))
                        {
                            if (daysOfWeek.Contains(myCal.GetDayOfWeek(myDT).ToString().ToLower()) == true)
                            {
                                affectedDays.Add(new Datum(myDT.Year, myDT.Month, myDT.Day));
                            }
                            myDT = myCal.AddDays(myDT, 1);
                        }
                        do
                        {
                            line = sr.ReadLine();
                        } while (line.Length == 0 || line.Contains("//"));
                        temp = line.Split(' ');
                        if (temp[0] == "set")
                        {
                            setTo = true;
                        }
                        else
                        {
                            setTo = false;
                        }
                        foreach (Datum d in affectedDays)
                        {
                            for (int i = 2; i < temp.Length; i++)
                            {
                                String o, p;
                                if (Int32.Parse(temp[i].Split(':')[0]) < 10)
                                {
                                    o = "0" + temp[i].Split(':')[0];
                                }
                                else
                                {
                                    o = temp[i].Split(':')[0];
                                }
                                if (Int32.Parse(temp[i].Split(':')[1]) < 10 && temp[i].Split(':')[1].Equals("00") == false)
                                {
                                    p = "0" + temp[i].Split(':')[1];
                                }
                                else
                                {
                                    p = temp[i].Split(':')[1];
                                }
                                napok.Add(new Nap(d.ToString() + o + p, setTo));
                            }
                        }
                    }
                }
                //Itt jön az "on"
                else if (temp[0] == "on")
                {
                    for (int i = 1; i < temp.Length; i++)
                    {
                        affectedDays.Add(new Datum(Int32.Parse(temp[i].Split('/')[0]), Int32.Parse(temp[i].Split('/')[1]), Int32.Parse(temp[i].Split('/')[2])));
                    }
                    do
                    {
                        line = sr.ReadLine();
                    } while (line.Length == 0 || line.Contains("//"));
                    temp = line.Split(' ');
                    if (temp[0] == "set")
                    {
                        setTo = true;
                    }
                    else
                    {
                        setTo = false;
                    }
                    foreach (Datum d in affectedDays)
                    {
                        for (int i = 2; i < temp.Length; i++)
                        {
                            String o, p;
                            if (Int32.Parse(temp[i].Split(':')[0]) < 10)
                            {
                                o = "0" + temp[i].Split(':')[0];
                            }
                            else
                            {
                                o = temp[i].Split(':')[0];
                            }
                            if (Int32.Parse(temp[i].Split(':')[1]) < 10 && temp[i].Split(':')[1].Equals("00") == false)
                            {
                                p = "0" + temp[i].Split(':')[1];
                            }
                            else
                            {
                                p = temp[i].Split(':')[1];
                            }
                            napok.Add(new Nap(d.ToString() + o + p, setTo));
                        }
                    }
                }
            }
            sr.Close();
        }
    }
}
