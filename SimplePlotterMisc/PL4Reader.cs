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
                using (BinaryReader br = new BinaryReader(fs))
                {
                    //read the header
                    byte[] bytes_0_36 = br.ReadBytes(36);
                    byte[] bytes_37_40 = br.ReadBytes(4);
                    byte[] bytes_41_44 = br.ReadBytes(4);
                    byte[] bytes_45_48 = br.ReadBytes(4);
                    byte[] bytes_49_52 = br.ReadBytes(4);
                    byte[] bytes_53_56 = br.ReadBytes(4);
                    byte[] bytes_57_60 = br.ReadBytes(4);
                    byte[] bytes_61_64 = br.ReadBytes(4);
                    byte[] bytes_65_68 = br.ReadBytes(4);
                    byte[] bytes_69_72 = br.ReadBytes(4);
                    byte[] bytes_73_80 = br.ReadBytes(8);
                    //convert from bytes
                    double max = BitConverter.ToSingle(bytes_37_40, 0);
                    double dt = BitConverter.ToSingle(bytes_41_44, 0);
                    uint nvar = BitConverter.ToUInt32(bytes_49_52, 0) / 2;
                    uint pl4size = BitConverter.ToUInt32(bytes_57_60, 0) - 1;
                    uint cpx = BitConverter.ToUInt32(bytes_65_68, 0);
                    uint nch = BitConverter.ToUInt32(bytes_69_72, 0);
                    //sets the metadata
                    metadata.simulType = (cpx == 0 && nch == 0) ? ATPSimulType.timeDomain : ATPSimulType.frequencyDomain;
                    switch (metadata.simulType)
                    {
                        case ATPSimulType.timeDomain:
                            metadata.deltat = dt;
                            metadata.nvar = nvar;
                            metadata.pl4size = pl4size;
                            metadata.steps = (pl4size - 5 * 16 - nvar * 16) / ((nvar + 1) * 4);
                            metadata.tmax = (metadata.steps - 1) * dt;
                            metadata.nvarDivisor = 1;
                            break;
                        case ATPSimulType.frequencyDomain:
                            metadata.fmax = max;
                            metadata.nvar = nvar;
                            metadata.pl4size = pl4size;
                            metadata.steps = (pl4size - 5 * 16 - nvar * 16) / ((nvar + 1) * 4);
                            metadata.freqDomainSimulType = ConvertFreqDomainSimulType(cpx, nch);
                            metadata.nvarDivisor = GetNVARDivisor(metadata.freqDomainSimulType);
                            break;
                        default:
                            throw new Exception("Not implemented ATP simulation type (it was supposed to be time- or frequency-domain).");
                    }
                    //creates the data table with the values
                    dfHEAD.Columns.Add("TYPE", typeof(string));
                    dfHEAD.Columns.Add("FROM", typeof(string));
                    dfHEAD.Columns.Add("TO", typeof(string));
                    for (int i = 0; i < metadata.nvar / metadata.nvarDivisor; i++)
                    {
                        br.BaseStream.Seek(5 * 16 + i * 16, SeekOrigin.Begin);
                        byte[] bytes = br.ReadBytes(16);
                        string typeAux = Encoding.UTF8.GetString(bytes.Skip(3).Take(1).ToArray()).Trim();
                        string type = ConvertVarType(Convert.ToInt32(typeAux == "?" ? 0 : Convert.ToInt32(typeAux)));
                        string from = Encoding.UTF8.GetString(bytes.Skip(4).Take(6).ToArray()).Trim();
                        string to = Encoding.UTF8.GetString(bytes.Skip(10).Take(6).ToArray()).Trim();
                        //adds additional data
                        switch (metadata.freqDomainSimulType)
                        {
                            case ATPFreqDomainSimulType.None:
                            case ATPFreqDomainSimulType.ABS:
                                dfHEAD.Rows.Add(type, from, to);
                                break;
                            case ATPFreqDomainSimulType.ABS_ANGLE:
                                dfHEAD.Rows.Add(type + "_ABS", from, to);
                                dfHEAD.Rows.Add(type + "_ARG", from, to);
                                break;
                            case ATPFreqDomainSimulType.Re_Im:
                                dfHEAD.Rows.Add(type + "_Re", from, to);
                                dfHEAD.Rows.Add(type + "_Im", from, to);
                                break;
                            case ATPFreqDomainSimulType.ABS_ANGLE_Re_Im:
                                dfHEAD.Rows.Add(type + "_ABS", from, to);
                                dfHEAD.Rows.Add(type + "_ARG", from, to);
                                dfHEAD.Rows.Add(type + "_Re", from, to);
                                dfHEAD.Rows.Add(type + "_Im", from, to);
                                break;
                            default:
                                throw new Exception("Not implemented ATP frequency-domain simulation type.");
                        }
                    }
                    //check for unexpected rows of zeroes
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

        public static string ConvertVarType(int type)
        {
            switch (type)
            {
                case 0:
                    return "Angle";
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

        public static ATPFreqDomainSimulType ConvertFreqDomainSimulType(uint cpx, uint nch)
        {
            ATPFreqDomainSimulType result = ATPFreqDomainSimulType.None;
            if (cpx == 0 && nch == 1) result = ATPFreqDomainSimulType.ABS;
            if (cpx == 1 && nch == 2) result = ATPFreqDomainSimulType.ABS_ANGLE;
            if (cpx == 2 && nch == 2) result = ATPFreqDomainSimulType.Re_Im;
            if (cpx == 3 && nch == 4) result = ATPFreqDomainSimulType.ABS_ANGLE_Re_Im;
            return result;
        }

        public static int GetNVARDivisor(ATPFreqDomainSimulType freqDomainSimulType)
        {
            switch (freqDomainSimulType)
            {
                case ATPFreqDomainSimulType.ABS_ANGLE:
                case ATPFreqDomainSimulType.Re_Im:
                    return 2;
                case ATPFreqDomainSimulType.ABS_ANGLE_Re_Im:
                    return 4;
                case ATPFreqDomainSimulType.ABS:
                case ATPFreqDomainSimulType.None:
                default:
                return 1;
            }
        }
    }

    public class PL4Metadata
    {
        public double fmax { get; set; }
        public double deltat { get; set; }
        public uint nvar { get; set; }
        public uint pl4size { get; set; }
        public uint steps { get; set; }
        public double tmax { get; set; }
        public ATPSimulType simulType { get; set; }
        public ATPFreqDomainSimulType freqDomainSimulType { get; set; }
        public int nvarDivisor { get; set; }
    }

    public enum ATPSimulType
    {
        timeDomain = 0,
        frequencyDomain = 1
    }

    public enum ATPFreqDomainSimulType
    {
        None = 0,
        ABS = 1,
        ABS_ANGLE = 2,
        Re_Im = 3,
        ABS_ANGLE_Re_Im = 4
    }
}
