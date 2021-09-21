using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

//todo optimize code
//todo add toggle for overwriting code

namespace PointerAdjuster
{
    public partial class Form1 : Form
    {
        string filePath = "";

        int test = 0;

        string[][] patchArray;
        //string[,] patchArray;
        //List<byte> dataList = new List<byte>();

        //private string originalPath = "";
        //private string modifiedPath = "";

        private string fileStart = "0";
        private string entryTotal = "0";

        private string originalMiddleOffsetReadOnly = "";
        //private string originalMiddleOffset;
        private string modifiedMiddleOffset;
        private byte[] modifiedMiddleOffsetBytes;

        //private string originalSecondPartStart.Text;
        private string modifiedSecondPartStart;

        private string modifiedMiddleOffset2;

        //public bool alreadyInjected = false;
        public string newFileCode = "";
        private int originalLength;
        private int modifiedLength;


        int cumulativeIncrease = 0;
        //todo add inject location option for top, bottom code


        int[] fileListLocation = { 0, 0 };

        string[,] fileList = new string[,]{

            //todo fill in rest of these
            //(0)how many parts, (1)file name, (2)first part middle, (3)second part start, (4)second part middle
            {"1", "", "", "", "" },

            //unit_ActionData

            {"1", "unit_Attack_Param", "B75C0", "", ""},
            {"1", "unit_Battle_BGM", "2020", "", "" },
            {"1", "unit_Battle_Entry_Talk_Data", "7BD0", "", "" },
            {"1", "unit_Battle_Victory_Talk_Data", "B350", "", "" },
            {"1", "unit_Camera_Data", "F140", "", "" },
            {"1", "unit_Demo_Setting_Data", "280", "", "" },
            {"1", "unit_Knockback_Param", "9B46C", "", "" },
            {"1", "unit_TableAttackComboRate", "B8", "", "" },
            {"1", "unit_TableAttackUpParam", "28", "", "" },
            {"1", "unit_TableDefenceParam", "860", "", "" },
            {"1", "unit_TableHitEffect", "33D0", "", "" },
            {"1", "unit_TablePostEffParam", "588", "", "" },
            {"1", "unit_TableShakeParam", "6C8", "", "" },
            {"1", "unit_TableVibrationParam", "9C", "", "" },

            //unit_GalleryData
            {"1", "unit_AnimeGallery_Data", "370", "", "" },
            {"1", "unit_Digital_Figure_Data", "4A20", "", "" },
            {"1", "unit_Glossary_Data", "15CC", "", "" },
            {"1", "unit_SoundTest_Data", "AD4", "", "" },
            {"1", "unit_VoiceLibrary_Data", "1896C", "", "" },
            {"1", "unit_VoiceLibrary_Tab_Data", "1A6", "", "" },

            //unit_master_data
            {"1", "unit_Acquire_GP_Data", "E90", "", "" },
            {"1", "unit_Action_Data", "142C", "", "" },
            {"1", "unit_Actor_Status_Data", "3424", "", "" },
            {"1", "unit_AutoMatch_Search_Setting", "1F0", "", "" },
            {"1", "unit_BGM_Data", "154", "", "" },
            {"1", "unit_Challenge_Evaluate_Data", "B0", "", "" },
            {"1", "unit_Challenge_Lottery_Data", "48", "", "" },
            {"1", "unit_Challenge_Setting_Data", "20", "", "" },
            {"1", "unit_Chapter_Data", "4C0", "", "" },
            {"1", "unit_Character_Color_Data", "740", "", "" },
            {"1", "unit_Character_Data", "2530", "", "" },
            {"1", "unit_CharaSelectPositioningData", "", "", "" },
            {"1", "unit_CharaSelectPositionLinkData", "", "", "" },
            {"1", "unit_Chat_Data", "5A10", "", "" },
            {"1", "unit_CommandList_Setting", "DAD0", "", "" },
            {"1", "unit_Config_Param", "718", "", "" },
            {"1", "unit_DLC_Data", "A0", "", "" },
            {"1", "unit_Environment_Data", "494", "", "" },
            {"1", "unit_Figure_Data", "2890", "", "" },
            {"1", "unit_KeyGuide_Define_Data", "FB0", "", "" },
            {"1", "unit_Lock_Contents_Data", "1B0", "", "" },
            {"1", "unit_Lock_Contents_Gallery_Data", "920", "", "" },
            {"1", "unit_Menu_System_Voice_Data", "1114", "", "" },
            {"1", "unit_Model_Color_Data", "1078", "", "" },
            {"1", "unit_Musou_Battle_Data", "6D0", "", "" },
            {"1", "unit_Musou_Item_Data", "154", "", "" },
            {"1", "unit_Musou_Lottery_Data", "A0", "", "" },
            {"1", "unit_Rank_Data", "2E0", "", "" },
            {"1", "unit_Stage_Data", "970", "", "" },
            {"1", "unit_Story_Advance_Data", "41C", "", "" },
            {"1", "unit_Story_Battle_Character_Data", "2738", "", "" },
            {"1", "unit_Story_Battle_Evaluate_Data", "9D0", "", "" },
            {"1", "unit_Story_Battle_Talk_Data", "448", "", "" },
            {"1", "unit_Story_Data", "E10", "", "" },
            {"1", "unit_Training_Menu_Data", "6A0", "", "" },
            {"1", "unit_Trophy_Data", "490", "", "" },
            {"1", "unit_Tutorial_Talk_Data", "85C", "", "" },

            //unit_SpeechData

            {"1", "unit_Decisin_Arrest_Data", "1A90", "", "" },
            {"1", "unit_Decisin_Demo_Setting", "670", "", "" },
            {"1", "unit_Decisin_Speech_Attack_Data", "6F8", "", "" },
            {"1", "unit_Decisin_Speech_Camera_Data", "9E8", "", "" },
            {"1", "unit_Decisin_Speech_Conflict_Dat", "3C8", "", "" },
            {"1", "unit_Decisin_Speech_Data", "5A4C", "", "" },
            {"1", "unit_Decisin_Speech_Merit_Data", "6F8", "", "" },
            {"1", "unit_Decisin_Speech_Position_Dat", "1E4", "", "" },
            {"1", "unit_Decisin_Story_Arrest_Data", "2D0", "", "" },
            {"1", "unit_Decisin_Story_Speech_Data", "ED4", "", "" },

            //{"1", "", "" },


            //unit_unit_data
            {"1", "unit_unitid_list", "985C", "", "" },


            {"2", "ACT_C01_RYU1_00", "A88AC", "26B310", "27726C" },
            {"2", "ACT_C01_RYU2_00", "C2DC0", "2EEB60", "2FC560" },
            {"2", "ACT_C11_MOB1_00", "24450", "8A538",  "8D748"},

            {"1", "015_RYU_MOD", "338", "",  ""}


        };








        public Form1()
        {
            InitializeComponent();

            //InitializeVariables();

            error1.Text = "";
            error2.Text = "";
            error3.Text = "";
            error4.Text = "";
            label10.Text = "";
            label11.Text = "";
            label17.Text = "";
            errorLabel.Text = "";

            error0.Text = "";
            error5.Text = "";

            //this.originalPath.DragDrop += new
            //System.Windows.Forms.DragEventHandler(this.textBox3_DragDrop);
            // this.originalPath.DragEnter += new
            //System.Windows.Forms.DragEventHandler(this.textBox3_DragEnter);

            this.modifiedPath.DragDrop += new
                System.Windows.Forms.DragEventHandler(this.modifiedPath_DragDrop);
            this.modifiedPath.DragEnter += new
                System.Windows.Forms.DragEventHandler(this.modifiedPath_DragEnter);

            this.patchPath.DragDrop += new
                System.Windows.Forms.DragEventHandler(this.patchPath_DragDrop);
            this.patchPath.DragEnter += new
                System.Windows.Forms.DragEventHandler(this.patchPath_DragEnter);



            //originalPath.Text = @"C:\SteamLibrary\steamapps\common\KILL la KILL -IF\ResourceWin\DataBase\unit master data\testing\unit_Character_Color_Data.bin";
            //modifiedPath.Text = @"C:\SteamLibrary\steamapps\common\KILL la KILL -IF\ResourceWin\DataBase\unit master data\testing\unit_Character_Color_Data.bin";
            //modifiedPath.Text = @"C:\SteamLibrary\steamapps\common\KILL la KILL -IF\ResourceWin\script\script character\step changes\ACT_C01_RYU1_00.spb";
            //modifiedPath.Text = @"C:\SteamLibrary\steamapps\common\KILL la KILL -IF\ResourceWin\KlkResources\C011_RYU\youfuku testing\015_RYU_MOD.mdl";
            ////modifiedPath.Text = @"C:\SteamLibrary\steamapps\common\KILL la KILL -IF\ResourceWin\KlkResources\C071_RAG\EXT\MOT_071_RAG_ACT\test\1010.mot";

            //pointerCode.Text = "88450200 ";


            //general-use
            ////pointerCode.Text = "70000000 02000000 50000000 02000000 98000000 02000000";
            //pointerCode.Text = "70790700 02000000 50790700 02000000 98790700 02000000";
            //regularCode.Text = "C8460200 01001F00";


            //replaces DB000000 with 01000000, so that all animations aren't 219 frames long
            ////regularCode.Text = "00000000 80FEF5BD E0290ABF 03ECC23D 01000000 88FEF5BD C0290ABF 03ECC23D 00000000 F6FF17B4 AACD7EB6 85297CB3 0000803F 01000000 00000000 00000000 00000000 0000803F 00000000 F8FF7F3F 0200803F F6FF7F3F 01000000 F8FF7F3F 0300803F F8FF7F3F";
            //regularCode.Text = "00000000 80FEF5BD E0290ABF 03ECC23D DB000000 88FEF5BD C0290ABF 03ECC23D 00000000 F6FF17B4 AACD7EB6 85297CB3 0000803F DB000000 00000000 00000000 00000000 0000803F 00000000 F8FF7F3F 0200803F F6FF7F3F DB000000 F8FF7F3F 0300803F F8FF7F3F";
            ////pointerCode2.Text = "C2E00900 B8220000 FAE00900 ";
            ////regularCode2.Text = "3AE50900 75230000 82E50900 77230000";


            //originalMiddleOffset.Text = "740";
            //modifiedMiddleOffset = "774";

            //originalLength = (new System.IO.FileInfo(modifiedPath.Text).Length) * 2;

            //modifiedLength = new System.IO.FileInfo(modifiedPath.Text).Length * 2;



        }





        //private void textBox3_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //        e.Effect = DragDropEffects.All;
        //    else
        //        e.Effect = DragDropEffects.None;
        //}

        //private void textBox3_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        //{

        //    string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
        //    int i;

        //    // originalPath.Text = (s[0]);


        //    for (i = 0; i < s.Length; i++)
        //    {
        //        //textBox3.Clear();
        //        //originalPath.Text = (s[i]);

        //    }
        //}


        private void modifiedPath_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void modifiedPath_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {

            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            modifiedPath.Text = (s[0]);

            //GetFileInfo();
            ////todo next make all this stuff run when button pressed
            //string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            ////int i;
            //modifiedPath.Text = (s[0]);
            //filePath = modifiedPath.Text;
            ////for (i = 0; i < s.Length; i++)
            ////{
            ////textBox4.Items.Clear();
            ////modifiedPath.Text = (s[i].Length.ToString());
            ////modifiedPath = s[i];

            ////todo change to work with bytes. remove *2 here
            //originalLength = Convert.ToInt32(new System.IO.FileInfo(modifiedPath.Text).Length) * 2;








            //int rowCount = fileList.GetLength(0);

            //int colCount = fileList.GetLength(1);
            ////label5.Text = fileList[5, 1] + rowCount + colCount + "00";




            //if (modifiedPath.Text.Contains(".mot"))
            //{



            //    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            //    //int storyLength = Convert.ToInt32(new System.IO.FileInfo(textBoxStoryMOT.Text).Length);


            //    int toChangeLength = Convert.ToInt32(new System.IO.FileInfo(filePath).Length);



            //    byte[] data = new byte[toChangeLength];


            //    fs.Read(data, 0, toChangeLength);





            //    int currentOffset = HexToInt("44");
            //    string bottomStart = "";



            //    fileStart = "3C";


            //    for (int i = 0; i < 4; i++)
            //    {

            //        //todo change the value this reads to reflect new size
            //        originalMiddleOffset.Text += (IntToHex(data[currentOffset + 3 - i])).PadLeft(2, '0');
            //        //bottomStart += (IntToHex(data[currentOffset + 3 - i])).PadLeft(2, '0');

            //    }

            //    modifiedMiddleOffset = IntToHex(HexToInt(originalMiddleOffset.Text) + pointerCode.Text.Length);

            //    isScriptCharacterFile.Checked = false;

            //    originalSecondPartStart.Text = IntToHex(originalLength);
            //    modifiedSecondPartStart = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.Length);





            //    fs.Close();






            //}




            ////if (originalMiddleOffset.Text == "")
            ////{
            //for (int rowIndex = 1; rowIndex < rowCount; rowIndex++)
            //{

            //    //for (int colIndex = 0; colIndex < colCount; colIndex++)
            //    //{

            //    if (modifiedPath.Text.Contains(fileList[rowIndex, 1]) && fileList[rowIndex, 1] != "")
            //    //if (fileList[rowIndex, colIndex] == elem)
            //    {
            //        fileListLocation = new int[] { rowIndex, 1 };
            //        originalMiddleOffset.Text = fileList[fileListLocation[0], 2];
            //        modifiedMiddleOffset = IntToHex(HexToInt(originalMiddleOffset.Text) + pointerCode.Text.Length);
            //        //label5.Text = fileList[fileListLocation[0], 1] + fileList[fileListLocation[0], 2];
            //        if (fileList[fileListLocation[0], 0] == "2")
            //        {
            //            if (!isScriptCharacterFile.Checked)
            //            {

            //                isScriptCharacterFile.Checked = true;

            //            }


            //            originalSecondPartStart.Text = fileList[fileListLocation[0], 3];
            //            modifiedSecondPartStart = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.Length);

            //            originalMiddleOffset2.Text = fileList[fileListLocation[0], 4];
            //            modifiedMiddleOffset2 = IntToHex(HexToInt(originalMiddleOffset2.Text) + pointerCode.Text.Length);
            //            //modifiedMiddleOffset2 = IntToHex(HexToInt(originalMiddleOffset2.Text) + pointerCode.Text.Length);
            //        }
            //        else if (fileList[fileListLocation[0], 0] == "1")
            //        {
            //            isScriptCharacterFile.Checked = false;

            //            originalSecondPartStart.Text = IntToHex(originalLength);
            //            modifiedSecondPartStart = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.Length);
            //        }


            //        break;
            //    }




            //}








            //}
        }


        private void patchPath_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void patchPath_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {






















            //todo change this to work with patch file format (which is?????)
            //todo make errors for patch stuff

            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            //patchPath.Text = s[0];
            string patchString = System.IO.File.ReadAllText(s[0]);
            Regex.Replace(patchString, @"\s+", "");
            patchString = patchString.Replace("\n", "").Replace("\r", "");
            patchPath.Text = patchString;
            patchArray = patchString.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(',')).ToArray();
            //patchPath.Text = patchArray[0][0] + " " + patchArray[1][0] +" "+ patchArray[2][1];
            //patchArray = System.IO.File.ReadAllText(s[0]);

            //patchPath.Text = System.IO.File.ReadAllText(s[0]);
            //patchPath.Text = s[0];
            //int i;
            //for (i = 0; i < s.Length; i++)
            //{
            //    //textBox4.Items.Clear();
            //    modifiedPath.Text = (s[i]);
            //    //modifiedPath = s[i];

            //    //todo change to work with bytes. remove *2 here
            //    originalLength = Convert.ToInt32(new System.IO.FileInfo(modifiedPath.Text).Length) * 2;








            //    int rowCount = fileList.GetLength(0);

            //    int colCount = fileList.GetLength(1);
            //    //label5.Text = fileList[5, 1] + rowCount + colCount + "00";


            //    //if (originalMiddleOffset.Text == "")
            //    //{
            //    for (int rowIndex = 1; rowIndex < rowCount; rowIndex++)
            //    {

            //        //for (int colIndex = 0; colIndex < colCount; colIndex++)
            //        //{

            //        if (modifiedPath.Text.Contains(fileList[rowIndex, 1]) && fileList[rowIndex, 1] != "")
            //        //if (fileList[rowIndex, colIndex] == elem)
            //        {
            //            fileListLocation = new int[] { rowIndex, 1 };
            //            originalMiddleOffset.Text = fileList[fileListLocation[0], 2];
            //            modifiedMiddleOffset = IntToHex(HexToInt(originalMiddleOffset.Text) + pointerCode.Text.Length);
            //            //label5.Text = fileList[fileListLocation[0], 1] + fileList[fileListLocation[0], 2];
            //            if (fileList[fileListLocation[0], 0] == "2")
            //            {
            //                if (!isScriptCharacterFile.Checked)
            //                {

            //                    isScriptCharacterFile.Checked = true;

            //                }


            //                originalSecondPartStart.Text = fileList[fileListLocation[0], 3];
            //                modifiedSecondPartStart = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.Length);

            //                originalMiddleOffset2.Text = fileList[fileListLocation[0], 4];
            //                modifiedMiddleOffset2 = IntToHex(HexToInt(originalMiddleOffset2.Text) + pointerCode.Text.Length);
            //            }
            //            else if (fileList[fileListLocation[0], 0] == "1")
            //            {
            //                isScriptCharacterFile.Checked = false;

            //                originalSecondPartStart.Text = IntToHex(originalLength);
            //                modifiedSecondPartStart = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.Length);
            //            }


            //            break;
            //        }
            //        //}
            //    }
            //    //}








            //}





















        }




        private void GetFileInfo(string filePath)
        {

            //todo next make all this stuff run when button pressed
            //string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            //int i;
            //modifiedPath.Text = (s[0]);


            //todo change text box for every file, just use filePath
            modifiedPath.Text = filePath;



            //filePath = modifiedPath.Text;






            //for (i = 0; i < s.Length; i++)
            //{
            //textBox4.Items.Clear();
            //modifiedPath.Text = (s[i].Length.ToString());
            //modifiedPath = s[i];

            //todo change to work with bytes. remove *2 here
            originalLength = Convert.ToInt32(new System.IO.FileInfo(filePath).Length);
            //originalLength = Convert.ToInt32(new System.IO.FileInfo(modifiedPath.Text).Length);








            int rowCount = fileList.GetLength(0);

            int colCount = fileList.GetLength(1);
            //label5.Text = fileList[5, 1] + rowCount + colCount + "00";



            //todo TEST THIS. check here if not working
            //todo make case for every file type???????
            if (modifiedPath.Text.Contains(".mot") || modifiedPath.Text.Contains(".bin") || modifiedPath.Text.Contains(".snp")
                || modifiedPath.Text.Contains(".spb"))
            {



                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                //int storyLength = Convert.ToInt32(new System.IO.FileInfo(textBoxStoryMOT.Text).Length);


                int toChangeLength = Convert.ToInt32(new System.IO.FileInfo(filePath).Length);



                byte[] data = new byte[toChangeLength];


                fs.Read(data, 0, toChangeLength);

                int currentOffset = HexToInt("8");
                for (int i = 0; i < 4; i++)
                {

                    //todo change the value this reads to reflect new size
                    entryTotal += (IntToHex(data[currentOffset + 3 - i])).PadLeft(2, '0');
                    //originalMiddleOffset.Text += (IntToHex(data[currentOffset + 3 - i])).PadLeft(2, '0');
                    //bottomStart += (IntToHex(data[currentOffset + 3 - i])).PadLeft(2, '0');

                    //entryLength.Text = IntToHex((HexToInt(originalMiddleOffset.Text) - HexToInt(fileStart))/);

                }



                currentOffset = 0;
                if (modifiedPath.Text.Contains(".bin")) { currentOffset = HexToInt("18"); }
                else if (modifiedPath.Text.Contains(".mot")) { currentOffset = HexToInt("44"); }
                else if (modifiedPath.Text.Contains(".snp")) { currentOffset = HexToInt("3C"); }
                else if (modifiedPath.Text.Contains(".spb")) { currentOffset = HexToInt("20"); }

                //int currentOffset = HexToInt("44");
                //string bottomStart = "";


                if (modifiedPath.Text.Contains(".bin")) { fileStart = "10"; }
                else if (modifiedPath.Text.Contains(".mot")) { fileStart = "3C"; }
                else if (modifiedPath.Text.Contains(".snp")) { fileStart = "3C"; }
                else if (modifiedPath.Text.Contains(".spb")) { fileStart = "10"; }
                //fileStart = "3C";

                originalMiddleOffset.Text = "";
                //first pointer
                for (int i = 0; i < 4; i++)
                {

                    //todo change the value this reads to reflect new size
                    originalMiddleOffset.Text += (IntToHex(data[currentOffset + 3 - i])).PadLeft(2, '0');
                    //bottomStart += (IntToHex(data[currentOffset + 3 - i])).PadLeft(2, '0');

                    entryLength.Text = IntToHex((HexToInt(originalMiddleOffset.Text) - HexToInt(fileStart))/HexToInt(entryTotal));

                }

                //label3.Text = "offset: " + originalMiddleOffset.Text;


                modifiedMiddleOffset = IntToHex(HexToInt(originalMiddleOffset.Text) + pointerCode.Text.Length / 2);


                //errorLabel.Text += " or:" + originalMiddleOffset.Text + " mod:" + modifiedMiddleOffset + " pointerLength:" + pointerCode.Text.Length;

                isScriptCharacterFile.Checked = false;

                //errorLabel.Text = IntToHex(originalLength);

                originalSecondPartStart.Text = IntToHex(originalLength);
                modifiedSecondPartStart = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.Length);



                fs.Close();






            }

















            //if (originalMiddleOffset.Text == "")
            //{
            //todo is this else good????
            else for (int rowIndex = 1; rowIndex < rowCount; rowIndex++)
                {

                    //for (int colIndex = 0; colIndex < colCount; colIndex++)
                    //{

                    if (modifiedPath.Text.Contains(fileList[rowIndex, 1]) && fileList[rowIndex, 1] != "")
                    //if (fileList[rowIndex, colIndex] == elem)
                    {
                        fileListLocation = new int[] { rowIndex, 1 };
                        originalMiddleOffset.Text = fileList[fileListLocation[0], 2];
                        modifiedMiddleOffset = IntToHex(HexToInt(originalMiddleOffset.Text) + pointerCode.Text.Length);

                        //label5.Text = fileList[fileListLocation[0], 1] + fileList[fileListLocation[0], 2];
                        if (fileList[fileListLocation[0], 0] == "2")
                        {
                            if (!isScriptCharacterFile.Checked)
                            {

                                isScriptCharacterFile.Checked = true;

                            }


                            originalSecondPartStart.Text = fileList[fileListLocation[0], 3];
                            modifiedSecondPartStart = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.Length);

                            originalMiddleOffset2.Text = fileList[fileListLocation[0], 4];
                            modifiedMiddleOffset2 = IntToHex(HexToInt(originalMiddleOffset2.Text) + pointerCode.Text.Length);
                            //modifiedMiddleOffset2 = IntToHex(HexToInt(originalMiddleOffset2.Text) + pointerCode.Text.Length);
                        }
                        else if (fileList[fileListLocation[0], 0] == "1")
                        {
                            isScriptCharacterFile.Checked = false;

                            originalSecondPartStart.Text = IntToHex(originalLength);
                            modifiedSecondPartStart = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.Length);
                        }


                        break;
                    }




                }


        }



        private void ApplyCode(string filePath)
        //private void button1_Click(object sender, EventArgs e)
        {
            //originalMiddleOffset = originalMiddleOffset.Text;
            //modifiedMiddleOffset = modifiedMiddleOffset;
            error1.Text = "";
            error2.Text = "";
            error3.Text = "";
            error4.Text = "";
            label10.Text = "";
            label11.Text = "";

            pointerCode.Text = Regex.Replace(pointerCode.Text, @"\s+", "");
            regularCode.Text = Regex.Replace(regularCode.Text, @"\s+", "");
            //todo next automatically change frame count of bottom code being inserted to that of mot file injected into

            pointerCode2.Text = Regex.Replace(pointerCode2.Text, @"\s+", "");
            regularCode2.Text = Regex.Replace(regularCode2.Text, @"\s+", "");

            //if (originalPath.Text == "")
            //{
            //    error1.Text = "Enter path for original file";

            //}




            if (modifiedPath.Text == "" && filePath == "")
            {
                error3.Text = "Enter path for modified file";

            }
            //if (originalMiddleOffset.Text == "")
            //{
            //    error2.Text = "Enter original file middle offset";
            //}
            //if (modifiedMiddleOffset == "")
            //{
            //    error4.Text = "Enter modified file middle offset";
            //}
            //if (originalPath.Text == modifiedPath.Text)
            //{
            //    error1.Text = "original file and modified file path must be different";
            //}
            //if (originalMiddleOffset.Text == modifiedMiddleOffset && originalMiddleOffset.Text != "")
            //{
            //    error2.Text = "Middle offset for original and modified file must be different";
            //}
            //if (!File.Exists(originalPath.Text))
            //{
            //    error1.Text = "Enter valid file path";
            //}
            if (!File.Exists(modifiedPath.Text) && !File.Exists(filePath))
            {
                error3.Text = "Enter valid file path";
            }


            int rowCount = fileList.GetLength(0);

            int colCount = fileList.GetLength(1);
            //label5.Text = fileList[5, 1] + rowCount + colCount + "00";


            //todo does this need to be nulled?
            if (originalMiddleOffset.Text == "")
            {
                for (int rowIndex = 1; rowIndex < rowCount; rowIndex++)
                {

                    //for (int colIndex = 0; colIndex < colCount; colIndex++)
                    //{

                    if (modifiedPath.Text.Contains(fileList[rowIndex, 1]) && fileList[rowIndex, 1] != "")
                    //if (fileList[rowIndex, colIndex] == elem)
                    {
                        fileListLocation = new int[] { rowIndex, 1 };
                        originalMiddleOffset.Text = fileList[fileListLocation[0], 2];
                        modifiedMiddleOffset = IntToHex(HexToInt(originalMiddleOffset.Text) + pointerCode.Text.Length);
                        //label5.Text = fileList[fileListLocation[0], 1] + fileList[fileListLocation[0], 2];
                        //error1.Text = originalMiddleOffset.Text + " " + modifiedMiddleOffset + " " + pointerCode.Text.Length.ToString();

                        break;
                    }
                    //}
                }
            }

            if (originalMiddleOffsetReadOnly == "")
            {
                originalMiddleOffsetReadOnly = originalMiddleOffset.Text;
            }

            if (regularCode.Text != "" && originalMiddleOffset.Text == "")
            {
                error2.Text = "Enter middle offset, or remove new bottom code";
            }


            if (fileListLocation == new int[] { 0, 0 })
            {
                error4.Text = "Enter usable file, or modified offset";
            }

            if (howManyInOneAdd.Text == "")
            {
                error5.Text = "Enter number of entries added";
              
            }

            if (pointerCode.Text == "" && pointerCode2.Text == "")
            {
                label10.Text = "enter pointer code";
            }
            //if (pointerCode.Text == "" || pointerCode2.Text == "")
            //{
            //    label10.Text = "enter pointer code";
            //}

            //if (regularCode.Text == "")
            //{
            //    label11.Text = "enter regular code";
            //}

            if (pointerCode.Text.Length % 8 != 0)
            {
                label10.Text = "code length must be multiple of 8";
            }


            //for (int i = 0; i<fileList.GetLength(0); i++)
            //{

            //}
            //if (!fileList modifiedPath.Text
            //    && modifiedMiddleOffset == "")
            //{
            //    error4.Text = "Enter usable file, or modified offset";
            //}



            if (isScriptCharacterFile.Checked)
            {


























                pointerCode2.Text = Regex.Replace(pointerCode2.Text, @"\s+", "");




                //int rowCount2 = fileList.GetLength(0);

                //int colCount2 = fileList.GetLength(1);
                //label5.Text = fileList[5, 1] + rowCount + colCount + "00";



                //if (originalMiddleOffset2.Text == "")
                //{




                for (int rowIndex = 1; rowIndex < rowCount; rowIndex++)
                {



                    if (modifiedPath.Text.Contains(fileList[rowIndex, 1]) && fileList[rowIndex, 1] != "")
                    //if (fileList[rowIndex, colIndex] == elem)
                    {
                        fileListLocation = new int[] { rowIndex, 1 };

                        if (originalMiddleOffset.Text == "")
                        {
                            originalMiddleOffset.Text = fileList[fileListLocation[0], 2];
                        }
                        modifiedMiddleOffset = IntToHex(HexToInt(originalMiddleOffset.Text) + pointerCode.Text.Length);
                        //label5.Text = fileList[fileListLocation[0], 1] + fileList[fileListLocation[0], 2];
                        if (fileList[fileListLocation[0], 0] == "2")
                        {
                            if (originalSecondPartStart.Text == "")
                            {
                                originalSecondPartStart.Text = fileList[fileListLocation[0], 3];
                            }
                            modifiedSecondPartStart = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.Length);

                            if (originalMiddleOffset2.Text == "")
                            {
                                originalMiddleOffset2.Text = fileList[fileListLocation[0], 4];
                            }
                            //todo fix this??
                            modifiedMiddleOffset2 = IntToHex(HexToInt(originalMiddleOffset2.Text) + pointerCode.Text.Length);
                        }


                        break;
                    }



                    //if (modifiedPath.Text.Contains(fileList[rowIndex, 1]) && fileList[rowIndex, 1] != "")
                    //if (fileList[rowIndex, colIndex] == elem)
                    //{
                    //fileListLocation = new int[] { rowIndex, 1 };
                    //secondPartStart = fileList[fileListLocation[0], 3];
                    //originalMiddleOffset2.Text = fileList[fileListLocation[0], 4];
                    //modifiedMiddleOffset2 = IntToHex(HexToInt(originalMiddleOffset.Text) + pointerCode.Text.Length);
                    //label5.Text = fileList[fileListLocation[0], 1] + fileList[fileListLocation[0], 2];


                    //break;
                    //}
                    //}
                }
                //}




                if (originalSecondPartStart.Text == "")
                {
                    error2.Text = "enter start of second part";
                }

                if (regularCode2.Text != "" && originalMiddleOffset2.Text == "")
                {
                    error2.Text = "Enter middle offset 2, or remove new bottom code 2";
                }



                //if (pointerCode2.Text == "")
                //{

                //    label10.Text = "enter pointer code 2";
                //}



                if (pointerCode2.Text.Length % 8 != 0)
                {
                    label10.Text = "code length must be multiple of 8";
                }























            }


            //label17.Text = "before errors";
            if (error1.Text.Length + error2.Text.Length + error3.Text.Length + error4.Text.Length + label10.Text.Length + label11.Text.Length == 0)
            //if (originalPath.Text != "" &&
            //    modifiedPath.Text != "" &&
            //    originalMiddleOffset.Text != "" &&
            //    modifiedMiddleOffset != ""
            //    && originalPath.Text != modifiedPath.Text
            //    && originalMiddleOffset.Text != modifiedMiddleOffset
            //    )
            {
                label17.Text = "past errors";
                //if (!File.Exists(originalPath.Text))
                //{

                //}
                //modifiedLength = new System.IO.FileInfo(modifiedPath.Text).Length * 2;

                //originalMiddleOffset.Text = "740";
                //modifiedMiddleOffset = "774";




                //hex to int
                int originalInt = Int32.Parse(originalMiddleOffset.Text, System.Globalization.NumberStyles.HexNumber);

                error1.Text = modifiedMiddleOffset;
                //error1.Text = originalMiddleOffset.Text + " " + originalInt.ToString();

                int modifiedInt = Int32.Parse(modifiedMiddleOffset, System.Globalization.NumberStyles.HexNumber);

                //int to hex
                string originalHex = originalInt.ToString("X");
                string modifiedHex = modifiedInt.ToString("X");


                //originalInt = ReverseEndianess(originalInt);
                //modifiedInt = ReverseEndianess(modifiedInt);

                originalHex = ReverseEndianess(originalHex);
                modifiedHex = ReverseEndianess(modifiedHex);

                //originalHex = originalInt.ToString("X");
                //modifiedHex = modifiedInt.ToString("X");

                //originalInt = Int32.Parse(originalHex.ToString(), System.Globalization.NumberStyles.HexNumber);


                //label3.Text = originalInt.ToString() + " " + originalHex;
                //label4.Text = modifiedInt.ToString() + " " + modifiedHex;

                //todo add parameters for this. OR change values with function
                //newFileCode = InsertNewCode(pointerCode, originalMiddleOffset, regularCode);

                //TestByteStuff();
                //label17.Text += " "+IntToHex(originalLength);
                List<byte> dataList = new List<byte>();
                //label17.Text +=" bb"+ dataList.Count.ToString();
                label17.Text = "insertNewCode NOT Done";
                dataList = InsertNewCode();
                label17.Text = "insertNewCode Done";
                //newFileCode = InsertNewCode();
                //label17.Text +=" bb"+ dataList.Count.ToString();


                //label5.Text = newFileCode[0].ToString() + newFileCode[1] + newFileCode[2];

                //todo make this called in InsertNewCode. OR change values with function



                ReadEightCharacters(dataList);
                label17.Text = "Success";
                //label17.Text = "Success";




                //todo don't change original values? insert all new code at same place?
                originalMiddleOffset.Text = modifiedMiddleOffset;
                originalSecondPartStart.Text = modifiedSecondPartStart;
                originalMiddleOffset2.Text = modifiedMiddleOffset2;

                error1.Text = originalMiddleOffset.Text + " " + modifiedMiddleOffset + " " + pointerCode.Text.Length.ToString();

                //label5.Text += "z";

                //label5.Text = ReadFourBytes();

                //ReadFourBytes();


                //if (currentOffset%4==0)







                //FileStream fs = new FileStream(modifiedPath.Text, FileMode.Open, FileAccess.ReadWrite);

                ////byte[] data = new byte[HexToInt(originalMiddleOffset.Text)];
                ////fs.Read(data, 0, HexToInt(originalMiddleOffset.Text));

                ////label17.Text = dataList.Count.ToString();
                ////label17.Text +=" bb"+ dataList.Count.ToString();
                ////error1.Text = cumulativeIncrease.ToString();
                //for (int i = 0; i < dataList.Count / 2 + cumulativeIncrease/2; i++)
                //{
                //    fs.Position = i;
                //    fs.WriteByte(dataList[i]);

                //    /*
                //    //byte newByte = byte.Parse((newFileCode[i].ToString() + newFileCode[i + 1].ToString()));
                //    byte newByte = byte.Parse((HexToInt(newFileCode[2 * i].ToString() + newFileCode[2 * i + 1].ToString())).ToString());
                //    //byte newByte = byte.Parse((HexToInt(newFileCode[i].ToString() )).ToString());
                //    //label5.Text += newFileCode[i].ToString() + newFileCode[i + 1].ToString() ;
                //    //label5.Text += (HexToInt(newFileCode[i].ToString() + newFileCode[i + 1].ToString())).ToString();

                //    fs.Position = i;
                //    fs.WriteByte(newByte);
                //    */

                //}
                ////fs.WriteByte(3);
                ////label5.Text = newFileCode;
                //fs.Close();













            }




















































            //if (modifiedPath.Text.Contains(".mot"))
            //{



            //    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);


            //    int toChangeLength = Convert.ToInt32(new System.IO.FileInfo(filePath).Length);


            //    byte[] data = new byte[toChangeLength];


            //    fs.Read(data, 0, toChangeLength);





            //    int currentOffset = HexToInt("34");
            //    string bottomStart = "";



            //    fileStart = "3C";


            //    for (int i = 0; i < 4; i++)
            //    {

            //        //todo change the value this reads to reflect new size
            //        originalMiddleOffset.Text += (IntToHex(data[currentOffset + 3 - i])).PadLeft(2, '0');
            //        //bottomStart += (IntToHex(data[currentOffset + 3 - i])).PadLeft(2, '0');

            //    }

            //    modifiedMiddleOffset = IntToHex(HexToInt(originalMiddleOffset.Text) + pointerCode.Text.Length);

            //    isScriptCharacterFile.Checked = false;

            //    originalSecondPartStart.Text = IntToHex(originalLength);
            //    modifiedSecondPartStart = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.Length);





            //    fs.Close();

















        }

        private void TestByteStuff()
        {
            char[] cArray = pointerCode.Text.ToCharArray();
            byte[] pointerCodeBytes = new byte[cArray.Length];

            for (int i = 0; i < cArray.Length; i++)
            {
                pointerCodeBytes[i] = Convert.ToByte(HexToInt(cArray[i].ToString()));
            }


            //byte[] pointerCodeBytes = cArray.Where(letter => HexToInt(letter.ToString()));

            //byte[] bytes = Encoding.ANSI.GetBytes(someString);
            //byte[] pointerCodeBytes = Encoding.ASCII.GetBytes( pointerCode.Text);
            modifiedPath.Text = pointerCodeBytes[0] + " " + IntToHex(pointerCodeBytes[0]);

            /*
            FileStream fs = new FileStream(modifiedPath.Text, FileMode.Open, FileAccess.Read);
            //int hexIn = 0;
            //string hex = "";
            byte[] data = new byte[originalLength];
            //byte[] data = new byte[HexToInt(originalMiddleOffset.Text)];
            fs.Read(data, 0, originalLength);
            modifiedPath.Text = data.Length.ToString();
            //modifiedPath.Text = data.Take(3).ToString();
            //modifiedPath.Text = data.Take(3).ToString()+ " " + IntToHex(data[0]);
            //modifiedPath.Text = data[0].ToString()+ " " + IntToHex(data[0]);
            */
        }

        private void ClearOldValues()
        {
            originalMiddleOffset.Text = "";
            entryTotal = "0";
        }

        private void ClearErrors()
        {
            error1.Text = "";
            error2.Text = "";
            error3.Text = "";
            error4.Text = "";
            label10.Text = "";
            label11.Text = "";

            error0.Text = "";
            error5.Text = "";

        }

        private string ReverseEndianess(string str)
        {
            //int returner = BitConverter.ToInt32(number.ToString().Take(4).Reverse().ToArray(), 0);

            int number = Int32.Parse(str, System.Globalization.NumberStyles.HexNumber);

            byte[] bytes = BitConverter.GetBytes(number);
            Array.Reverse(bytes, 0, bytes.Length);

            int result = BitConverter.ToInt32(bytes, 0);

            return result.ToString("X");
        }

        private List<byte> InsertNewCode()
        //private string InsertNewCode()
        {
            newFileCode = "";
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //FileStream fs = new FileStream(modifiedPath.Text, FileMode.Open, FileAccess.Read);
            //int hexIn = 0;
            //string hex = "";

            byte[] dataArray = new byte[originalLength];

            byte[] data = new byte[originalLength];

            //byte[] data = new byte[HexToInt(originalMiddleOffset.Text)];



            fs.Read(dataArray, 0, originalLength);

            List<byte> dataList = dataArray.ToList<byte>();


            //List<byte> dataList = new List<byte>(dataArray);

            //label17.Text =" "+ dataList.Count.ToString();
            //label17.Text =" "+ dataList.Count.ToString();
            //var logFile = File.ReadAllLines(LOG_PATH);
            //var logList = new List<string>(logFile);


            //fs.Read(data, 0, Convert.ToInt32(originalLength));
            //fs.Read(data, 0, HexToInt(originalMiddleOffset.Text));
            string newByte = "";

            bool alreadyInjected = false;
            //for (int i = 0; i < 10; i++)
            cumulativeIncrease = 0;

            //for (int i = HexToInt(fileStart); i < HexToInt(originalMiddleOffset.Text); i++)
            if (true)
            {

                //label17.Text += dataList.Count.ToString();

                char[] pointerArray = pointerCode.Text.ToCharArray();
                byte[] pointerCodeBytes = new byte[pointerArray.Length / 2];

                for (int i = 0; i < pointerCodeBytes.Length; i++)
                {
                    //pointerCodeBytes[i] = (byte) ( HexToInt(pointerArray[2*i+1].ToString()) + HexToInt(pointerArray[2*i].ToString()));
                    pointerCodeBytes[i] = Convert.ToByte(HexToInt(pointerArray[2 * i].ToString() + pointerArray[2 * i + 1].ToString()));
                    //pointerCodeBytes[i] = Convert.ToByte(HexToInt(pointerArray[i].ToString()));
                }



                //for (int i = 0; i< pointerCodeBytes.Length ; i++)
                //{
                //    dataList.Insert(HexToInt(originalMiddleOffset.Text), pointerCodeBytes[i]);
                //}

                for (int i = pointerCodeBytes.Length - 1; i >= 0; i--)
                {
                    //todo22 find out why this deletes at the end half the values it adds
                    dataList.Insert(HexToInt(originalMiddleOffset.Text), pointerCodeBytes[i]);
                    //label17.Text +=i+" ";
                }
                //label17.Text +=" bb"+ dataList.Count.ToString();
                cumulativeIncrease += pointerCodeBytes.Length;
                modifiedMiddleOffset = IntToHex(HexToInt(originalMiddleOffset.Text) + cumulativeIncrease);
                //dataList.Insert(HexToInt(originalMiddleOffset.Text), pointerCodeBytes);

                //byte[] pointerCodeBytes = Encoding.ASCII.GetBytes(pointerCode.Text);

                //dataList.Insert(HexToInt(originalMiddleOffset.Text), (pointerCode.Text));


                //byte[] regularCodeBytes = Encoding.ASCII.GetBytes(regularCode.Text);

                //dataList.Insert(HexToInt(originalMiddleOffset.Text), (regularCode.Text));
                //label17.Text += dataList.Count.ToString();
            }
            else
            {
                for (int i = 0; i < HexToInt(originalMiddleOffset.Text); i++)
                {

                    //if (injectLocation.Text == IntToHex(i))
                    //{
                    //    alreadyInjected = true;
                    //    newFileCode += pointerCode.Text;
                    //}

                    //label17.Text = i.ToString();
                    //label5.Text = IntToHex(i)+injectLocation.Text + " " + label5.Text;

                    fs.Position = i;

                    newByte += (IntToHex(data[i])).PadLeft(2, '0');
                    //string newByte = (IntToHex(data[i])).PadLeft(2, '0');

                    //string newByte = (   (data[i ]).ToString() );
                    //string newByte = (   IntToHex(data[i * 2]) + IntToHex(data[i * 2+1]));
                    //string newByte = (  data[i * 2].ToString() + data[i * 2+1].ToString());
                    //string newByte = (IntToHex( data[i * 2])+ "www" +  data[i * 2].ToString());
                    //string newByte = ((HexToInt(data[i * 2].ToString() + data[i * 2 + 1].ToString())).ToString());


                    //fs.WriteByte(newByte);


                    //newFileCode += System.Text.Encoding.UTF8.GetString(newByte);

                    //label5.Text = newByte + "w" + label5.Text;

                    //label5.Text = newFileCode;
                    //if (i +9 >= HexToInt(originalMiddleOffset.Text)/2 )
                    //{
                    //    label5.Text += newByte+ " ";
                    //}

                    //if (alreadyInjected)


                    //newFileCode += newByte;
                }
            }
            //newFileCode += newByte;
            //if (alreadyInjected == false)
            //{
            //    newFileCode += pointerCode.Text;

            //}
            //label17.Text +=" "+ dataList.Count.ToString();
            alreadyInjected = false;

            //if (injectLocation.Text == "")
            //{
            //    newFileCode += pointerCode.Text;

            //}


            //modifiedMiddleOffset = IntToHex(newFileCode.Length / 2);
            //modifiedMiddleOffset = IntToHex(Convert.ToInt32(newFileCode.Length) / 2);

            //for (int i = HexToInt(originalMiddleOffset.Text); i < HexToInt(originalMiddleOffset.Text)+6; i++)

            //long secondGroupEnd = 0;
            //if (isScriptCharacterFile.Checked)
            //{

            //}
            newByte = "";
            if (true)
            {
                //errorLabel.Text = originalSecondPartStart.Text;

                //label17.Text = regularCode.Text.ToString();
                //label17.Text = regularCodeBytes.Length.ToString();
                char[] regularCodeArray = regularCode.Text.ToCharArray();
                byte[] regularCodeBytes = new byte[regularCodeArray.Length / 2];

                for (int i = 0; i < regularCodeBytes.Length; i++)
                {
                    regularCodeBytes[i] = Convert.ToByte(HexToInt(regularCodeArray[2 * i].ToString() + regularCodeArray[2 * i + 1].ToString()));
                    //regularCodeBytes[i] = Convert.ToByte(HexToInt(regularCodeArray[i].ToString()));

                    //errorLabel.Text += IntToHex( regularCodeBytes[i]);
                }

                //todo TEST THIS. check this if not working
                //for (int i= 0; i<12; i++)
                //{
                //    dataList.RemoveAt(dataList.Count - 1);
                //}

                for (int i = regularCodeBytes.Length - 1; i >= 0; i--)
                {
                    //todo TEST THIS. GOES WITH UPPER THING
                    //dataList.Insert(HexToInt(originalSecondPartStart.Text) + cumulativeIncrease -12, regularCodeBytes[i]);

                    dataList.Insert(HexToInt(originalSecondPartStart.Text) + cumulativeIncrease, regularCodeBytes[i]);

                }


                cumulativeIncrease += regularCodeBytes.Length;
                modifiedSecondPartStart = IntToHex(HexToInt(originalSecondPartStart.Text) + cumulativeIncrease);

            }
            else
            {
                for (int i = HexToInt(originalMiddleOffset.Text); i < HexToInt(originalSecondPartStart.Text); i++)
                //for (int i = HexToInt(originalMiddleOffset.Text); i < originalLength / 2; i++)
                {
                    fs.Position = i;

                    newByte += (IntToHex(data[i])).PadLeft(2, '0');
                    //newByte = (IntToHex(data[i])).PadLeft(2, '0');

                    //string newByte = (   (data[i ]).ToString() );
                    //string newByte = (   IntToHex(data[i * 2]) + IntToHex(data[i * 2+1]));
                    //string newByte = (  data[i * 2].ToString() + data[i * 2+1].ToString());
                    //string newByte = (IntToHex( data[i * 2])+ "www" +  data[i * 2].ToString());
                    //string newByte = ((HexToInt(data[i * 2].ToString() + data[i * 2 + 1].ToString())).ToString());


                    //fs.WriteByte(newByte);


                    //newFileCode += System.Text.Encoding.UTF8.GetString(newByte);


                    //label5.Text =   label5.Text+ "w"  + newByte;
                    //label5.Text = newByte + "w" + label5.Text;
                    //label5.Text = newFileCode;
                    //if (i +9 >= HexToInt(originalMiddleOffset.Text)/2 )
                    //{
                    //    label5.Text += newByte+ " ";
                    //}

                    //newFileCode += newByte;
                }

                newFileCode += newByte;
                newByte = "";
                newFileCode += regularCode.Text;
            }


            if (isScriptCharacterFile.Checked)
            {



                if (true)
                {

                    char[] pointerArray2 = pointerCode2.Text.ToCharArray();
                    byte[] pointerCodeBytes2 = new byte[pointerArray2.Length / 2];

                    for (int i = 0; i < pointerCodeBytes2.Length; i++)
                    {
                        pointerCodeBytes2[i] = Convert.ToByte(HexToInt(pointerArray2[2 * i].ToString() + pointerArray2[2 * i + 1].ToString()));
                        //pointerCodeBytes2[i] = Convert.ToByte(HexToInt(pointerArray2[i].ToString()));
                    }


                    for (int i = pointerCodeBytes2.Length - 1; i >= 0; i--)
                    {
                        dataList.Insert(HexToInt(originalMiddleOffset2.Text) + cumulativeIncrease, pointerCodeBytes2[i]);
                    }
                    cumulativeIncrease += pointerCodeBytes2.Length;
                    modifiedMiddleOffset2 = IntToHex(HexToInt(originalMiddleOffset2.Text) + cumulativeIncrease);

                }
                else
                {
                    //for (int i = 0; i < 10; i++)
                    for (int i = HexToInt(originalSecondPartStart.Text); i < HexToInt(originalMiddleOffset2.Text); i++)
                    {
                        //label5.Text = IntToHex(i)+injectLocation.Text + " " + label5.Text;
                        if (injectLocation2.Text == IntToHex(i))
                        {
                            alreadyInjected = true;
                            newFileCode += pointerCode2.Text;
                        }

                        fs.Position = i;
                        //string newByte = (   (data[i ]).ToString() );
                        string newByte2 = (IntToHex(data[i])).PadLeft(2, '0');


                        newFileCode += newByte2;
                    }
                    if (alreadyInjected == false)
                    {
                        newFileCode += pointerCode2.Text;

                    }

                    alreadyInjected = false;


                    //modifiedMiddleOffset2 = IntToHex(newFileCode.Length / 2);
                }

                if (true)
                {


                    char[] regularCodeArray2 = regularCode2.Text.ToCharArray();
                    byte[] regularCodeBytes2 = new byte[regularCodeArray2.Length / 2];

                    for (int i = 0; i < regularCodeBytes2.Length; i++)
                    {

                        regularCodeBytes2[i] = Convert.ToByte(HexToInt(regularCodeArray2[2 * i].ToString() + regularCodeArray2[2 * i + 1].ToString()));
                        //regularCodeBytes2[i] = Convert.ToByte(HexToInt(regularCodeArray2[i].ToString()));
                    }


                    //for (int i = 0; i< regularCodeBytes2.Length;  i++)
                    for (int i = regularCodeBytes2.Length - 1; i >= 0; i--)
                    {
                        //error1.Text += regularCodeBytes2[i];
                        //dataList.Add(regularCodeBytes2[i]);
                        //error1.Text = IntToHex((originalLength) + cumulativeIncrease);



                        //first one that works
                        //dataList.Insert(HexToInt(originalSecondPartStart.Text) + cumulativeIncrease, regularCodeBytes[i]);
                        //a
                        dataList.Insert((originalLength) + cumulativeIncrease, regularCodeBytes2[i]);
                        //dataList.Insert((originalLength) / 2 + cumulativeIncrease, regularCodeBytes2[i]);





                        //dataList.Insert(HexToInt(originalLength)+cumulativeIncrease, regularCodeBytes2[i]);
                    }
                    cumulativeIncrease += regularCodeBytes2.Length;


                }
                else
                {

                    for (int i = HexToInt(originalMiddleOffset2.Text); i < originalLength; i++)
                    //for (int i = HexToInt(originalMiddleOffset2.Text); i < originalLength / 2; i++)
                    //for (int i = HexToInt(originalMiddleOffset.Text); i < originalLength / 2; i++)
                    {
                        fs.Position = i;
                        //string newByte = (   (data[i ]).ToString() );
                        string newByte2 = (IntToHex(data[i])).PadLeft(2, '0');


                        newFileCode += newByte2;
                    }
                    newFileCode += regularCode2.Text;
                }



























            }



            //label5.Text = newFileCode;
            //label5.Text = modifiedMiddleOffset;

            //label5.Text = IntToHex( newFileCode.Length/2);
            //label5.Text += " " + IntToHex(Convert.ToInt32( originalLength/2));
            //label5.Text = data.ToString();


            modifiedLength = originalLength + cumulativeIncrease;
            //modifiedLength = originalLength / 2 + cumulativeIncrease;
            //errorLabel.Text = IntToHex( originalLength) + " " + IntToHex( modifiedLength);
            //modifiedLength = newFileCode.Length;
            fs.Close();
            //label17.Text += " a" + dataList.Count.ToString();

            //errorLabel.Text = IntToHex( dataList[dataList.Count-2]);

            return dataList;
            //return newFileCode;
        }

        private void ReadEightCharacters(List<byte> dataList)
        {
            string result = "";

            //errorLabel.Text = IntToHex( dataList[dataList.Count-2])+ "asdf";
            //FileStream fs = new FileStream(modifiedPath.Text, FileMode.Open, FileAccess.ReadWrite);

            //byte[] data = new byte[HexToInt(originalMiddleOffset.Text)];
            //fs.Read(data, 0, HexToInt(originalMiddleOffset.Text));

            string bigGroup = "";
            string littleGroup = "";
            string regularCodeOffset = "";


            //label5.Text += "aa "+originalMiddleOffset.Text.ToString();

            //not here
            //if (true) { } else
            for (int i = HexToInt(fileStart) / 4; i < HexToInt(modifiedMiddleOffset) / 4; i++)
            //for (int i = 2 * HexToInt(fileStart) / 8; i < 2 * HexToInt(modifiedMiddleOffset) / 8; i++)
            //for (int i = 0; i < 2 * HexToInt(modifiedMiddleOffset) / 8; i++)
            {


                bigGroup = "";
                littleGroup = "";
                for (int j = 3; j >= 0; j--)
                {

                    //button1.Text = bigGroup.ToString();
                    //swapped to big endian

                    //string toAdd = IntToHex(data[i * 4 + j]).ToString().PadLeft(2, '0');
                    //if (toAdd.Length < 2) { toAdd = "0" + toAdd; }
                    //bigGroup += toAdd;
                    //label5.Text += toAdd + " ";



                    //bigGroup += IntToHex(dataList[i * 4 + j] + dataList[i * 4 + j+1]);
                    bigGroup += IntToHex(dataList[i * 4 + j]).PadLeft(2, '0');
                    //bigGroup += newFileCode[i * 8 + 2 * j].ToString() + newFileCode[i * 8 + 2 * j + 1].ToString();
                }

                //label5.Text = bigGroup + IntToHex( i*8)+ " "  + label5.Text;
                //label5.Text += bigGroup + "a";

                for (int j = 0; j < 4; j++)
                {
                    //string toAdd = IntToHex(data[i * 4 + j]).ToString().PadLeft(2, '0');
                    //if (toAdd.Length < 2) { toAdd = "0" + toAdd; }
                    //littleGroup += toAdd;


                    //swapped to big endian
                    //littleGroup += IntToHex(data[i * 4 + j]).ToString();

                    littleGroup += IntToHex(dataList[i * 4 + j]).PadLeft(2, '0');
                    //littleGroup += newFileCode[i * 8 + j];
                }




                //label5.Text = littleGroup + IntToHex(i * 8 / 2) + " " + label5.Text;
                //label5.Text = littleGroup+ " " + label5.Text;
                //bigGroup =

                //todo fix all this stuff for character script second part
                //if (HexToInt(bigGroup) >= HexToInt(originalMiddleOffset.Text)
                //    && HexToInt(bigGroup) < HexToInt(originalSecondPartStart.Text)
                //    //&& HexToInt(bigGroup) < originalLength
                //    && (i * 8 / 2) < HexToInt(originalMiddleOffset.Text))
                //{
                //    //label5.Text = (HexToInt(bigGroup) < modifiedLength).ToString();
                //    //label5.Text =  modifiedMiddleOffset +" "+ label5.Text;
                //    //label5.Text = "little: " + littleGroup + " big:" + bigGroup + label5.Text;
                //    //label5.Text += "little: " + littleGroup + " big:" + bigGroup;
                //    //result += "little: " + littleGroup + " big:" + bigGroup;
                //}



                //else if ((HexToInt(bigGroup) > HexToInt(modifiedMiddleOffset)
                //    && HexToInt(bigGroup) < modifiedLength)
                //    && (i * 8 / 2) >= HexToInt(originalMiddleOffset.Text)
                //{
                //    label5.Text = "NEWWWWWlittle: " + littleGroup + " big:" + bigGroup +label5.Text;
                //}



                //if (HexToInt(bigGroup) >= HexToInt(modifiedMiddleOffset)
                //    && HexToInt(bigGroup) < modifiedLength)
                //{
                //    result += "little: "+littleGroup + " big:" + bigGroup;
                //} 


                //errorLabel.Text += bigGroup + " " + errorLabel.Text;

                //if (bigGroup == "00077C90")
                ////if (bigGroup == "907C0700")
                //{

                //    errorLabel.Text = ((bigGroup) != "02000000").ToString() +
                //    //errorLabel.Text = (HexToInt(bigGroup) >= 2).ToString() +
                //    //errorLabel.Text = (HexToInt(bigGroup) >= HexToInt("02000000")).ToString() +
                //    //HexToInt(bigGroup) >= HexToInt(modifiedMiddleOffset)
                //    //&& HexToInt(bigGroup) < HexToInt(modifiedSecondPartStart)


                //    //&& (i * 8 ) >= HexToInt(originalMiddleOffset.Text)
                //    //&& (i * 8 ) < HexToInt(modifiedMiddleOffset)

                //    ((i * 8 / 2) >= HexToInt(originalMiddleOffset.Text)).ToString() +
                //     ((i * 8 / 2) < HexToInt(modifiedMiddleOffset)).ToString();


                //}


                int change = 0;





                if (bigGroup == "00000740")
                {
                    error2.Text = bigGroup + " " + originalMiddleOffsetReadOnly;

                    error2.Text += (!modifiedPath.Text.Contains(".mot")).ToString()
                        + (HexToInt(bigGroup) >= HexToInt(originalMiddleOffset.Text)).ToString()
                        + (HexToInt(bigGroup) < HexToInt(originalSecondPartStart.Text)).ToString()
                        + ((i * 8 / 2) >= HexToInt(originalMiddleOffset.Text)).ToString()
                        + ((i * 8 / 2) < HexToInt(modifiedMiddleOffset)).ToString()
                        + (regularCode.Text != "").ToString() + " ";
                }







                //not here
                //if (true) { } else
                //for changing old pointers
                if (HexToInt(bigGroup) >= HexToInt(originalMiddleOffset.Text)
                    && HexToInt(bigGroup) < HexToInt(originalSecondPartStart.Text) //todo change to originalLength/2 ????
                                                                                   //&& HexToInt(bigGroup) < originalLength //todo change to originalLength/2 ????
                    && (i * 8 / 2) < HexToInt(modifiedMiddleOffset)
                    && !((i * 8 / 2) >= HexToInt(originalMiddleOffset.Text))
                    //&& !((i * 8 / 2) > HexToInt(originalMiddleOffset.Text) && (regularCode.Text != ""))

                    //these are big, actual code is little endian
                    //for catching .ogg
                    && (bigGroup != "00006767")



                    //for catching top code that aren't pointers
                    && (bigGroup != "0050004C")
                    && (bigGroup != "001F0001")
                    && (bigGroup != "001F0000")
                    && (bigGroup != "0010000C")
                    //todo next add all that shouldn't be changed in top code

                    //only for .spb files???
                    && (bigGroup != "00010000")

                    //not here
                    //&& false




                    )

                {



                    //label5.Text = IntToHex(i*8/2) + "_" + (originalMiddleOffset.Text) + label5.Text;
                    //label5.Text = (i*8/2).ToString() + "_" + HexToInt(originalMiddleOffset.Text) + label5.Text;

                    //int      =  ToInt  ( Hex                  ) -    ToInt (Hex      )
                    change = (HexToInt(modifiedMiddleOffset) - HexToInt(originalMiddleOffset.Text));
                    //return change.ToString();
                }

                //todo add box to choose new pointer code location
                //todo mixing old and new pointers???
                //todo cleanup *2 /2 stuff



                //todo check this part if things not working



                //else if (modifiedPath.Text.Contains(".mot") && 
                //    HexToInt(bigGroup) >= HexToInt(fileList[fileListLocation[0], 2])
                //    && HexToInt(bigGroup) < HexToInt(fileList[fileListLocation[0], 3])
                //    //HexToInt(bigGroup) >= HexToInt(modifiedMiddleOffset)
                //    //&& HexToInt(bigGroup) < HexToInt(modifiedSecondPartStart)

                //    && (i * 8 / 2) >= HexToInt(originalMiddleOffset.Text)
                //    && (i * 8 / 2) < HexToInt(modifiedMiddleOffset)
                //    && regularCode.Text != "") {aa }
                //todo add option for custom offset values for new files
                //else if (true) { }


                //BE CAREFUL to use pointers near end, so this code doesn't miss them
                //for matching new bottom code
                else
                if (

                    //HexToInt(bigGroup) >= HexToInt(fileList[fileListLocation[0], 2])
                    //&& HexToInt(bigGroup) < HexToInt(fileList[fileListLocation[0], 3])

                    true &&
                    //DON"T NEED THESE??????? check here if not working


                    (bigGroup != "0050004C") &&


                    //only use this for .mot files?????
                    (bigGroup) != "00000002" &&
                    (bigGroup) != "00000001" &&


                    //todo check this
                    //only use this for .spb files???

                    //00000100
                    (bigGroup) != "00010000" &&
                    (bigGroup) != "00000100" &&


                    //for catching .ogg
                    //bigGroup != "00006767" &&



                    //different for .mot and .snp files???
                    //todo check here if not working. don't need this???
                    //HexToInt(bigGroup) >= HexToInt(originalMiddleOffset.Text)
                    //&& HexToInt(bigGroup) < HexToInt(originalSecondPartStart.Text)

                    (
                    (
                    !modifiedPath.Text.Contains(".mot")
                    //todo next check if not working. replace other instances of originalMiddleOffset.Text with originalMiddleOffsetReadOnly???????
                    && HexToInt(bigGroup) >= HexToInt(originalMiddleOffsetReadOnly)
                    //&& HexToInt(bigGroup) >= HexToInt(originalMiddleOffset.Text)
                    && HexToInt(bigGroup) < HexToInt(originalSecondPartStart.Text)

                    //&& HexToInt(bigGroup) >= HexToInt(modifiedMiddleOffset)
                    //&& HexToInt(bigGroup) < HexToInt(modifiedSecondPartStart)
                    )
                    ||
                    (
                    modifiedPath.Text.Contains(".mot")
                    )
                    )

                    //different for .mot and .snp files???
                    //&& (i * 8 ) >= HexToInt(originalMiddleOffset.Text)
                    //&& (i * 8 ) < HexToInt(modifiedMiddleOffset)
                    && (i * 8 / 2) >= HexToInt(originalMiddleOffset.Text)
                    && (i * 8 / 2) < HexToInt(modifiedMiddleOffset)


                    && regularCode.Text != "")
                {


                    //errorLabel.Text += " " + bigGroup;
                    //errorLabel.Text += "a";
                    //todonext find out why it's going here so much
                    //test += 1;
                    //error1.Text = test.ToString();
                    //error1.Text += "a";
                    //label5.Text += "a";
                    if (regularCodeOffset == "")
                    {
                        //errorLabel.Text += "a";
                        string regularCodeStartOffset = "";



                        if (modifiedPath.Text.Contains(".mot"))
                        {

                            //regularCodeOffset = IntToHex(HexToInt(bigGroup) - (i * 8 / 2));

                            //string regularCodeStartOffset = IntToHex(HexToInt(originalSecondPartStart.Text));

                            //todo figure out why this works
                            //string regularCodeStartOffset = IntToHex(HexToInt(originalSecondPartStart.Text) + );


                            //string regularCodeStartOffset = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.ToString().Length / 2);
                            //string regularCodeStartOffset = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.ToString().Length/2);
                            //todo figure out why this works. check here if not working
                            regularCodeStartOffset = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.ToString().Length / 2 + 4 * 8);
                            //string regularCodeStartOffset = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.ToString().Length + 8);
                            //string regularCodeStartOffset = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.ToString().Length / 2);
                            //string regularCodeStartOffset = IntToHex(originalLength / 2 + pointerCode.Text.ToString().Length);
                            //string regularCodeStartOffset = IntToHex(Convert.ToInt32(originalLength / 2) + pointerCode.Text.ToString().Length);



                        }
                        else
                        {
                            regularCodeStartOffset = IntToHex(HexToInt(originalSecondPartStart.Text) + pointerCode.Text.ToString().Length / 2);
                        }



                        regularCodeOffset = IntToHex(HexToInt(regularCodeStartOffset) - HexToInt(bigGroup));
                        //regularCodeOffset = IntToHex(HexToInt(bigGroup) - HexToInt(regularCodeStartOffset) );

                        //label5.Text = pointerCode.Text[0].ToString() + pointerCode.Text[1].ToString() + pointerCode.Text[2].ToString() +
                        //    pointerCode.Text[3].ToString() + pointerCode.Text[4].ToString() + pointerCode.Text[5].ToString() +
                        //    pointerCode.Text[6].ToString() + pointerCode.Text[7].ToString();
                        //label5.Text = IntToHex(Convert.ToInt32(originalLength/2)) +  " " +  (pointerCode.Text.ToString().Length).ToString();
                        //label5.Text = regularCodeStartOffset +  " " +  bigGroup;

                        //label5.Text = regularCodeStartOffset + "aaa ";

                    }

                    change = HexToInt(regularCodeOffset);

                }

                //if (bigGroup.Contains( "B418"))
                if (bigGroup.Contains("18B4"))
                //if (bigGroup.Contains( "B418") || bigGroup.Contains( "18B4"))
                {
                    //{
                    //    errorLabel.Text += " " + bigGroup;

                    //}

                    //errorLabel.Text = ((bigGroup)) + " " + (modifiedMiddleOffset);
                    //errorLabel.Text = (HexToInt(bigGroup)).ToString() + " " + HexToInt(modifiedMiddleOffset).ToString();

                    //errorLabel.Text += " " + bigGroup;





                    //errorLabel.Text += " "+ (HexToInt(bigGroup) >= HexToInt(modifiedMiddleOffset)).ToString() +
                    //(HexToInt(bigGroup) < HexToInt(modifiedSecondPartStart)).ToString() +

                    ////&& (i * 8 ) >= HexToInt(originalMiddleOffset.Text)
                    ////&& (i * 8 ) < HexToInt(modifiedMiddleOffset)

                    //((i * 8 / 2) >= HexToInt(originalMiddleOffset.Text)).ToString() +
                    //((i * 8 / 2) < HexToInt(modifiedMiddleOffset)).ToString();
                }













                //else
                //{
                //    errorLabel.Text = (HexToInt(bigGroup)).ToString() + " " + HexToInt(modifiedMiddleOffset).ToString();

                //    errorLabel.Text += " " + bigGroup;
                //    //errorLabel.Text = (HexToInt(bigGroup) >= HexToInt(modifiedMiddleOffset)).ToString() +
                //    //(HexToInt(bigGroup) < HexToInt(modifiedSecondPartStart)).ToString() +

                //    ////&& (i * 8 ) >= HexToInt(originalMiddleOffset.Text)
                //    ////&& (i * 8 ) < HexToInt(modifiedMiddleOffset)

                //    //((i * 8 / 2) >= HexToInt(originalMiddleOffset.Text)).ToString() +
                //    //((i * 8 / 2) < HexToInt(modifiedMiddleOffset)).ToString();
                //}


                //string   =       ToHex (    ToInt(Hex)   +        Int     )
                //not this
                //change = 0;
                string newValueBig = IntToHex(HexToInt(bigGroup) + change).PadLeft(8, '0');


                if (newValueBig == "00500060")
                //if (newValueBig == "0050004C")
                {
                    errorLabel.Text += "TOP";
                }



                //label5.Text = newValueBig + " " + bigGroup + " " + change;


                //return newValueBig;
                char[] s = newValueBig.ToCharArray();


                char[] newValueLittleChar = { s[6], s[7], s[4], s[5], s[2], s[3], s[0], s[1] };


                //if (i == 2 * HexToInt(fileStart) / 8)
                //{
                //    label17.Text = newValueBig;
                //}



                byte[] newValueLittleBytes = new byte[newValueLittleChar.Length / 2];
                //label17.Text = newValueLittleChar[3].ToString();

                for (int k = 0; k < newValueLittleBytes.Length; k++)
                {
                    //label17.Text += k.ToString() + "p";
                    newValueLittleBytes[k] = Convert.ToByte(HexToInt(newValueLittleChar[2 * k].ToString() + newValueLittleChar[2 * k + 1].ToString()));
                    //newValueLittleBytes[k] = Convert.ToByte(HexToInt(newValueLittleChar[2 * k].ToString()) + HexToInt(newValueLittleChar[2 * k + 1].ToString()));
                    //label17.Text += k.ToString() + "p";
                }
                //string newValueLittle = s[6].ToString() + s[7].ToString() + s[4].ToString() + s[5].ToString() +
                //s[2].ToString() + s[3].ToString() + s[0].ToString() + s[1].ToString();



                for (int j = 0; j < 4; j++)
                {
                    dataList[i * 4 + j] = newValueLittleBytes[j];//todo change this if not working???
                }


                //char[] cArray = pointerCode.Text.ToCharArray();
                //byte[] pointerCodeBytes = new byte[cArray.Length];

                //for (int i = 0; i < cArray.Length; i++)
                //{
                //    pointerCodeBytes[i] = Convert.ToByte(HexToInt(cArray[i].ToString()));
                //}



                //bigGroup += IntToHex(dataList[i*4 + j]);
                //newFileCode = newFileCode.Remove((i * 8), 8).Insert((i * 8), newValueLittle);

                //newFileCode = newFileCode.Remove((i * 8 / 2), 8).Insert((i * 8 / 2), "AA");

                //label5.Text =  label5.Text + littleGroup ;

                if (regularCodeOffset != "")
                {
                    //label5.Text = newValueBig + " " + bigGroup + " " + IntToHex(change);
                    //label5.Text = newValueLittle + label5.Text + "bb ";
                }
                //label5.Text = littleGroup + label5.Text;

                //AdjustPointer(fs, i * 4, newValueLittle);

                //}

                //else if ( (i * 8 / 2) >= HexToInt(originalMiddleOffset.Text)
                //    )
                //{
                //    if (regularCodeOffset == "")
                //    {
                //        regularCodeOffset = IntToHex((i * 8 / 2) - HexToInt(bigGroup) );

                //    }




                //    //label5.Text = "NEWWWWWlittle: " + littleGroup + " big:" + bigGroup + label5.Text;
                //}

            }




















            bigGroup = "";
            littleGroup = "";
            regularCodeOffset = "";

            //not here
            //if (true) { } else
            if (!isScriptCharacterFile.Checked) { }
            else for (int i = 2 * HexToInt(originalSecondPartStart.Text) / 8; i < 2 * HexToInt(modifiedMiddleOffset2) / 8; i++)
                //for (int i = 0; i < 2 * HexToInt(modifiedMiddleOffset) / 8; i++)
                {


                    bigGroup = "";
                    littleGroup = "";
                    for (int j = 3; j >= 0; j--)
                    {
                        //button1.Text = bigGroup.ToString();
                        //swapped to big endian

                        //string toAdd = IntToHex(data[i * 4 + j]).ToString().PadLeft(2, '0');
                        //if (toAdd.Length < 2) { toAdd = "0" + toAdd; }
                        //bigGroup += toAdd;
                        //label5.Text += toAdd + " ";


                        bigGroup += IntToHex(dataList[i * 4 + j]).PadLeft(2, '0');
                        //bigGroup += newFileCode[i * 8 + 2 * j].ToString() + newFileCode[i * 8 + 2 * j + 1].ToString();
                    }

                    //label5.Text = bigGroup + IntToHex( i*8)+ " "  + label5.Text;
                    //label5.Text += bigGroup + "a";

                    for (int j = 0; j < 8; j++)
                    {
                        //string toAdd = IntToHex(data[i * 4 + j]).ToString().PadLeft(2, '0');
                        //if (toAdd.Length < 2) { toAdd = "0" + toAdd; }
                        //littleGroup += toAdd;


                        //swapped to big endian
                        //littleGroup += IntToHex(data[i * 4 + j]).ToString();

                        littleGroup += IntToHex(dataList[i * 4 + j]).PadLeft(2, '0');
                        //littleGroup += newFileCode[i * 8 + j];
                    }

                    //label5.Text = littleGroup + IntToHex(i * 8 / 2) + " " + label5.Text;
                    //label5.Text = littleGroup+ " " + label5.Text;
                    //bigGroup =




                    ////todo fix all this stuff for character script second part
                    //if (HexToInt(bigGroup) >= HexToInt(originalMiddleOffset2.Text)
                    //    && HexToInt(bigGroup) < originalLength/2
                    //    && (i * 8 / 2) < HexToInt(originalMiddleOffset2.Text))
                    //{
                    //    //label5.Text = (HexToInt(bigGroup) < modifiedLength).ToString();
                    //    //label5.Text =  modifiedMiddleOffset +" "+ label5.Text;
                    //    //label5.Text = "little: " + littleGroup + " big:" + bigGroup + label5.Text;
                    //    //label5.Text += "little: " + littleGroup + " big:" + bigGroup;
                    //    //result += "little: " + littleGroup + " big:" + bigGroup;
                    //}



                    //else if ((HexToInt(bigGroup) > HexToInt(modifiedMiddleOffset)
                    //    && HexToInt(bigGroup) < modifiedLength)
                    //    && (i * 8 / 2) >= HexToInt(originalMiddleOffset.Text)
                    //{
                    //    label5.Text = "NEWWWWWlittle: " + littleGroup + " big:" + bigGroup +label5.Text;
                    //}



                    //if (HexToInt(bigGroup) >= HexToInt(modifiedMiddleOffset)
                    //    && HexToInt(bigGroup) < modifiedLength)
                    //{
                    //    result += "little: "+littleGroup + " big:" + bigGroup;
                    //} 





                    int change = 0;

                    //not here
                    //if (true) { } else
                    //for changing old pointers
                    if (HexToInt(bigGroup) >= HexToInt(originalMiddleOffset2.Text)
                        //&& a
                        && HexToInt(bigGroup) < originalLength
                        //&& HexToInt(bigGroup) < originalLength / 2

                        && (i * 8 / 2) < HexToInt(originalMiddleOffset2.Text) + pointerCode.Text.Length
                        //&& (i * 8 / 2) < HexToInt(originalMiddleOffset2.Text) 
                        //&& (i * 8 / 2) < HexToInt(modifiedMiddleOffset2)
                        && !((i * 8 / 2) >= HexToInt(originalMiddleOffset2.Text) + pointerCode.Text.Length)
                        //&& !((i * 8 / 2) >= HexToInt(originalMiddleOffset2.Text) && (regularCode2.Text != ""))

                        && (bigGroup != "0050004C")

                        )

                    {
                        //label5.Text = IntToHex(i*8/2) + "_" + (originalMiddleOffset.Text) + label5.Text;
                        //label5.Text = (i*8/2).ToString() + "_" + HexToInt(originalMiddleOffset.Text) + label5.Text;

                        //int      =  ToInt  ( Hex                  ) -    ToInt (Hex      )
                        change = (HexToInt(modifiedMiddleOffset2) - HexToInt(originalMiddleOffset2.Text));
                        //return change.ToString();
                    }

                    //todo add box to choose new pointer code location
                    //todo mixing old and new pointers???
                    //todo cleanup *2 /2 stuff

                    //for matching new bottom code
                    else
                    if (

                    (bigGroup != "0050004C") &&

                    HexToInt(bigGroup) >= HexToInt(fileList[fileListLocation[0], 4])
                    //&& a


                    //todo check here if not working. don't need this???
                    && HexToInt(bigGroup) < originalLength


                    //&& HexToInt(bigGroup) < originalLength / 2
                    //&& HexToInt(bigGroup) < HexToInt(fileList[fileListLocation[0], 4])
                    //HexToInt(bigGroup) >= HexToInt(modifiedMiddleOffset)
                    //&& HexToInt(bigGroup) < HexToInt(modifiedSecondPartStart)

                    && (i * 8 / 2) >= HexToInt(originalMiddleOffset2.Text)
                    && (i * 8 / 2) < HexToInt(modifiedMiddleOffset2)

                    && regularCode.Text != "")
                    //todo check if 2 needs to be here for character script
                    //&& regularCode2.Text != "")
                    //if ((i * 8 / 2) < HexToInt(modifiedMiddleOffset2)
                    //    && regularCode2.Text != "")
                    {
                        //error1.Text += ((i * 8 / 2) < HexToInt(originalMiddleOffset2.Text) + pointerCode.Text.Length).ToString() + " ";
                        //error1.Text += ((i * 8 / 2) < HexToInt(originalMiddleOffset2.Text)+ pointerCode.Text.Length + regularCode.Text.Length).ToString() + " ";
                        //error1.Text += ((i * 8 / 2) + ","+ HexToInt(originalMiddleOffset2.Text)).ToString() + " ";
                        //error1.Text = originalMiddleOffset2 + " " + modifiedMiddleOffset2;
                        //error1.Text += IntToHex(i * 8 / 2) + "," + ;

                        //test += 1;
                        //error1.Text += test.ToString();
                        //label5.Text += "a";
                        if (regularCodeOffset == "")
                        {
                            //todo see why pointerCode.Text.Length is 6

                            //regularCodeOffset = IntToHex(HexToInt(bigGroup) - (i * 8 / 2));

                            //string regularCodeStartOffset = IntToHex(originalLength / 2);
                            //+ pointerCode.Text.ToString().Length/2 + 
                            //regularCode.Text.ToString().Length/2 +
                            //pointerCode2.Text.ToString().Length / 2 );

                            //string regularCodeStartOffset = IntToHex(modifiedLength/2 + pointerCode.Text.ToString().Length);
                            //a
                            //todo next remove /2 from regularCode.Text.Length / 2 ??????
                            string regularCodeStartOffset = IntToHex(originalLength + cumulativeIncrease - regularCode.Text.Length / 2);
                            //string regularCodeStartOffset = IntToHex(originalLength / 2 + cumulativeIncrease - regularCode.Text.Length / 2);


                            //string regularCodeStartOffset = IntToHex(originalLength / 2 + cumulativeIncrease - regularCode2.Text.Length / 2);
                            //string regularCodeStartOffset = IntToHex(originalLength/2 + pointerCode2.Text.ToString().Length / 2 );
                            //string regularCodeStartOffset = IntToHex(Convert.ToInt32(originalLength / 2) + pointerCode.Text.ToString().Length);

                            regularCodeOffset = IntToHex(HexToInt(regularCodeStartOffset) - HexToInt(bigGroup));
                            //regularCodeOffset = IntToHex(HexToInt(bigGroup) - HexToInt(regularCodeStartOffset) );

                            //label5.Text = pointerCode.Text[0].ToString() + pointerCode.Text[1].ToString() + pointerCode.Text[2].ToString() +
                            //    pointerCode.Text[3].ToString() + pointerCode.Text[4].ToString() + pointerCode.Text[5].ToString() +
                            //    pointerCode.Text[6].ToString() + pointerCode.Text[7].ToString();
                            //label5.Text = IntToHex(Convert.ToInt32(originalLength/2)) +  " " +  (pointerCode.Text.ToString().Length).ToString();
                            //label5.Text = regularCodeStartOffset +  " " +  bigGroup;

                            //label5.Text = regularCodeStartOffset + "aaa ";

                        }

                        change = HexToInt(regularCodeOffset);

                    }

                    //string   =       ToHex (    ToInt(Hex)   +        Int     )
                    //not this
                    //change = 0;

                    string newValueBig = IntToHex(HexToInt(bigGroup) + change).PadLeft(8, '0');

                    if (newValueBig == "00500060")
                    //if (newValueBig == "0050004C")
                    {
                        errorLabel.Text += "BOTTOM";
                    }
                    //label5.Text = newValueBig + " " + bigGroup + " " + change;


                    //return newValueBig;
                    char[] s = newValueBig.ToCharArray();



                    char[] newValueLittleChar = { s[6], s[7], s[4], s[5], s[2], s[3], s[0], s[1] };
                    byte[] newValueLittleBytes = new byte[newValueLittleChar.Length / 2];
                    //label17.Text = newValueLittleChar[3].ToString();

                    for (int k = 0; k < newValueLittleBytes.Length; k++)
                    {
                        //label17.Text += k.ToString() + "p";
                        newValueLittleBytes[k] = Convert.ToByte(HexToInt(newValueLittleChar[2 * k].ToString() + newValueLittleChar[2 * k + 1].ToString()));
                        //newValueLittleBytes[k] = Convert.ToByte(HexToInt(newValueLittleChar[2 * k].ToString()) + HexToInt(newValueLittleChar[2 * k + 1].ToString()));
                        //label17.Text += k.ToString() + "p";
                    }


                    //char[] newValueLittleChar = { s[6], s[7], s[4], s[5], s[2], s[3], s[0], s[1] };
                    //byte[] newValueLittleBytes = new byte[newValueLittleChar.Length];

                    //for (int k = 0; k < newValueLittleChar.Length; i++)
                    //{
                    //    newValueLittleBytes[i] = Convert.ToByte(HexToInt(newValueLittleChar[i].ToString()));
                    //}
                    //string newValueLittle = s[6].ToString() + s[7].ToString() + s[4].ToString() + s[5].ToString() +
                    //s[2].ToString() + s[3].ToString() + s[0].ToString() + s[1].ToString();



                    for (int j = 0; j < 4; j++)
                    {
                        dataList[i * 4 + j] = newValueLittleBytes[j];//todo change this if not working???
                    }







                    //string newValueLittle = s[6].ToString() + s[7].ToString() + s[4].ToString() + s[5].ToString() +
                    //    s[2].ToString() + s[3].ToString() + s[0].ToString() + s[1].ToString();


                    //newFileCode = newFileCode.Remove((i * 8 / 2), 8).Insert((i * 8 / 2), "AA");
                    //newFileCode = newFileCode.Remove((i * 8), 8).Insert((i * 8), newValueLittle);

                    //label5.Text =  label5.Text + littleGroup ;

                    if (regularCodeOffset != "")
                    {
                        //label5.Text = newValueBig + " " + bigGroup + " " + IntToHex(change);
                        //label5.Text = newValueLittle + label5.Text + "bb ";
                    }
                    //label5.Text = littleGroup + label5.Text;

                    //AdjustPointer(fs, i * 4, newValueLittle);

                    //}

                    //else if ( (i * 8 / 2) >= HexToInt(originalMiddleOffset.Text)
                    //    )
                    //{
                    //    if (regularCodeOffset == "")
                    //    {
                    //        regularCodeOffset = IntToHex((i * 8 / 2) - HexToInt(bigGroup) );

                    //    }




                    //    //label5.Text = "NEWWWWWlittle: " + littleGroup + " big:" + bigGroup + label5.Text;
                    //}

                }












































            //label5.Text = newFileCode;

            //char[] charArray = newFileCode.ToCharArray();
            //Array.Reverse(charArray);
            //label5.Text = new string(charArray);


            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            //FileStream fs = new FileStream(modifiedPath.Text, FileMode.Open, FileAccess.ReadWrite);


            byte[] data = new byte[100];
            fs.Read(data, 0, 100);

            //label17.Text = dataList.Count.ToString();
            //label17.Text +=" bb"+ dataList.Count.ToString();

            //todo22 find out why need to add cumulative here
            //error1.Text = cumulativeIncrease.ToString();
            //for (int i = 0; i < modifiedLength; i++)
            //error1.Text = cumulativeIncrease.ToString();

            //for (int i = 0; i < dataList.Count/2; i++)



            //errorLabel.Text += IntToHex( dataList[dataList.Count-2])+ "asdf";
            int count = 0;

            for (int i = 0; i < dataList.Count; i++)
            //for (int i = 0; i < dataList.Count + cumulativeIncrease -2 ; i++)
            //for (int i = 0; i < dataList.Count / 2 + cumulativeIncrease / 2; i++)
            {
                count += 1;
                //errorLabel.Text = i + " ";

                fs.Position = i;
                byte byteToWrite = dataList[i];
                fs.WriteByte(byteToWrite);

                /*
                //byte newByte = byte.Parse((newFileCode[i].ToString() + newFileCode[i + 1].ToString()));
                byte newByte = byte.Parse((HexToInt(newFileCode[2 * i].ToString() + newFileCode[2 * i + 1].ToString())).ToString());
                //byte newByte = byte.Parse((HexToInt(newFileCode[i].ToString() )).ToString());
                //label5.Text += newFileCode[i].ToString() + newFileCode[i + 1].ToString() ;
                //label5.Text += (HexToInt(newFileCode[i].ToString() + newFileCode[i + 1].ToString())).ToString();

                fs.Position = i;
                fs.WriteByte(newByte);
                */

            }

            //errorLabel.Text = count + " "+(dataList.Count) + " ";
            //error1.Text = IntToHex(cumulativeIncrease);
            //fs.WriteByte(3);
            //label5.Text = newFileCode;








            //todo check if needed for other file types? check here if not working
            //changes file size after insertion

            

            if (modifiedPath.Text.Contains(".mot"))
            {

                //ChangeEntryTotal("34", data, fs);
                //todo for mot only?
                int currentOffset = HexToInt("34");
                string sizeBlock = "";
                byte[] baseModelBoneNumberBytes = new byte[4];

                for (int i = 0; i < 4; i++)
                {


                    //fs.Position += i;

                    sizeBlock += (IntToHex(data[currentOffset + 3 - i])).PadLeft(2, '0');
                    //textBoxMOTDataTop.Text += (IntToHex(data[currentOffset + i])).PadLeft(2, '0');
                    //newByte += (IntToHex(data[i])).PadLeft(2, '0');
                    baseModelBoneNumberBytes[i] = data[currentOffset + i];

                }
                int baseModelBoneNumber = HexToInt(sizeBlock);



                //string newBoneBig = (sizeBlock + Convert.ToInt32(timesToAdd.Text)).PadLeft(8, '0');
                string newBoneBig = IntToHex(HexToInt(sizeBlock) + Convert.ToInt32(timesToAdd.Text) * Convert.ToInt32(howManyInOneAdd.Text)).PadLeft(8, '0');
                //string newBoneBig = IntToHex(HexToInt(sizeBlock) + Convert.ToInt32(timesToAdd.Text)*Convert.ToInt32(howManyInOneAdd.Text)).PadLeft(8, '0');
                //string newBoneBig = IntToHex(HexToInt(sizeBlock) + Convert.ToInt32(timesToAdd.Text)).PadLeft(8, '0');





                //label5.Text = newValueBig + " " + bigGroup + " " + change;


                //return newValueBig;
                char[] bs = newBoneBig.ToCharArray();


                char[] newBoneLittleChar = { bs[6], bs[7], bs[4], bs[5], bs[2], bs[3], bs[0], bs[1] };


                //if (i == 2 * HexToInt(fileStart) / 8)
                //{
                //    label17.Text = newValueBig;
                //}



                byte[] newBoneLittleBytes = new byte[newBoneLittleChar.Length / 2];
                //label17.Text = newValueLittleChar[3].ToString();

                for (int k = 0; k < newBoneLittleBytes.Length; k++)
                {
                    //label17.Text += k.ToString() + "p";
                    newBoneLittleBytes[k] = Convert.ToByte(HexToInt(newBoneLittleChar[2 * k].ToString() + newBoneLittleChar[2 * k + 1].ToString()));
                    //newValueLittleBytes[k] = Convert.ToByte(HexToInt(newValueLittleChar[2 * k].ToString()) + HexToInt(newValueLittleChar[2 * k + 1].ToString()));
                    //label17.Text += k.ToString() + "p";
                }
                //string newValueLittle = s[6].ToString() + s[7].ToString() + s[4].ToString() + s[5].ToString() +
                //s[2].ToString() + s[3].ToString() + s[0].ToString() + s[1].ToString();






                currentOffset = HexToInt("34");
                for (int i = 0; i < 4; i++)
                {
                    fs.Position = currentOffset + i;
                    fs.WriteByte(newBoneLittleBytes[i]);

                }



            }

            else
            {
                //ChangeEntryTotal("08", data, fs);



                int currentOffset = HexToInt("8");
                string sizeBlock = "";
                byte[] baseModelBoneNumberBytes = new byte[4];

                for (int i = 0; i < 4; i++)
                {


                    //fs.Position += i;

                    sizeBlock += (IntToHex(data[currentOffset + 3 - i])).PadLeft(2, '0');
                    //textBoxMOTDataTop.Text += (IntToHex(data[currentOffset + i])).PadLeft(2, '0');
                    //newByte += (IntToHex(data[i])).PadLeft(2, '0');
                    baseModelBoneNumberBytes[i] = data[currentOffset + i];

                }
                int baseModelBoneNumber = HexToInt(sizeBlock);



                //string newBoneBig = (sizeBlock + Convert.ToInt32(timesToAdd.Text)).PadLeft(8, '0');
                string newBoneBig = IntToHex(HexToInt(sizeBlock) + Convert.ToInt32(timesToAdd.Text) * Convert.ToInt32(howManyInOneAdd.Text)).PadLeft(8, '0');
                //string newBoneBig = IntToHex(HexToInt(sizeBlock) + Convert.ToInt32(timesToAdd.Text)*Convert.ToInt32(howManyInOneAdd.Text)).PadLeft(8, '0');
                //string newBoneBig = IntToHex(HexToInt(sizeBlock) + Convert.ToInt32(timesToAdd.Text)).PadLeft(8, '0');





                //label5.Text = newValueBig + " " + bigGroup + " " + change;


                //return newValueBig;
                char[] bs = newBoneBig.ToCharArray();


                char[] newBoneLittleChar = { bs[6], bs[7], bs[4], bs[5], bs[2], bs[3], bs[0], bs[1] };


                //if (i == 2 * HexToInt(fileStart) / 8)
                //{
                //    label17.Text = newValueBig;
                //}



                byte[] newBoneLittleBytes = new byte[newBoneLittleChar.Length / 2];
                //label17.Text = newValueLittleChar[3].ToString();

                for (int k = 0; k < newBoneLittleBytes.Length; k++)
                {
                    //label17.Text += k.ToString() + "p";
                    newBoneLittleBytes[k] = Convert.ToByte(HexToInt(newBoneLittleChar[2 * k].ToString() + newBoneLittleChar[2 * k + 1].ToString()));
                    //newValueLittleBytes[k] = Convert.ToByte(HexToInt(newValueLittleChar[2 * k].ToString()) + HexToInt(newValueLittleChar[2 * k + 1].ToString()));
                    //label17.Text += k.ToString() + "p";
                }
                //string newValueLittle = s[6].ToString() + s[7].ToString() + s[4].ToString() + s[5].ToString() +
                //s[2].ToString() + s[3].ToString() + s[0].ToString() + s[1].ToString();






                currentOffset = HexToInt("8");
                for (int i = 0; i < 4; i++)
                {
                    fs.Position = currentOffset + i;
                    fs.WriteByte(newBoneLittleBytes[i]);

                }


            }










            fs.Close();
            //return result;

        }


        private void ChangeEntryTotal(string entryTotalLocation, byte[] data, FileStream fs)
        {
            //todo for mot only?
                int currentOffset = HexToInt(entryTotalLocation);
                //int currentOffset = HexToInt("34");
                string sizeBlock = "";
                byte[] baseModelBoneNumberBytes = new byte[4];

                for (int i = 0; i < 4; i++)
                {


                    //fs.Position += i;

                    sizeBlock += (IntToHex(data[currentOffset + 3 - i])).PadLeft(2, '0');
                    //textBoxMOTDataTop.Text += (IntToHex(data[currentOffset + i])).PadLeft(2, '0');
                    //newByte += (IntToHex(data[i])).PadLeft(2, '0');
                    baseModelBoneNumberBytes[i] = data[currentOffset + i];

                }
                int baseModelBoneNumber = HexToInt(sizeBlock);



                //string newBoneBig = (sizeBlock + Convert.ToInt32(timesToAdd.Text)).PadLeft(8, '0');
                string newBoneBig = IntToHex(HexToInt(sizeBlock) + Convert.ToInt32(timesToAdd.Text) * Convert.ToInt32(howManyInOneAdd.Text)).PadLeft(8, '0');
                //string newBoneBig = IntToHex(HexToInt(sizeBlock) + Convert.ToInt32(timesToAdd.Text)*Convert.ToInt32(howManyInOneAdd.Text)).PadLeft(8, '0');
                //string newBoneBig = IntToHex(HexToInt(sizeBlock) + Convert.ToInt32(timesToAdd.Text)).PadLeft(8, '0');





                //label5.Text = newValueBig + " " + bigGroup + " " + change;


                //return newValueBig;
                char[] bs = newBoneBig.ToCharArray();


                char[] newBoneLittleChar = { bs[6], bs[7], bs[4], bs[5], bs[2], bs[3], bs[0], bs[1] };


                //if (i == 2 * HexToInt(fileStart) / 8)
                //{
                //    label17.Text = newValueBig;
                //}



                byte[] newBoneLittleBytes = new byte[newBoneLittleChar.Length / 2];
                //label17.Text = newValueLittleChar[3].ToString();

                for (int k = 0; k < newBoneLittleBytes.Length; k++)
                {
                    //label17.Text += k.ToString() + "p";
                    newBoneLittleBytes[k] = Convert.ToByte(HexToInt(newBoneLittleChar[2 * k].ToString() + newBoneLittleChar[2 * k + 1].ToString()));
                    //newValueLittleBytes[k] = Convert.ToByte(HexToInt(newValueLittleChar[2 * k].ToString()) + HexToInt(newValueLittleChar[2 * k + 1].ToString()));
                    //label17.Text += k.ToString() + "p";
                }
                //string newValueLittle = s[6].ToString() + s[7].ToString() + s[4].ToString() + s[5].ToString() +
                //s[2].ToString() + s[3].ToString() + s[0].ToString() + s[1].ToString();




                currentOffset = HexToInt("entryTotalLocation");
                //currentOffset = HexToInt("34");
                for (int i = 0; i < 4; i++)
                {
                    fs.Position = currentOffset + i;
                    fs.WriteByte(newBoneLittleBytes[i]);

                }



        }

        private string IntToHex(int integer)
        {
            return integer.ToString("X");
        }

        private int HexToInt(string str)
        {
            return Int32.Parse(str, System.Globalization.NumberStyles.HexNumber);
        }

        private long HexTo64(string str)
        {
            return Int64.Parse(str, System.Globalization.NumberStyles.HexNumber);
        }

        private float HexToFloat(string str)
        {
            return float.Parse(str, System.Globalization.NumberStyles.HexNumber);
        }

        private string HexPlusHexBig(string hex1, string hex2)
        {
            return IntToHex(HexToInt(hex1) + HexToInt(hex2));
        }

        private string HexMinusHexBig(string hex1, string hex2)
        {
            return IntToHex(HexToInt(hex1) - HexToInt(hex2));
        }

        private void AdjustPointer(FileStream fs, int offset, string newValue)
        {
            for (int i = 0; i < 4; i++)
            {
                //byte newbyte = 3;
                byte newByte = byte.Parse((HexToInt(newValue[i * 2].ToString() + newValue[i * 2 + 1].ToString())).ToString());

                fs.Position = offset + i;
                fs.WriteByte(newByte);
                //fs.WriteByte(Convert.ToByte(newValue[i]));
                //fs.WriteByte(newValue[i]);
                //fs.Write(newValue, offset, 4);
                //fs.write
            }



        }

        private void ToggleCharacterStuff()
        {
            label1.Enabled = !label1.Enabled;
            originalSecondPartStart.Enabled = !originalSecondPartStart.Enabled;
            label7.Enabled = !label7.Enabled;
            label13.Enabled = !label13.Enabled;
            label6.Enabled = !label6.Enabled;
            pointerCode2.Enabled = !pointerCode2.Enabled;
            injectLocation2.Enabled = !injectLocation2.Enabled;
            regularCode2.Enabled = !regularCode2.Enabled;
            //overwrite2.Enabled = !overwrite2.Enabled;
            originalMiddleOffset2.Enabled = !originalMiddleOffset2.Enabled;
            label15.Enabled = !label15.Enabled;
            //checkBox3.Enabled = !checkBox3.Enabled;

        }

        private void ReadPatch()
        {
            //todo disable patch/normal file stuff when not using the other????
            //todo add patch error stuff
            //todo add more error stuff
            //string[,] patchArray;
            //patchArray = System.IO.File.ReadAllText(s[0]);
            //pointerCode.Text = Regex.Replace(patchPath.Text, @"\s+", "");

        }


        private void ApplyPatch()
        {
            //foreach (string[] singlePatch in patchArray)
            //{
            //    ReadPatch();
            //    //todo add patch filepath field, and apply patch button (can use same apply button???)
            //    //todo change all needed values here


            //    ApplyCode();
            //}



        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ToggleCharacterStuff();
        }



        private void button1_Click(object sender, EventArgs e)
        //private void ApplyCode()
        {
            originalMiddleOffset.Enabled = true;
            ClearErrors();
            ClearOldValues();
            label17.Text += "started...";
            //if (modifiedPath.Text == "")
            filePath = modifiedPath.Text;
            //todo get rid of this stuff???
            if (isScriptCharacterFile.Checked)
            {
                fileStart = "20";
            }
            else { fileStart = "0"; }
            GetFileInfo(modifiedPath.Text);
            //label17.Text += "infoGot...";

            ApplyCode(modifiedPath.Text);
        }






        private void button2_Click(object sender, EventArgs e)
        {
            pointerCode.Text = Regex.Replace(pointerCode.Text, @"\s+", "");
            regularCode.Text = Regex.Replace(regularCode.Text, @"\s+", "");

            pointerCode2.Text = Regex.Replace(pointerCode2.Text, @"\s+", "");
            regularCode2.Text = Regex.Replace(regularCode2.Text, @"\s+", "");


            string[] filePaths = Directory.GetFiles(folderDirectory.Text, "*.mot",
                                                     SearchOption.TopDirectoryOnly);

            //for (int i = 0; i < Convert.ToInt32(timesToAdd.Text); i++)
            //{

            //    foreach (string singleFilePath in filePaths)
            //    {

            //        //todo next find out why it's not correcting pointers for mass .mot change

            //        filePath = singleFilePath;

            //        ClearErrors();
            //        label17.Text += "started...";
            //        //todo get rid of this stuff???
            //        if (isScriptCharacterFile.Checked)
            //        {
            //            fileStart = "20";
            //        }
            //        //else if (checkBoxMot.Checked == true) { fileStart = "3C"; }
            //        else { fileStart = "0"; }
            //        GetFileInfo(filePath);

            //        ApplyCode(filePath);


            //        //errorLabel.Text += " origin:"+ originalLength +" modified:" +modifiedLength;


            //    }
            //}





            string pointerCodeOne = pointerCode.Text;
            string pointerCodePlaceholder = pointerCode.Text;
            string regularCodeOne = regularCode.Text;
            string regularCodePlaceholder = regularCode.Text;






            for (int i = 0; i < Convert.ToInt32(timesToAdd.Text); i++)
            {














                pointerCodePlaceholder += pointerCodeOne;
                regularCodePlaceholder += regularCodeOne;
            }







            //"70000000 02000000 50000000 02000000 98000000 02000000";
            // length of each 20000000 28000000 20000000
            // total length 68000000


            if (Convert.ToInt32(timesToAdd.Text) > 1)
            {

                //string input = "123456782234567832345678";
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < pointerCodePlaceholder.Length; i++)
                {
                    if (i % 8 == 0 && i != 0)
                        sb.Append(',');
                    sb.Append(pointerCodePlaceholder[i]);
                }
                string formatted = sb.ToString();

                string[] formattedArray;
                formattedArray = formatted.Split(',');


                pointerCodePlaceholder = "";
                for (int i = 0; i < formattedArray.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        //todo next fix this so it's adding by the correct endianess
                        string littlePlace = formattedArray[i][6].ToString() + formattedArray[i][7] + formattedArray[i][4] + formattedArray[i][5] +
                                                      formattedArray[i][2] + formattedArray[i][3] + formattedArray[i][0] + formattedArray[i][1];

                        string pointerCodeLittle = "";
                        pointerCodeLittle += IntToHex(HexToInt(littlePlace) + (HexToInt("00000068") * (i / 6))).PadLeft(8, '0');

                        pointerCodePlaceholder += pointerCodeLittle[6].ToString() + pointerCodeLittle[7] + pointerCodeLittle[4] + pointerCodeLittle[5] + 
                                                 pointerCodeLittle[2] + pointerCodeLittle[3] + pointerCodeLittle[0] + pointerCodeLittle[1];

                        //pointerCodePlaceholder += IntToHex(HexToInt(formattedArray[i]) + (HexToInt("68000000") * (i / 6))).PadLeft(8, '0');
                        //formattedArray[i] = IntToHex(HexToInt(formattedArray[i]) + (HexToInt("68000000") * (i / 6))).PadLeft(8, '0');
                    }
                    else
                    {
                        pointerCodePlaceholder += formattedArray[i];
                    }
                }




                //for ()
                //pointerCodePlaceholder =

                //button3.Text = formattedArray[0] + " " + formattedArray[1] + " " + formattedArray[2];




            }










            pointerCode.Text = pointerCodePlaceholder;
            regularCode.Text = regularCodePlaceholder;



            foreach (string singleFilePath in filePaths)
            {

                //todo next find out why it's not correcting pointers for mass .mot change

                filePath = singleFilePath;

                ClearErrors();
                
                label17.Text += "started...";
                //todo get rid of this stuff???
                if (isScriptCharacterFile.Checked)
                {
                    fileStart = "20";
                }
                //else if (checkBoxMot.Checked == true) { fileStart = "3C"; }
                else { fileStart = "0"; }
                GetFileInfo(filePath);

                ApplyCode(filePath);


                //errorLabel.Text += " origin:"+ originalLength +" modified:" +modifiedLength;


            }















        }

        private void button3_Click(object sender, EventArgs e)
        {

            button3.Text = IntToHex(HexToInt("02000000") + HexToInt("68000000") * (3 / 8)).PadLeft(8, '0');




            string input = "123456782234567832345678";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (i % 8 == 0 && i != 0)
                    sb.Append(',');
                sb.Append(input[i]);
            }
            string formatted = sb.ToString();

            string[] formattedArray;
            formattedArray = formatted.Split(',');


            string littlePlace = formattedArray[0][6].ToString() + formattedArray[0][7] + formattedArray[0][4] + formattedArray[0][5] +
                                          formattedArray[0][2] + formattedArray[0][3] + formattedArray[0][0] + formattedArray[0][1];



            button3.Text = littlePlace;
            //button3.Text = formattedArray[0] + " " + formattedArray[1] + " " + formattedArray[2];
        }
        ////todo next put this 
    }
}
