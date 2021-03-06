﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class Editor
{
    public static List<STRUCT_MOB> MOBs = new List<STRUCT_MOB>();
    public static List<STRUCT_MOB> NPCs = new List<STRUCT_MOB>();
    public static sITEMLIST ItemList = new sITEMLIST();
    public static List<STRUCT_MOB> MOBsLocalizados = new List<STRUCT_MOB>();

    public static void ReadNpc()
    {
        try
        {
            string DirNPC = Directory.GetCurrentDirectory() + @"\npc";

            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(DirNPC);
            foreach (string fileName in fileEntries)
            {
                Byte[] data = File.ReadAllBytes(fileName);
                STRUCT_MOB pMob = (STRUCT_MOB)Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), typeof(STRUCT_MOB));
                STRUCT_MOB pNpc = (STRUCT_MOB)Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), typeof(STRUCT_MOB));

                if (pMob.Merchant == 0)
                {
                    Editor.MOBs.Add(pMob);
                }

                if (pMob.Merchant != 0)
                {
                    Editor.NPCs.Add(pNpc);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    public static void SaveNPC(STRUCT_MOB NPC)
    {
        try
        {
            string DirNPC = Directory.GetCurrentDirectory() + @"\npc\";

            byte[] arr = new byte[Marshal.SizeOf(NPC)];

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(NPC));
            Marshal.StructureToPtr(NPC, ptr, false);
            Marshal.Copy(ptr, arr, 0, Marshal.SizeOf(NPC));
            Marshal.FreeHGlobal(ptr);

            File.WriteAllBytes(DirNPC + NPC.name, arr);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    public static void ReadItemList()
    {
        try
        {
            Byte[] data = File.ReadAllBytes("Itemlist.bin");
            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= 0x5A;
            }

            ItemList = (sITEMLIST)Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), typeof(sITEMLIST));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    public static void ReadNPCNovo(List<STRUCT_MOB> Localizados)
    {
        try
        {
            Editor.MOBs.Clear();

            foreach (var item in Localizados)
            {
                string NPC = Directory.GetCurrentDirectory() + @"\npc\" + item.name;
                Byte[] data = File.ReadAllBytes(NPC);
                STRUCT_MOB pMob = (STRUCT_MOB)Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), typeof(STRUCT_MOB));
                Editor.MOBs.Add(pMob);
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    public int LocalizarItens(int index)
    {
        int encontrados = 0;
        string DirNPC = Directory.GetCurrentDirectory() + @"\npc";

        // Process the list of files found in the directory.
        string[] fileEntries = Directory.GetFiles(DirNPC);
        foreach (string fileName in fileEntries)
        {
            Byte[] data1 = File.ReadAllBytes(fileName);
            STRUCT_MOB MobsLocalizados = (STRUCT_MOB)Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(data1, 0), typeof(STRUCT_MOB));

            for (int i = 0; i < 64; i++)
            {
               if(MobsLocalizados.Carry[i].sIndex == index)
                {                    
                    encontrados++;
                    MOBsLocalizados.Add(MobsLocalizados);
                }
            }
        }

            return encontrados;
    }
}