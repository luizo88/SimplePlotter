using OxyPlot.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlotterMisc
{
    //!/usr/bin/env python
    //-*- coding: utf-8 -*-
    //lib_readPL4_py3.py
    //https://github.com/ldemattos/readPL4
    // 
    //Copyright 2019 Leonardo M. N. de Mattos <l@mattos.eng.br>
    // 
    //This program is free software; you can redistribute it and/or modify
    //it under the terms of the GNU General Public License as published by
    //the Free Software Foundation, version 3.0.
    // 
    //This program is distributed in the hope that it will be useful,
    //but WITHOUT ANY WARRANTY; without even the implied warranty of
    //MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    //GNU General Public License for more details.
    // 
    //You should have received a copy of the GNU General Public License
    //along with this program; if not, write to the Free Software
    //Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
    //MA 02110-1301, USA.

    //Read PISA's binary PL4

    //Converted to C# by luizo (luizo.88@outlook.com + ChatGPT)
    public class PL4Reader
    {
        public static (DataTable, double[,], PL4Metadata) ReadPL4(string pl4file)
        {
            PL4Metadata metadata = new PL4Metadata();
            DataTable dfHEAD = new DataTable();
            double[,] data = null;
            using (FileStream fs = new FileStream(pl4file, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new     BinaryReader(fs))
                {
                    double burn = 0;
                    burn = BitConverter.ToSingle(br.ReadBytes(40), 0);
                    metadata.deltat = BitConverter.ToSingle(br.ReadBytes(4), 0);
                    burn = BitConverter.ToSingle(br.ReadBytes(4), 0);
                    metadata.nvar = BitConverter.ToUInt32(br.ReadBytes(4), 0) / 2;
                    burn = BitConverter.ToSingle(br.ReadBytes(4), 0);
                    metadata.pl4size = BitConverter.ToUInt32(br.ReadBytes(4), 0) - 1;
                    metadata.steps = (metadata.pl4size - 5 * 16 - metadata.nvar * 16) / ((metadata.nvar + 1) * 4);
                    metadata.tmax = (metadata.steps - 1) * metadata.deltat;
                    dfHEAD.Columns.Add("TYPE", typeof(int));
                    dfHEAD.Columns.Add("FROM", typeof(string));
                    dfHEAD.Columns.Add("TO", typeof(string));
                    for (int i = 0; i < metadata.nvar; i++)
                    {
                        br.BaseStream.Seek(5 * 16 + i * 16, SeekOrigin.Begin);
                        byte[] bytes = br.ReadBytes(16);
                        int type = Convert.ToInt32(Encoding.UTF8.GetString(bytes.Skip(3).Take(1).ToArray()).Trim());
                        string from = Encoding.UTF8.GetString(bytes.Skip(4).Take(6).ToArray()).Trim();
                        string to = Encoding.UTF8.GetString(bytes.Skip(10).Take(6).ToArray()).Trim();
                        dfHEAD.Rows.Add(type, from, to);
                    }
                    //for (int i = 0; i < dfHEAD.Rows.Count; i++)
                    //{
                    //    dfHEAD.Rows[i]["TYPE"] = ConvertType((int)dfHEAD.Rows[i]["TYPE"]);
                    //}
                    int expsize = (5 + (int)metadata.nvar) * 16 + (int)metadata.steps * ((int)metadata.nvar + 1) * 4;
                    int nullbytes = 0;
                    if (metadata.pl4size > expsize)
                    {
                        nullbytes = (int)metadata.pl4size - expsize;
                    }
                    int dataOffset = (5 + (int)metadata.nvar) * 16 + nullbytes;
                    int rowCount = (int)metadata.steps;
                    int colCount = (int)metadata.nvar + 1;
                    data = new double[rowCount, colCount];
                    for (int i = 0; i < rowCount; i++)
                    {
                        for (int j = 0; j < colCount; j++)
                        {
                            data[i, j] = BitConverter.ToSingle(br.ReadBytes(4), 0);
                        }
                    }
                }
            }
            return (dfHEAD, data, metadata);
        }

        public static string ConvertType(int type)
        {
            switch (type)
            {
                case 4:
                    return "V-node";
                case 7:
                    return "E-bran";
                case 8:
                    return "V-bran";
                case 9:
                    return "I-bran";
                default:
                    return type.ToString();
            }
        }
    }

    public class PL4Metadata
    {
        public double deltat { get; set; }
        public uint nvar { get; set; }
        public uint pl4size { get; set; }
        public uint steps { get; set; }
        public double tmax { get; set; }
    }
}
